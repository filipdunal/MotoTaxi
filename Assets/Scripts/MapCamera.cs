using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    Transform player;
    Transform playerIcon;
    Vector3 cameraOffset=new Vector3(0f,5f,0f);

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Motorcycle").transform;
        playerIcon = player.transform.Find("PlayerIcon");
    }
    private void Update()
    {
        transform.position = player.position + cameraOffset;
        transform.rotation = Quaternion.Euler(90f, player.rotation.eulerAngles.y, 0f);
    }
}
