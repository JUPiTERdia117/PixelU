using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{

    bool modoEscudo = false;

    // Start is called before the first frame update
    void Start()
    {

        //Anim
        
    }

    // Update is called once per frame
    void Update()
    {
        if(modoEscudo){

        }
        else{
            //Anim
        }
    }

    public void ActivarModoEscudo(){

        modoEscudo = true;

    }

    public void DarEscudo(GameObject pixel){

        PixelController pixelC = pixel.GetComponent<PixelController>(); 
        pixelC.ActivarEscudos();

    }

}
