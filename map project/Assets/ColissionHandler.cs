using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColissionHandler : MonoBehaviour
{
    int numberOfColides;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag =="Player"){
            //damge code
            Debug.Log("Take Damage");
        }
        // if(numberOfColides==2)
        //     Destroy(gameObject);
    }
}
