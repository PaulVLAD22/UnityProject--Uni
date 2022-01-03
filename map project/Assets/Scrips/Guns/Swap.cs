using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour
{
    [SerializeField] GameObject gun;
    Camera camera;
    GameObject gunToDestroy;
    
    private void Awake(){
        camera = Camera.main;
    }
    public void SwapGuns()
    {   
        if(!gun.gameObject.CompareTag(camera.transform.GetChild(1).gameObject.tag)) {
            Destroy(camera.transform.GetChild(1).gameObject);
            GameObject uiObj = Instantiate<GameObject>(gun,new Vector3 (0, 0, 10), Quaternion.identity);
            uiObj.transform.SetParent(camera.transform);
            uiObj.transform.localPosition = gun.transform.localPosition;
            uiObj.transform.localRotation = gun.transform.localRotation;
        }
        
    }
}
