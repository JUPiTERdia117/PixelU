using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GM : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPoints, spawnPointsL2;

    //Tiempos (segundos) de cada seccion
    [SerializeField] float tL1, tL2, tL3, tDescanso, tL4, tiempoADA; 

    PixelSpawner pixelSP;

    List<GameObject> SP_List, SP_ListL2, Pixels_List;

    static public List<GameObject> PixelsToShield_List; //Lista de pixeles disponibles para dar escudos.

    [SerializeField] int level1Q=4, level2Q, level3Q; // Cantidad maxima de pixeles a spawnear en nivel 1

    [SerializeField] GameObject glitch; //El glitch

    int currentPixelQ = 0;//Cantidad actual de pixeles

    int aviableSpawnQ = 0; //Cantidad disponible de spawns para cada nivel

    //Tiempo de aparicion y desaparicion de pixeles
    [SerializeField] int tDesaparicion, tAparicion, tDesaparicionL2;

    [SerializeField] float tActivacionGL1, tActivacionGL2, tActivacionGL3;

    //Variables que sirven para tener control del tiempo
    private float tiempoTranscurrido = 0f;
    private int minutos, segundos, centesimas, segundosTotales;

    bool level1Started = false, level2Started = false, level3Started = false, freeTStarted = false, level4Started = false;

    GlitchController glitchC;

    [SerializeField] GameObject TXTL1,TXTL2,TXTL3,TXT4,TXTDESCANSO,TXTADA;


    void Awake(){
        //Lista que contiene posibles puntos de spawneo de pixeles
        SP_List = new List<GameObject>(spawnPoints);
        SP_ListL2 = new List<GameObject>(spawnPointsL2);
        Pixels_List = new List<GameObject>();
        PixelsToShield_List = new List<GameObject>();

        glitchC =glitch.GetComponent<GlitchController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

        tiempoTranscurrido += Time.deltaTime;
        minutos = (int)(tiempoTranscurrido/60f);
        segundos = (int)(tiempoTranscurrido - minutos*60f);
        centesimas = (int)((tiempoTranscurrido - (int)tiempoTranscurrido)*100f);

        segundosTotales = (minutos*60 + segundos);

        
        if(!level1Started){

            //Llega glitch
            //Empieza nivel 1
            Level1();
        }
        if(segundosTotales>tL1 && !level2Started){

            
            Level2();

        }
            
        if(segundosTotales>tL1+tL2 && !level3Started){

            Level3();
                
            
                
        }
        if(segundosTotales>tL1+tL2+tL3 && !freeTStarted){

        }
        if(segundosTotales>tL1+tL2+tL3+tDescanso && !level4Started){

        }

        //Comprueba si hubo un "click"
        if(Input.GetMouseButtonDown(0)){
               
            Vector3 mousePos = Input.mousePosition;

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {

                if(hit.collider.tag == "Enemy"){
                    GameObject pixelGolpeado = hit.collider.gameObject;
                    PixelController pController = pixelGolpeado.GetComponent<PixelController>();
                    if(pController.QuitarVida()==0){
                        GameObject pixelParent = pixelGolpeado.transform.parent.gameObject;
                        Pixels_List.Remove(pixelGolpeado);
                        PixelsToShield_List.Remove(pixelGolpeado);
                        currentPixelQ--;
                        SP_List.Add(pixelParent);

                    }
                    else{

                        PixelsToShield_List.Add(pixelGolpeado);

                    }

                                    
                }
                if(hit.collider.tag == "Glitch"){

                    glitchC.QuitarVida();

                }
            }
        }
        
    }


    void Level1(){

        level1Started = true;
        Debug.Log("Nivel 1 iniciado");

        aviableSpawnQ = level1Q;

        if (spawnPoints.Length != 0){
            
            if(spawnPoints.Length >= level1Q)
            {
                //Spawnea la cantidad requerida, sin repetir
                for(int i=0; i<level1Q; i++){

                    CrearPixel();

                }
            }
            else{
                Debug.LogWarning("No hay la suficiente cantidad de spawns");
            }

            InvokeRepeating("DesaparecerPixel", 5f, tDesaparicion);
            InvokeRepeating("AparecerPixel", 5f, tAparicion);
            
        }

       

        StartCoroutine(ActivarG(5.0f, tActivacionGL1));

    }

    void Level2(){

        TXTL1.SetActive(false);
        TXTL2.SetActive(true);
        
        level2Started = true;
        Debug.Log("Nivel 1 terminado");
        Debug.Log("Nivel 2 iniciado");

        glitchC.DesactivarGlitch();

        DeleteAllPixels();

        CancelInvoke("DesaparecerPixel");
        CancelInvoke("AparecerPixel");
        
        

        InvokeRepeating("DeleteAllPixels", tDesaparicionL2,tDesaparicionL2);
        InvokeRepeating("CrearPixelL2", 0.0f,tDesaparicionL2+0.5f);
            
        

        StartCoroutine(ActivarG(5.0f, tActivacionGL2));

        

        



    }

    void Level3(){

        TXTL2.SetActive(false);
        TXTL3.SetActive(true);
        
        level3Started = true;
        Debug.Log("Nivel 2 terminado");
        Debug.Log("Nivel 3 iniciado");

        glitchC.DesactivarGlitch();

        DeleteAllPixels();

        CancelInvoke("DeleteAllPixels");
        CancelInvoke("CrearPixelL2");

    }

    void Level4(){

    }

    void CrearPixel(){

        

        //Si no se ha superado la cantidad permitida de pixeles entonces crea uno 
        if(currentPixelQ < aviableSpawnQ)
        {
            //Debug.Log("Creando pixel...");


            int randomIndex = Random.Range(0, SP_List.Count); // Elegir un índice aleatorio
            GameObject randomObject = SP_List[randomIndex]; // Obtener el objeto correspondiente

            pixelSP = randomObject.GetComponent<PixelSpawner>();

            GameObject pixelCreado = pixelSP.SpawnPixel();

            SP_List.RemoveAt(randomIndex);

            Pixels_List.Add(pixelCreado);
            PixelsToShield_List.Add(pixelCreado);

            
            
            

            currentPixelQ++;

            
        }
        else{
            Debug.LogWarning("Cantidad máxima de pixeles para este nivel");
            

        }



    }

    void CrearPixelL2(){

        
        if (spawnPointsL2.Length != 0){
            
            if(spawnPointsL2.Length >= level2Q)
            {
                //Spawnea la cantidad requerida, sin repetir
                for(int i=0; i<level2Q; i++){

                    //Si no se ha superado la cantidad permitida de pixeles entonces crea uno 
                    if(currentPixelQ < aviableSpawnQ)
                    {
                        //Debug.Log("Creando pixel...");


                        int randomIndex = Random.Range(0, SP_ListL2.Count); // Elegir un índice aleatorio
                        GameObject randomObject = SP_ListL2[randomIndex]; // Obtener el objeto correspondiente

                        pixelSP = randomObject.GetComponent<PixelSpawner>();

                        GameObject pixelCreado = pixelSP.SpawnPixel();

                        SP_ListL2.RemoveAt(randomIndex);

                        Pixels_List.Add(pixelCreado);
                        PixelsToShield_List.Add(pixelCreado);

                        
                        pixelCreado.GetComponent<PixelController>().ActivarMovimiento();
                        

                        currentPixelQ++;

                        

                        
                    }
                    else{
                        Debug.LogWarning("Cantidad máxima de pixeles para este nivel");
                        

                    }

                    

                }
            }
            else{
                Debug.LogWarning("No hay la suficiente cantidad de spawns");
            }
            
        }
        
        



    }

    void DesaparecerPixel(){
        if(Pixels_List.Count>0){
            int randomIndex = Random.Range(0, Pixels_List.Count); // Elegir un índice aleatorio
            GameObject randomPixel = Pixels_List[randomIndex]; // Obtener el objeto correspondiente
            Pixels_List.Remove(randomPixel);
            PixelsToShield_List.Remove(randomPixel.transform.gameObject);
            GameObject pixelParent = randomPixel.transform.parent.gameObject;
            SP_List.Add(pixelParent);
            //randomPixel.GetComponent<PixelController>().DestruirPixel();
            //implementar funcion para desaparecer pixel
            Destroy(randomPixel);
            currentPixelQ--;

        }
        else{
            Debug.LogWarning("No hay pixeles para desaparecer");
        }
        
    }

    //Elimina todos los pixeles
    void DeleteAllPixels(){

        if(Pixels_List.Count>0){

            List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            foreach (GameObject enemy in enemies) // Copia la lista
            {
                Destroy(enemy);
            }

        }
        else{
            Debug.Log("La lista de pixeles está vacia");
        }

        Pixels_List.Clear();
        SP_List = new List<GameObject>(spawnPoints);
        SP_ListL2 = new List<GameObject>(spawnPointsL2);
        PixelsToShield_List.Clear();
        currentPixelQ = 0;

    }

    void AparecerPixel(){

        CrearPixel();

    }

    

    
    private IEnumerator ActivarG(float segundos, float tActivacionG){
        
     
        yield return new WaitForSeconds(segundos);
            
        
        glitchC.ActivarGlitch(tActivacionG);
        
    }
    

    
}
