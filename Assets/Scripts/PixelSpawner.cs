using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para manejo de spawners de píxeles
public class PixelSpawner : MonoBehaviour
{

    // Arreglo de prefabs de píxeles
    [SerializeField] GameObject[] prefabsPixel;


    
    // Método para instanciar un píxel
    public GameObject SpawnPixel(){
        // Si no hay prefabs, retornar nulo
        if (prefabsPixel.Length == 0) return null;

        int randomIndex = Random.Range(0, prefabsPixel.Length); // Elegir un índice aleatorio
        GameObject randomObject = prefabsPixel[randomIndex]; // Obtener el objeto correspondiente

        GameObject pixel = Instantiate(randomObject, transform.position, Quaternion.identity, transform); // Instanciar el pixel
        //Retornar el pixel instanciado
        return pixel;
    }
}
