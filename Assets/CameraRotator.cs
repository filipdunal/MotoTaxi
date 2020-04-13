using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float speed = 1f;
    private void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
