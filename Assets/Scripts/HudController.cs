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
    public Animator circleAnimator;
    public float slowMotionSpeed=0.5f;
    public GameObject postProcessing;

    [Header("Tachometer")]
    public Image rpmTachometer;
    public Color lowRPM;
    public Color midRPM;
    public Color highRPM;
    public Color uberRPM;
    Color targetColor;

    bool pause;
    bool motoSettings;


    private void OnGUI()
    {
        speedValue.text = cms.GetSpeed(0).ToString();
        gearValue.text = cms.actualGear.number.ToString();
        if(gearValue.text=="0")
        {
            gearValue.text = "N";
        }
        if (UniversalRenderPipeline.asset.supportsHDR)
        {
            hdrStatus.color = Color.green;
        }
        else
        {
            hdrStatus.color = Color.red;
        }

        float rpmPercent= cms.engineRPMsmooth / cms.tachometerMaxRPM;
        rpmTachometer.fillAmount = rpmPercent * 0.25f;

        if (rpmPercent < 0.33f)
            rpmTachometer.color = Color.Lerp(lowRPM, midRPM, rpmPercent * 3f);
        else if (rpmPercent < 0.66f)
            rpmTachometer.color = Color.Lerp(midRPM, highRPM, (rpmPercent-0.33f) * 3f);
        else
            rpmTachometer.color = Color.Lerp(highRPM, uberRPM, (rpmPercent-0.66f) * 3f);

    }

    public void SwitchDayNight()
    {
        sun.SetActive(!sun.activeInHierarchy);
    }

    public void Pause()
    {
        pause = !pause;
        pausePanel.SetActive(pause);
        Time.timeScale = pause ? 0f : 1f;
        AudioListener.pause = pause;
    }

    public void MotoSettings()
    {
        motoSettings = !motoSettings;
        circleAnimator.SetBool("Opened", motoSettings);
        //Time.timeScale = motoSettings ? slowMotionSpeed : 1f;
        
    }

    public void SwitchHDR()
    {
        UniversalRenderPipeline.asset.supportsHDR = !UniversalRenderPipeline.asset.supportsHDR;
        postProcessing.SetActive(!postProcessing.activeInHierarchy);
    }


}
