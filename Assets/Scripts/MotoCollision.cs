using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoCollision : MonoBehaviour
{
    public float maxSpeedImpact = 20f;

    private void OnCollisionStay(Collision collision)
    {
        float impactSpeed = VelocityToKMH(collision.relativeVelocity.magnitude);
        //Debug.Log("Bum o sile: " + impactSpeed);
        if (impactSpeed > maxSpeedImpact)
        {
            Crash();
        }
    }

    float VelocityToKMH(float velocity)
    {
        float speed = (float)(velocity * 3.6);
        return speed;
    }

    void Crash()
    {
        //Debug.Log("OJ JAK BOLI ZA MOCNO!");
    }
}
