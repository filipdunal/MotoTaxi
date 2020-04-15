using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public Text speedValue;
    public CarMovementScript cms;
    public GameObject sun;
    private void OnGUI()
    {
        speedValue.text = cms.GetSpeed(0).ToString();
    }

    public void SwitchDayNight()
    {
        sun.SetActive(!sun.activeInHierarchy);
    }
    

}
