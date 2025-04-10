using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para manejo de spawners de píxeles
public class PixelSpawner : MonoBehaviour
{

    // Arreglo de prefabs de píxeles
    [SerializeField] GameObject[] prefabsPixel;

    

    List<GameObject> allowedPixelPrefabs = new List<GameObject>(); // Cambiado a List<GameObject>

    public void SetColors(List<string> colores)
    {
        
        foreach (string color in colores)
        {
            // Determinar el índice basado en el color
            int index = GetIndexFromColor(color);

            // Validar que el índice sea válido
            if (index >= 0 && index < prefabsPixel.Length)
            {
                allowedPixelPrefabs.Add(prefabsPixel[index]);
            }
            else
            {
                Debug.LogWarning($"Color '{color}' no tiene un prefab asignado o está fuera de rango.");
            }


        }

        allowedPixelPrefabs.Add(prefabsPixel[10]); // Agregar el prefab de "comodin" al final de la lista


    }

    // Método para obtener el índice del prefab basado en el color
    private int GetIndexFromColor(string color)
    {
        // Mapear colores a índices específicos
        switch (color.ToLower())
        {
            case "blanco": return 0;
            case "rojo": return 1;
            case "verde": return 2;
            case "azul": return 3;
            case "amarillo": return 4;
            case "rosa": return 5;
            case "morado": return 6;
            case "naranja": return 7;
            case "cyan": return 8;
            case "cafe": return 9;


            
            


            // Agregar más colores según sea necesario
            default: return -1; // Retornar -1 si el color no está mapeado
        }
    }
    
    // Método para instanciar un píxel
    public GameObject SpawnPixel(){
        // Si no hay prefabs, retornar nulo
        if (prefabsPixel.Length == 0){
            Debug.LogError("No hay prefabs de píxeles asignados.");
            return null;
        } 

        int randomIndex = Random.Range(0, allowedPixelPrefabs.Count); // Elegir un índice aleatorio
        GameObject randomObject = allowedPixelPrefabs[randomIndex]; // Obtener el objeto correspondiente

        GameObject pixel = Instantiate(randomObject, transform.position, Quaternion.identity, transform); // Instanciar el pixel
        //Retornar el pixel instanciado
        return pixel;
    }
}
