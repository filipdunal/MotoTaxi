using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class HudController : MonoBehaviour
{
    public Text speedValue;
    public Text gearValue;
    public CarMovementScript cms;
    public GameObject sun;
    public GameObject pausePanel;
    public Text hdrStatus;
    private void OnGUI()
    {
        speedValue.text = cms.GetSpeed(0).ToString();
        gearValue.text = cms.actualGear.number.ToString();
        if(UniversalRenderPipeline.asset.supportsHDR)
        {
            hdrStatus.color = Color.green;
        }
        else
        {
            hdrStatus.color = Color.red;
        }
    }

    public void SwitchDayNight()
    {
        sun.SetActive(!sun.activeInHierarchy);
    }

    public void Pause(bool condition)
    {
        pausePanel.SetActive(condition);
        Time.timeScale = condition ? 0f : 1f;
        AudioListener.pause = condition;
    }

    public void SwitchHDR()
    {
        UniversalRenderPipeline.asset.supportsHDR = !UniversalRenderPipeline.asset.supportsHDR;
    }


}
