using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MapCamera : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Camera>().ResetAspect();
    }
}
