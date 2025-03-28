using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_InputField inputField; 

    
    void Start()
    {
       inputField.onSubmit.AddListener(EnviarTexto);
    }

    public void Reiniciar(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    /*
    public void GetText()
    {
        string text = tmpInputField.text;
        Debug.Log("Texto ingresado: " + text);

        if (int.TryParse(tmpInputField.text, out int value))
        {
            
            PlayerPrefs.SetInt("MasterId", value); // Guarda el valor
            PlayerPrefs.Save(); // Asegura que se guarde en memoria

            SceneManager.LoadScene("Juego");
        }
        else
        {
            Debug.LogWarning("El texto no es un número válido.");
            
        }


    }
    */

    void EnviarTexto(string texto)
    {
       if (int.TryParse(texto, out int value))
        {
            
            PlayerPrefs.SetInt("MasterId", value); // Guarda el valor
            PlayerPrefs.Save(); // Asegura que se guarde en memoria

            SceneManager.LoadScene("Juego");
        }
        else
        {
            Debug.LogWarning("El texto no es un número válido.");
            
        }
    }
}
