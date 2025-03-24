using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para controlar el glitch
public class GlitchController : MonoBehaviour
{

 

    bool glitchActivado = false;//Condición para activar glitch

    [SerializeField] int tempVida;//Vida inicial de Glitch

    [SerializeField] SpriteRenderer shieldMode, damageSprite;//Sprites de escudo y daño
    
    [SerializeField] float tCadenciaEscudos;//Tiempo de cadencia para dar escudos

    float tCadenciaMovimiento;//Tiempo actual de cadencia para movimiento

    float curentTActivacion;//Tiempo de activación actual

    int vida = 0;//Vida actual de Glitch

    float startX, xNewPos;//Posición inicial de Glitch y nueva posición

    float timeCounter = 0f; // Variable para llevar el tiempo del PingPong

    bool isFirstActivation = true;//Condición para la primera activación

    float speed = 2.5f, width = 30.0f;//Velocidad y ancho máximo de movimiento

    [SerializeField] float movementRange;//Rango de movimiento

    bool animEn, animSal = false;//Condiciones para animaciones de entrada y salida

    bool isValid = false;//Condición para movimiento
    
    
    private float elapsedTime, elapsedTimeAnim = 0f;//Variables para tiempo transcurrido

    // Start is called before the first frame update
    void Awake()
    {
        // Asignar la vida inicial
        vida = tempVida;
        startX = transform.position.x;//Asignar la posición inicial

        

        //Anim
        
    }

    // Update is called once per frame
    void Update()
    {

        // Si está activado el glitch
        if(glitchActivado){
           
            
            //Si se puede mover
            if(isValid){
                

                Movement();

            }else{
                ChooseNewPos();
            }
            
            
        }

        // Si no está activado el glitch y la vida es 0
        if(!glitchActivado && vida<=0){
            
            DesactivarGlitch();

            
            //Invocar la activación del glitch
            Invoke("ActivarGlitch", curentTActivacion);



        }
        else{
            //Anim
        }

        //Si se activa la animación de entrada
        if(animEn){
            //Si el tiempo transcurrido es menor a 1 segundo
            if (elapsedTimeAnim< 1.0f)
            {   
                elapsedTimeAnim += Time.deltaTime;
                float t = elapsedTimeAnim / 1.0f;
                //Interpolación de escala
                transform.localScale = Vector3.Lerp(Vector3.zero,new Vector3(0.48f,0.48f,0.48f) , t);
            }else{
                //Reiniciar el tiempo y desactivar la animación
                elapsedTimeAnim = 0.0f;
                animEn = false;
            }
        }

        //Si se activa la animación de salida
        if(animSal){
            
           
            if (elapsedTimeAnim < 1.0f)
            {
                
                elapsedTimeAnim += Time.deltaTime;
                float t = elapsedTimeAnim / 1.0f;
                //Interpolación de escala
                transform.localScale = Vector3.Lerp(new Vector3(0.48f,0.48f,0.48f), Vector3.zero, t);
            }else{
                //Reiniciar el tiempo y desactivar la animación
                elapsedTimeAnim = 0.0f;
                animSal = false;
            }

        }

        
    }
    // Método para quitar vida a Glitch
    public int QuitarVida(){
        //Si el glitch está activado
        if(glitchActivado){
            vida--;
            shieldMode.enabled = false;
            damageSprite.enabled=true;
            
            StartCoroutine(MostrarDamage(0.2f));
            //AnimDaño

        }
        
        return vida;

    }

    // Método para activar el glitch
    public void ActivarGlitch(){

        

        glitchActivado = true;


        Debug.Log("Glitch Activado");
       
    
        //Invocar la cadencia de escudos
        InvokeRepeating("DarEscudo", 0f, tCadenciaEscudos);

    

    }

    //Método para activar el glitch con tiempo de activación
    public void ActivarGlitch(float tActivacion, float tMovGlitch){
        //Asignar el tiempo de activación
        curentTActivacion = tActivacion;

        tCadenciaMovimiento = tMovGlitch;

        glitchActivado = true;

        // Para la primera vez hace que el glitch empiece a moverse
        //Desde su punto inicial
        if (isFirstActivation)
        {
            timeCounter = width / speed; // Midpoint of PingPong cycle
            isFirstActivation = false;
        }


        Debug.Log("Glitch Activado");
        //Invocar la cadencia de escudos
        InvokeRepeating("DarEscudo", 0f, tCadenciaEscudos);

    

    }

    //Método para dar escudos a píxeles
    public void DarEscudo(){


        //Activar sprite de modo de escudo
        
        SpriteRenderer idleMode = GetComponent<SpriteRenderer>();

        idleMode.enabled = false;

        shieldMode.enabled = true;

        //Si hay píxeles disponibles para dar escudo
        if(GM.PixelsToShield_List.Count>0){
            
            
            int randomIndex = Random.Range(0, GM.PixelsToShield_List.Count); // Elegir un índice aleatorio
            GameObject randomPixel = GM.PixelsToShield_List[randomIndex]; // Obtener el objeto correspondiente

            //Activar escudo en el píxel
            PixelController pixelC = randomPixel.GetComponent<PixelController>(); 
            pixelC.ActivarEscudos();


            //Remover el píxel de la lista de píxeles disponibles para dar escudo
            GM.PixelsToShield_List.Remove(randomPixel);

        }
        else{

            Debug.Log("Sin pixel disppnible para dar escudo");

        }



    }

    //Corrutina para mostrar el daño    
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

    //Método para desactivar el glitch
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

    //Método para activar la animación de entrada
    public void EntradaGlitch(){
        animEn = true;
        transform.localScale = Vector3.zero;
    }

    //Método para activar la animación de salida
    public void SalidaGlitch(){
        animSal = true;
        transform.localScale = new Vector3(0.48f,0.48f,0.48f);

        Invoke("DesactivarGlitch", 1.0f);

        
    }
    
    //Método para mover el glitch
    void Movement()
    {
        
        
        if (elapsedTime < tCadenciaMovimiento)
        {
            
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / tCadenciaMovimiento;
            //Interpolación de posición
            transform.position = Vector3.Lerp(transform.position, new Vector3(xNewPos, transform.position.y, transform.position.z), t);
        }else{
            
            isValid = false;
            //Reiniciar el tiempo y desactivar movimiento
            elapsedTime = 0.0f;
        }
        

        
            
    }

    void ChooseNewPos()
    {

        
        //Elegir una nueva posición aleatoria
        while(!isValid){
            
            //Elegir una nueva posición
            xNewPos = Random.Range(-width, width);
            //Si la nueva posición está dentro del rango de movimiento
            if((xNewPos>(transform.position.x+movementRange) && xNewPos < width-movementRange ) || (xNewPos<(transform.position.x-movementRange) && xNewPos > -width+movementRange)){
                isValid = true;
            }
            else{
                
                isValid = false;
            }
        }
    }


    

}
