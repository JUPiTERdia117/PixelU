using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelController : MonoBehaviour
{
    
    [SerializeField] int vida;

    [SerializeField] SpriteRenderer shieldSprite;

    //bool shieldActive = false;
    bool movement = false, movementAl = false;

    float startX, direction;
    


    // Start is called before the first frame update

    

    void Awake(){
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
        if(movement){


            float speed = 2.5f;
            float width = 12f;
            transform.position = new Vector2(Mathf.PingPong(Time.time * speed, 2*width)+(startX-width), transform.position.y);
        }
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

    public int QuitarVida(){

        vida--;

        if(vida==0){
            DestruirPixel();
        }

        if(vida==1){
            DesactivarEscudos();
        }

        return vida;

    }

    public void DestruirPixel(){

    

        //Animacion

        Destroy(this.gameObject);



    }

    public void ActivarEscudos(){

        

        shieldSprite.enabled = true;

        //shieldActive = true;

        vida++;



    }
    

    public void DesactivarEscudos(){

        shieldSprite.enabled = false;

        //shieldActive = false;

    }

    public void ActivarMovimiento(){

        movement = true;

    }

    public void ActivarMovimientoAleatorio(){

        movementAl = true;

    }

    

}
