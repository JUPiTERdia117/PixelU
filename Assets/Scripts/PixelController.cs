using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelController : MonoBehaviour
{

    [SerializeField] int Id;// Identificador de Pixel
    
    [SerializeField] int vida;// Vida inicial de Pixel

    [SerializeField] SpriteRenderer shieldSprite;//Sprite del escudo

    //bool shieldActive = false;
    bool movement = false, movementAl = false;//Condiciones para activar movimiento 

    float startX, direction;//Posición inicial y dirección del movimiento
    


    // Start is called before the first frame update

    

    void Awake(){
        // Asignar la posición inicial
        startX = transform.position.x;
        // Aleatoriamente asignar dirección (1 para derecha, -1 para izquierda)
        direction = Random.value > 0.5f ? 1 : -1;
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Si se activa el movimiento
        if(movement){

            // Movimiento de PingPong
            float speed = 2.5f;
            float width = 12f;
            transform.position = new Vector2(Mathf.PingPong(Time.time * speed, 2*width)+(startX-width), transform.position.y);
        }
        // Si se activa el movimiento aleatorio
        if(movementAl){
            float speed = 2.5f;
            float width = 12f;

            // Movimiento manual con cambio de dirección en los bordes
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

            // Cambiar dirección si alcanza los límites
            if (transform.position.x >= startX + width || transform.position.x <= startX - width)
            {
                direction *= -1;
            }

        }
        
    }

    // Método  para quitar  vida a Pixel
    public int QuitarVida(){

        vida--;


        if(vida==1){
            DesactivarEscudos();
        }

        return vida;

    }

    // Método para destruir Pixel
    public int DestruirPixel(){

    

        //Animacion

        Destroy(this.gameObject);

        return Id;



    }
    
    // Método para activar escudos
    public void ActivarEscudos(){

        

        shieldSprite.enabled = true;

        //shieldActive = true;

        vida++;



    }
    
    // Método para desactivar escudos
    public void DesactivarEscudos(){

        shieldSprite.enabled = false;

        //shieldActive = false;

    }

    // Método para activar movimiento
    public void ActivarMovimiento(){

        movement = true;

    }
    
    // Método para activar movimiento aleatorio
    public void ActivarMovimientoAleatorio(){

        movementAl = true;

    }

    

}
