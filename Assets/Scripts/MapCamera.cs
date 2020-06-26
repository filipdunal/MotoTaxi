using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    Transform player;
    Transform playerIcon;
    Vector3 cameraOffset=new Vector3(0f,5f,70f);
    public PhoneCamera phoneCamera;
    public float speedOfDraging=1f;
    Touch touch;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Motorcycle").transform;
        playerIcon = player.transform.Find("PlayerIcon");
    }
    private void Update()
    {
        if(!phoneCamera.openedPhone)
        {
            transform.position = player.position + Quaternion.Euler(0f, player.rotation.eulerAngles.y, 0f) * cameraOffset;
            transform.rotation = Quaternion.Euler(90f, player.rotation.eulerAngles.y, 0f);
        }
        else
        {
            if(Input.touchCount>0)
            {
                touch = Input.GetTouch(0);
                if(touch.phase==TouchPhase.Moved)
                {
                    Debug.Log("Przesuwanie");
                    transform.position = new Vector3(
                        transform.position.x -touch.deltaPosition.y * speedOfDraging,
                        transform.position.y,
                        transform.position.z + touch.deltaPosition.x * speedOfDraging
                        );
                }
            }
        }
        
    }
}
