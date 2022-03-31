using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColissionHandler : MonoBehaviour
{
    IEnumerator OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag =="Player"){
            //damge code
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(20);
            Debug.Log("Take Damage");
            Destroy(this.gameObject);
        }
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
        
    }
}
