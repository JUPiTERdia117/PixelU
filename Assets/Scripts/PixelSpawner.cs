using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] prefabsPixel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPixel(){
        if (prefabsPixel.Length == 0) return;

        int randomIndex = Random.Range(0, prefabsPixel.Length); // Elegir un índice aleatorio
        GameObject randomObject = prefabsPixel[randomIndex]; // Obtener el objeto correspondiente

        Instantiate(randomObject, transform.position, Quaternion.identity); // Instanciar el objeto
    }
}
