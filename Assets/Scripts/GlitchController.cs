using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{

    bool modoEscudo = false;

    [SerializeField] int tempVida;

    [SerializeField] SpriteRenderer shieldMode;

    int curentTEscudos;

    int vida = 0;

    // Start is called before the first frame update
    void Awake()
    {
        vida = tempVida;
        //Anim
        
    }

    // Update is called once per frame
    void Update()
    {
        if(vida<=0){
            CancelInvoke("DarEscudo");

            //Debug.Log("Glitch Desactivado");
        
            modoEscudo = false;

            SpriteRenderer idleMode = GetComponent<SpriteRenderer>();

            idleMode.enabled = true;
            shieldMode.enabled = false;

            vida = tempVida;

            InvokeRepeating("DarEscudo", 10.0f, curentTEscudos);



        }
        else{
            //Anim
        }
    }

    public void QuitarVida(){
        vida--;
        //AnimDaño
    }

    public void ActivarGlitch(int tEscudos){

        curentTEscudos = tEscudos;

        Debug.Log("Glitch Activado");
        InvokeRepeating("DarEscudo", 0f, curentTEscudos);

    

    }

    public void DarEscudo(){



        Debug.Log("Glitch dando escudos");

        //InvokeRepeating("DarEscudo", 0f, curentTEscudos);

        SpriteRenderer idleMode = GetComponent<SpriteRenderer>();

        idleMode.enabled = false;

        shieldMode.enabled = true;

        modoEscudo = true;

         
        
        if(GM.PixelsToShield_List.Count>0){
            //Debug.Log("Dando escudo");
            int randomIndex = Random.Range(0, GM.PixelsToShield_List.Count); // Elegir un índice aleatorio
            GameObject randomPixel = GM.PixelsToShield_List[randomIndex]; // Obtener el objeto correspondiente

            PixelController pixelC = randomPixel.GetComponent<PixelController>(); 
            pixelC.ActivarEscudos();

            GM.PixelsToShield_List.Remove(randomPixel);

                

        }
        else{

            Debug.Log("Sin pixel disppnible para dar escudo");

        }



    }

    

}
