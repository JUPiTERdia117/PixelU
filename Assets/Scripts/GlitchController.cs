using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{

    bool modoEscudo = false;

    [SerializeField] int vida;

    [SerializeField] SpriteRenderer shieldMode;



    // Start is called before the first frame update
    void Start()
    {

        //Anim
        
    }

    // Update is called once per frame
    void Update()
    {
        if(vida<=0){
            CancelInvoke("DarEscudo");
        
            modoEscudo = false;

            SpriteRenderer idleMode = GetComponent<SpriteRenderer>();

            idleMode.enabled = true;
            shieldMode.enabled = false;

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

        Debug.Log("Glitch Activado");

        InvokeRepeating("DarEscudo", 0f, tEscudos);

        SpriteRenderer idleMode = GetComponent<SpriteRenderer>();

        idleMode.enabled = false;

        shieldMode.enabled = true;

        modoEscudo = true;

    }

    public void DarEscudo(){

         
        Debug.Log("Antes de dar s:"+GM.PixelsToShield_List.Count);   
        if(GM.PixelsToShield_List.Count>0){
            //Debug.Log("Dando escudo");
            int randomIndex = Random.Range(0, GM.PixelsToShield_List.Count); // Elegir un índice aleatorio
            GameObject randomPixel = GM.PixelsToShield_List[randomIndex]; // Obtener el objeto correspondiente

            PixelController pixelC = randomPixel.GetComponent<PixelController>(); 
            pixelC.ActivarEscudos();

            GM.PixelsToShield_List.Remove(randomPixel);

            Debug.Log("Despues de dar s:"+GM.PixelsToShield_List.Count);      

        }
        else{

            Debug.Log("Sin pixel disppnible para dar escudo");

        }



    }

    

}
