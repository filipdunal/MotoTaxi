using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapCamera : MonoBehaviour
{
    Transform player;
    Vector3 cameraOffset = new Vector3(0f, 5f, 70f);
    Touch touch;

    public Transform playerIconPrefab;
    public PhoneCamera phoneCamera;
    public float speedOfDraging=1f;

    Camera uiCamera;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Motorcycle").transform;
        Transform playerIcon = player.transform.Find("PlayerIcon");
        if (playerIcon == null)
        {
            Instantiate(playerIconPrefab, player);
        }
        uiCamera = GetComponent<Camera>();
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
            UpdateOnUI();
        }
        
    }
    void UpdateOnUI()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved) //Moving map
            {
                transform.position = new Vector3(
                    transform.position.x - touch.deltaPosition.y * speedOfDraging,
                    transform.position.y,
                    transform.position.z + touch.deltaPosition.x * speedOfDraging
                    );
            }

            if(touch.phase==TouchPhase.Stationary)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if(Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.textureCoord);
                }
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = uiCamera.
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
