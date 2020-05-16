﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueStabilizer : MonoBehaviour
{
    public float stability = 0.3f;
    public float speed = 2.0f;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 predictedUp = Quaternion.AngleAxis(
            rb.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            rb.angularVelocity
        ) * transform.up;

        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        // Uncomment the next line to stabilize on only 1 axis.
        torqueVector = Vector3.Project(torqueVector, transform.forward);
        rb.AddTorque(torqueVector * speed);
    }
}
