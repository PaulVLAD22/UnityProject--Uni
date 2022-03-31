using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunSpown : MonoBehaviour
{
    public float time = 10f;
    float spawnTime;

    public GameObject objectToSpawn;
    private void Awake(){
        spawnTime = time;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.childCount == 0){
            spawnTime -= Time.deltaTime;
            if(spawnTime <= 0.0f){
                GameObject uiObj = Instantiate<GameObject>(objectToSpawn,new Vector3 (0, 0, 10), Quaternion.identity);
                uiObj.transform.SetParent(transform);
                uiObj.transform.localPosition = new Vector3(0,0,0);
                spawnTime = time;
            }
        }

    }
}
