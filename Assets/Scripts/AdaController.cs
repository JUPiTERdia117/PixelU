using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AdaController : MonoBehaviour
{
    [SerializeField] int vida = 1;

    Light2D light2D; // Asigna tu luz en el inspector
    float minSize = 1f;  // Tamaño mínimo del Outer Radius
    [SerializeField] float maxSize=15.0f;  // Tamaño máximo del Outer Radius
    [SerializeField] float growthSpeed=1.0f; // Velocidad de crecimiento/reducción
    bool creciendo = false; // Condición que determina si crece o se reduce

    [SerializeField] float tToHit=1.0f;

    void Awake(){
        light2D = GetComponent<Light2D>();
        minSize = light2D.pointLightOuterRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if(creciendo){
            light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, maxSize, Time.deltaTime * growthSpeed);

        }else{
            light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, minSize, Time.deltaTime * growthSpeed);
        }
        
    }

    public bool DarVida(){
        creciendo = true;
        vida++;
        //transform.localScale = new Vector2(transform.localScale.x+1,transform.localScale.y+1);
        
        StartCoroutine(EsperarGolpe(tToHit));

        if(vida==4 && light2D.pointLightOuterRadius>10.0f){
            return true;
        }
        else{
            return false;
        }

    }

    private IEnumerator EsperarGolpe(float segundos){
        
     
        yield return new WaitForSeconds(segundos);

        creciendo = false;
    }

}
