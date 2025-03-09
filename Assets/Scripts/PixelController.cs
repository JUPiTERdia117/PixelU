using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelController : MonoBehaviour
{
    
    [SerializeField] int vida;

    [SerializeField] SpriteRenderer shieldSprite;

    bool shieldActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        shieldActive = true;

        vida++;



    }
    

    public void DesactivarEscudos(){

        shieldSprite.enabled = false;

        shieldActive = false;

    }

    

}
