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

    float startX;

    float timeCounter = 0f; // Variable para llevar el tiempo del PingPong

    bool isFirstActivation = true;

    float speed = 2.5f, width = 12.0f;

    bool animEn, animSal = false;

    
    
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        vida = tempVida;
        startX = transform.position.x;

        //timeCounter = ((currentX - (startX - width)) / (2 * width)) / speed;

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

        if(animEn){
            if (elapsedTime < 1.0f)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / 1.0f;
                transform.localScale = Vector3.Lerp(Vector3.zero,new Vector3(0.48f,0.48f,0.48f) , t);
            }else{
                elapsedTime = 0.0f;
                animEn = false;
            }
        }

        if(animSal){
            if (elapsedTime < 1.0f)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / 1.0f;
                transform.localScale = Vector3.Lerp(new Vector3(0.48f,0.48f,0.48f), Vector3.zero, t);
            }else{
                elapsedTime = 0.0f;
                animSal = false;
            }

        }

        if(glitchActivado){
            
            timeCounter += Time.deltaTime;
            transform.position = new Vector2(Mathf.PingPong(timeCounter * speed, 2*width)+(startX-width), transform.position.y);
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

        // Para la primera vez hace que el glitch empiece a moverse
        //Desde su punto inicial
        if (isFirstActivation)
        {
            timeCounter = width / speed; // Midpoint of PingPong cycle
            isFirstActivation = false;
        }


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

        CancelInvoke("ActivarGlitch");

        Debug.Log("Glitch Desactivado");
        
        

        glitchActivado = false;

        SpriteRenderer idleMode = GetComponent<SpriteRenderer>();


        idleMode.enabled = true;
        damageSprite.enabled = false;
        shieldMode.enabled = false;

        vida = tempVida;


        
    }

    public void EntradaGlitch(){
        animEn = true;
        transform.localScale = Vector3.zero;
    }

    public void SalidaGlitch(){
        animSal = true;
        transform.localScale = new Vector3(0.48f,0.48f,0.48f);

        Invoke("DesactivarGlitch", 1.0f);

        
    }



    

}
