using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelController : MonoBehaviour
{
    
    [SerializeField] int vida;

    [SerializeField] SpriteRenderer shieldSprite;

    //bool shieldActive = false;
    bool movement = false;

    float startX;

    // Start is called before the first frame update

    

    void Awake(){
        startX = transform.position.x;
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

    

}
