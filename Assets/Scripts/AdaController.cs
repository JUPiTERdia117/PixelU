using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaController : MonoBehaviour
{
    [SerializeField] int vida = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool DarVida(){

        vida++;
        transform.localScale = new Vector2(transform.localScale.x+1,transform.localScale.y+1);

        if(vida==4){
            return true;
        }
        else{
            return false;
        }

    }

}
