using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapWepons : MonoBehaviour
{
    GameObject obj;


    PlayerInventory playerInventory;
    private void Awake(){
        playerInventory = Component.FindObjectOfType<PlayerInventory>();
    }
     void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Assault")
        {
            playerInventory.heavyBullets += 60;
            obj = collision.gameObject;
            obj.GetComponent<Swap>().SwapGuns();
        }
        if (collision.gameObject.tag == "HandGun")
        {
            playerInventory.heavyBullets += 60;
            obj = collision.gameObject;
            obj.GetComponent<Swap>().SwapGuns();
        }
        if (collision.gameObject.tag == "Shotgun")
        {
            playerInventory.shotgunBullets += 20;
            obj = collision.gameObject;
            obj.GetComponent<Swap>().SwapGuns();
        }
    }
    


}
