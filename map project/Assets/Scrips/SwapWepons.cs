using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapWepons : MonoBehaviour
{
    GameObject obj;


    
     void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Assault")
        {
            obj = collision.gameObject;
            obj.GetComponent<Swap>().SwapGuns();
        }
        if (collision.gameObject.tag == "Handgun")
        {
            obj = collision.gameObject;
            obj.GetComponent<Swap>().SwapGuns();
        }
        if (collision.gameObject.tag == "Shotgun")
        {
            obj = collision.gameObject;
            obj.GetComponent<Swap>().SwapGuns();
        }
    }


}
