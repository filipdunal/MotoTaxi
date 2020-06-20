using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInCamera : MonoBehaviour
{
    public Camera cameraToFit;
    public bool openedPhone;

    float ratio16v9 = 16f / 9f;
    private void Update()
    {
        
    }

    public void OpenPhone()
    {
        transform.localPosition = new Vector3(0f,0f,transform.localPosition.z);
        transform.rotation = Quaternion.identity;

        float height = 2.0f * Mathf.Tan(0.5f * cameraToFit.fieldOfView * Mathf.Deg2Rad) * transform.localPosition.z;
        float width = height * cameraToFit.aspect;
        if (cameraToFit.aspect > ratio16v9)
        {
            height *= 1.4f;
            transform.localScale = new Vector3(height, height, transform.localScale.z);
        }
        else
        {
            width *= 0.8f;
            transform.localScale = new Vector3(width, width, transform.localScale.z);
        }
    }

    public void ClosePhone()
    {
        transform.localPosition = new Vector3(0f, -0.7f, transform.localPosition.z);
        transform.localEulerAngles = new Vector3(70f, 0f, 0f);
        transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
    }

    private void OnMouseDown()
    {
        openedPhone = !openedPhone;
        if(openedPhone)
        {
            OpenPhone();
        }
        else
        {
            ClosePhone();
        }
    }
}
