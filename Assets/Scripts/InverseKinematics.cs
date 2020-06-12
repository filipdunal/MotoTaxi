using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    public Transform headPosition;
    public float speedToLowerHead;
    public float headLowPosition;
    float headHighPosition;
    public float speedOfChangingHeadHeight;
    CarMovementScript cms;
    bool lowerHead;
    float desireHeadPosition;
    float epsilon=0.001f;

    private void Start()
    {
        cms = GetComponent<CarMovementScript>();
        headHighPosition = headPosition.localPosition.x;
    }
    private void Update()
    {
        if(lowerHead!= cms.GetSpeed(0) > speedToLowerHead)
        {
            StopAllCoroutines();
            StartCoroutine(LowerHead());
        }
        lowerHead = cms.GetSpeed(0) > speedToLowerHead;
        headPosition.localEulerAngles = new Vector3(headPosition.localEulerAngles.x, headPosition.localEulerAngles.y, cms.turningAxis*cms.steeringForce);

    }

    IEnumerator LowerHead()
    {
        desireHeadPosition = !lowerHead ? headLowPosition : headHighPosition;
        while(Mathf.Abs(headPosition.localPosition.x-desireHeadPosition)>epsilon)
        {
            headPosition.localPosition = new Vector3(Mathf.Lerp(headPosition.localPosition.x,desireHeadPosition,Time.deltaTime*speedOfChangingHeadHeight), headPosition.localPosition.y, headPosition.localPosition.z);
            yield return null;
        }
        yield return null;
    }
}
