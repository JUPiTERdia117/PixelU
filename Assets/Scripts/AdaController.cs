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
    float targetSize; // Tamaño objetivo
    public float currentSize; // Tamaño anterior
    [SerializeField] float growthSpeed, shrinkSpeed; // Velocidad de crecimiento/reducción
    bool creciendo = false; // Condición que determina si crece o se reduce

    Coroutine golpeCoroutine;

    [SerializeField] float tToHit=1.0f;

    void Awake(){
        light2D = GetComponent<Light2D>();
        minSize = light2D.pointLightOuterRadius;
        targetSize = light2D.pointLightOuterRadius;
    }

    // Update is called once per frame
    void Update()
    {
        currentSize = light2D.pointLightOuterRadius;
        if(creciendo){
            if(light2D.pointLightOuterRadius < maxSize){
                light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, targetSize, Time.deltaTime * growthSpeed);
               
            }


        }
        
        if(!creciendo && light2D.pointLightOuterRadius > minSize){
            light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, minSize, Time.deltaTime * shrinkSpeed);

            if(light2D.pointLightOuterRadius%(maxSize/20) < 0.5f && vida>1){
                vida--;
                
            }
        }

        
        
    }

    public bool DarVida(){
        targetSize+=maxSize/20;
        creciendo = true;
        vida++;

        
        
        if (golpeCoroutine != null) {
            StopCoroutine(golpeCoroutine);
        }
        
        golpeCoroutine = StartCoroutine(EsperarGolpe(tToHit));

        if(vida==20){
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
