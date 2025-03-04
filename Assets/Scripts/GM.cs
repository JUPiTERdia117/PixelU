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

    int spawnQuantity=4; // Cantidad de pixeles a spawnear

    int currentPixels = 0;//Cantidad actual de pixeles

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
        //spawnQuantity = 4;
        if (spawnPoints.Length != 0){
            
            if(spawnPoints.Length > spawnQuantity)
            {
                //Spawnea la cantidad requerida, sin repetir
                for(int i=0; i<spawnQuantity; i++){

                    CrearPixel();

                }
            }
            else{
                Debug.LogWarning("No hay la suficiente cantidad de spawns");
            }

            InvokeRepeating("CrearPixel", 3f, tAparicion);
            
        }



        

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

            /*
            //Cada 3 segundos
            if(segundosTotales%3.0f == 0.0f ){
                Debug.Log("Apareciendo pixel");

                CrearPixel();


            }
            */

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
                    pController.DestruirPixel();
                    currentPixels--;
                    //SP_List.Add();
                }
            }
        }
        
    }

    void CrearPixel(){

        


        if(currentPixels<spawnPoints.Length)
        {
            Debug.Log("Creando pixel...");


            int randomIndex = Random.Range(0, SP_List.Count); // Elegir un índice aleatorio
            GameObject randomObject = SP_List[randomIndex]; // Obtener el objeto correspondiente

            pixelSP = randomObject.GetComponent<PixelSpawner>();

            pixelSP.SpawnPixel();

            SP_List.RemoveAt(randomIndex);

            currentPixels++;
        }
        else{
            Debug.LogWarning("Cantidad máxima de pixeles");
        }



    }

    
}
