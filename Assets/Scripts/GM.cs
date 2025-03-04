using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPoints;

    //Tiempos (segundos) de cada seccion
    [SerializeField] float tL1, tL2, tL3, tDescanso, tL4, tiempoADA; 

    PixelSpawner pixelSP;

    List<GameObject> SP_List;

    [SerializeField] int level1Q=4; // Cantidad maxima de pixeles a spawnear en nivel 1

    int currentPixelQ = 0;//Cantidad actual de pixeles

    int aviableSpawnQ = 0; //Cantidad disponible de spawns para cada nivel

    //Tiempo de aparicion y desaparicion de pixeles
    [SerializeField] int tAparicion, tDesaparicion;

    //Variables que sirven para tener control del tiempo
    private float tiempoTranscurrido = 0f;
    private int minutos, segundos, centesimas, segundosTotales;


    void Awake(){
        //Lista que contiene posibles puntos de spawneo de pixeles
        SP_List = new List<GameObject>(spawnPoints);
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

            Level1();



        }

        //Comprueba si hubo un "click"
        if(Input.GetMouseButtonDown(0)){
               
            Vector3 mousePos = Input.mousePosition;

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                if(hit.collider.tag == "Enemy"){
                    PixelController pController = hit.collider.gameObject.GetComponent<PixelController>();
                    GameObject pixelParent = hit.collider.gameObject.transform.parent.gameObject;
                    pController.DestruirPixel();
                    currentPixelQ--;
                    SP_List.Add(pixelParent);
                }
            }
        }
        
    }


    void Level1(){

        aviableSpawnQ = level1Q;

        if (spawnPoints.Length != 0){
            
            if(spawnPoints.Length > level1Q)
            {
                //Spawnea la cantidad requerida, sin repetir
                for(int i=0; i<level1Q; i++){

                    CrearPixel();

                }
            }
            else{
                Debug.LogWarning("No hay la suficiente cantidad de spawns");
            }

            InvokeRepeating("CrearPixel", 3f, tAparicion);
            
        }

    }

    void CrearPixel(){

        

        //Si no se ha superado la cantidad permitida de pixeles entonces crea uno 
        if(currentPixelQ < aviableSpawnQ)
        {
            Debug.Log("Creando pixel...");


            int randomIndex = Random.Range(0, SP_List.Count); // Elegir un índice aleatorio
            GameObject randomObject = SP_List[randomIndex]; // Obtener el objeto correspondiente

            pixelSP = randomObject.GetComponent<PixelSpawner>();

            pixelSP.SpawnPixel();

            SP_List.RemoveAt(randomIndex);

            currentPixelQ++;
        }
        else{
            Debug.LogWarning("Cantidad máxima de pixeles para este nivel");
        }



    }

    
}
