using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject Camera;
    GameObject gunToDestroy;
    public void SwapGuns()
    {
        Destroy(Camera.transform.GetChild(0).gameObject);
        GameObject uiObj = Instantiate<GameObject>(gun,new Vector3 (0, 0, 10), Quaternion.identity);
        uiObj.transform.SetParent(Camera.transform);
        uiObj.transform.localPosition = gun.transform.localPosition;
        uiObj.transform.localRotation = gun.transform.localRotation;
        
    }
}
