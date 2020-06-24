using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneHUD : MonoBehaviour
{
    public Text speedValue;
    public Text speedUnit;
    CarMovementScript cms;

    private void Start()
    {
        cms = FindObjectOfType<CarMovementScript>();
    }
    private void FixedUpdate()
    {
        speedValue.text = cms.GetSpeed(0).ToString();
    }
}
