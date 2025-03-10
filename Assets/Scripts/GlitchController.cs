using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{

 

    bool glitchActivado = false;

    [SerializeField] int tempVida;

    [SerializeField] SpriteRenderer shieldMode, damageSprite;
    
    [SerializeField] float tCadenciaEscudos;

    float curentTActivacion;

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
        if(!glitchActivado && vida<=0){
            
            DesactivarGlitch();

            

            Invoke("ActivarGlitch", curentTActivacion);



        }
        else{
            //Anim
        }
    }

    public void QuitarVida(){

        if(glitchActivado){
            vida--;
            shieldMode.enabled = false;
            damageSprite.enabled=true;
            
            StartCoroutine(MostrarDamage(0.2f));
            //AnimDaño

        }
        

    }

    public void ActivarGlitch(){

        

        glitchActivado = true;


        Debug.Log("Glitch Activado");
        InvokeRepeating("DarEscudo", 0f, tCadenciaEscudos);

    

    }

    public void ActivarGlitch(float tActivacion){

        curentTActivacion = tActivacion;

        glitchActivado = true;


        Debug.Log("Glitch Activado");
        InvokeRepeating("DarEscudo", 0f, tCadenciaEscudos);

    

    }

    public void DarEscudo(){


        //InvokeRepeating("DarEscudo", 0f, curentTEscudos);

        SpriteRenderer idleMode = GetComponent<SpriteRenderer>();

        idleMode.enabled = false;

        shieldMode.enabled = true;

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

    private IEnumerator MostrarDamage(float segundos){
        
     
        yield return new WaitForSeconds(segundos);
        if(vida>0 && glitchActivado){
            shieldMode.enabled = true;
            damageSprite.enabled = false;

        }
        else{
            glitchActivado=false;
        }
            
        
    }

    public void DesactivarGlitch(){



        CancelInvoke("DarEscudo");

        Debug.Log("Glitch Desactivado");
        
        

        glitchActivado = false;

        SpriteRenderer idleMode = GetComponent<SpriteRenderer>();


        idleMode.enabled = true;
        damageSprite.enabled = false;
        shieldMode.enabled = false;

        vida = tempVida;


        
    }

    

}
