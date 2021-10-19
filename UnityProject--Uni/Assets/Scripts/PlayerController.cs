using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float OX, OY, OZ;
    private Vector3 direction;
    public int speed = 10;
    private Rigidbody rb;
    private static float breakStrength = 0.9f;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        OY = 0;
        rb = GetComponent<Rigidbody>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        OX = Input.GetAxis("Horizontal");
        OZ = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            slowDown();
        }
    }

    private void FixedUpdate()
    {

        direction = new Vector3(OX, OY, OZ);
        rb.AddForce(direction * speed);
        //Debug.Log(OX+" "+OZ );
    }

    public void slowDown()
    {
        this.rb.velocity *= breakStrength;
    }

    
}