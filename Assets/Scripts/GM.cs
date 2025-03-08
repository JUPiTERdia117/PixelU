using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPoints;

    //Tiempos (segundos) de cada seccion
    [SerializeField] float tL1, tL2, tL3, tDescanso, tL4, tiempoADA; 

    PixelSpawner pixelSP;

    List<GameObject> SP_List, Pixels_List;

    static public List<GameObject> PixelsToShield_List; //Lista de pixeles disponibles para dar escudos.

    [SerializeField] int level1Q=4; // Cantidad maxima de pixeles a spawnear en nivel 1

    [SerializeField] GameObject glitch; //El glitch

    int currentPixelQ = 0;//Cantidad actual de pixeles

    int aviableSpawnQ = 0; //Cantidad disponible de spawns para cada nivel

    //Tiempo de aparicion y desaparicion de pixeles
    [SerializeField] int tDesaparicion, tAparicion;

    [SerializeField] int tEscudosL1, tEscudosL2, tEscudosL3;

    //Variables que sirven para tener control del tiempo
    private float tiempoTranscurrido = 0f;
    private int minutos, segundos, centesimas, segundosTotales;

    bool level1Started = false;

    GlitchController glitchC;


    void Awake(){
        //Lista que contiene posibles puntos de spawneo de pixeles
        SP_List = new List<GameObject>(spawnPoints);
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

        
        if(segundosTotales<tL1){

            //Llega glitch


            //Empieza nivel 1

            if(!level1Started){
                Level1();
            }
            



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

        
       
        level1Started = true;

        StartCoroutine(ActivarG(5.0f));

        

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

    void AparecerPixel(){

        CrearPixel();

    }

    

    
    private IEnumerator ActivarG(float segundos){
        
     
        yield return new WaitForSeconds(segundos);
            
        
        glitchC.ActivarGlitch(tEscudosL1);
        
    }
    

    
}
