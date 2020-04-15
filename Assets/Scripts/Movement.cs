using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    public float forwardForce=1f;
    public float turnForce = 1f;
    public Transform rotateAround;
    public Transform model;
    public Transform frontWheel;
    Vector3 localVelocity;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.forward * forwardForce);
        }
        transform.RotateAround(rotateAround.position, transform.up, turnForce * Time.timeScale * Input.GetAxis("Horizontal")*localVelocity.z);
        
    }

    public float leanForce = 45f;
    public float wheelTurningForce = 45f;
    private void Update()
    {
        Vector3 eulerRotation=new Vector3();
        eulerRotation.z = (localVelocity.x / 100f) * leanForce;
        model.localEulerAngles = eulerRotation;


        Vector3 wheelEulerRotation = new Vector3();
        wheelEulerRotation.z = 90f+(localVelocity.x / 100f) * wheelTurningForce;
        wheelEulerRotation.x = 90f;
        frontWheel.localEulerAngles = wheelEulerRotation;
    }
}
