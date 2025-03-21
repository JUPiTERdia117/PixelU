using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Script para controlar a Ada
public class AdaController : MonoBehaviour
{
    [SerializeField] int vida = 1; // Vida inicial de Ada
   

    Light2D light2D; // Asigna tu luz en el inspector
    float minSize = 1f;  // Tamaño mínimo del Outer Radius
    [SerializeField] float maxSize=15.0f;  // Tamaño máximo del Outer Radius
    float targetSize; // Tamaño objetivo
    //public float currentSize; // Tamaño anterior
    [SerializeField] float growthSpeed, shrinkSpeed; // Velocidad de crecimiento/reducción
    bool creciendo = false; // Condición que determina si crece o se reduce

    Coroutine golpeCoroutine; // Corrutina para esperar el golpe

    [SerializeField] float tToHit=1.0f; // Tiempo para recibir un golpe

    void Awake(){
        // Asignar la luz
        light2D = GetComponent<Light2D>();
        minSize = light2D.pointLightOuterRadius;
        targetSize = light2D.pointLightOuterRadius;
    }

    // Update is called once per frame
    void Update()
    {
        //currentSize = light2D.pointLightOuterRadius;
        //Si está en estado de crecimiento y no ha alcanzado el tamaño máximo
        if(creciendo){
            if(light2D.pointLightOuterRadius < maxSize){
                light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, targetSize, Time.deltaTime * growthSpeed);
               
            }


        }
        
        //Si está en estado de reducción y no ha alcanzado el tamaño mínimo
        if(!creciendo){
            targetSize = light2D.pointLightOuterRadius;
            light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, minSize, Time.deltaTime * shrinkSpeed);

            if(light2D.pointLightOuterRadius%(maxSize/20) < 0.05f && vida>1){
                vida--;
                
            }
        }

        
        
    }

    // Método para dar vida a Ada
    public bool DarVida(){
        targetSize+=maxSize/20;
        creciendo = true;
        vida++;

        
        //Si ya hay una corrutina en espera, detenerla
        if (golpeCoroutine != null) {
            StopCoroutine(golpeCoroutine);
        }
        //Iniciar la corrutina para esperar el golpe
        golpeCoroutine = StartCoroutine(EsperarGolpe(tToHit));

        //Si la vida es igual a 20, devolver verdadero para ganar
        if(vida==20){
            return true;
        }
        else{
            return false;
        }

    }

    //Corrutina para esperar el golpe
    private IEnumerator EsperarGolpe(float segundos){
        
     
        yield return new WaitForSeconds(segundos);
        creciendo = false;

    }

}
