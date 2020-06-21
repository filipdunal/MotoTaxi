using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FitInCamera : MonoBehaviour
{
    public Camera cameraToFit;
    public bool openedPhone;
    public GraphicRaycaster graphicRaycaster;
    public float phoneAnimationSpeed=1f;
    public GameObject hidePhoneButton;
    public List<Transform> steeringButtons;

    float ratio16v9 = 16f / 9f;
    
    public void OpenPhone()
    {
        Vector3 position = new Vector3(0f, 0f, transform.localPosition.z);
        Quaternion rotation = Quaternion.identity;
        Vector3 scale;

        graphicRaycaster.enabled = true;

        //transform.localPosition = new Vector3(0f,0f,transform.localPosition.z);
        //transform.rotation = Quaternion.identity;

        float height = 2.0f * Mathf.Tan(0.5f * cameraToFit.fieldOfView * Mathf.Deg2Rad) * transform.localPosition.z;
        float width = height * cameraToFit.aspect;
        if (cameraToFit.aspect > ratio16v9)
        {
            height *= 1.4f;
            scale = new Vector3(height, height, transform.localScale.z);
        }
        else
        {
            width *= 0.8f;
            scale = new Vector3(width, width, transform.localScale.z);
        }
        StopAllCoroutines();
        StartCoroutine(PhoneAnimation(position, rotation, scale));
        hidePhoneButton.SetActive(true);

        foreach(Transform button in steeringButtons)
        {
            button.gameObject.SetActive(false);
        }
        
    }

    public void ClosePhone()
    {
        hidePhoneButton.SetActive(false);
        graphicRaycaster.enabled = false;

        Vector3 position = new Vector3(0f, -0.7f, transform.localPosition.z);
        Quaternion rotation = Quaternion.Euler(70f, 0f, 0f);
        Vector3 scale = new Vector3(1f, 1f, transform.localScale.z);
        StopAllCoroutines();
        StartCoroutine(PhoneAnimation(position, rotation, scale));

        foreach (Transform button in steeringButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    IEnumerator PhoneAnimation(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        
        while (Vector3.Distance(transform.localScale, scale)>0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, phoneAnimationSpeed * Time.unscaledDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, phoneAnimationSpeed * Time.unscaledDeltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, scale, phoneAnimationSpeed * Time.unscaledDeltaTime);
            yield return 0;
        }
        
    }

    private void OnMouseUp()
    {
        if(!IsPointerOverUIObject())
        {
            openedPhone = !openedPhone;
            if (openedPhone)
            {
                OpenPhone();
            }
        }
        
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
