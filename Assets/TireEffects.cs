using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireEffects : MonoBehaviour
{
    public WheelCollider frontWheelColl;
    public WheelCollider rearWheelColl;
    public AudioSource tireSoundSource;
    public AnimationCurve slipVolume;

    WheelHit frontWH;
    WheelHit rearWH;
    float frontSlip;
    float rearSlip;
    float summarySlip;
    float summarySlipSmoothed;
    public float soundLerp;

    private void Update()
    {
        frontWheelColl.GetGroundHit(out frontWH);
        rearWheelColl.GetGroundHit(out rearWH);

        frontSlip = Mathf.Abs(frontWH.sidewaysSlip) + Mathf.Abs(frontWH.forwardSlip);
        rearSlip= Mathf.Abs(rearWH.sidewaysSlip) + Mathf.Abs(rearWH.forwardSlip);
        summarySlip = frontSlip + rearSlip;
        summarySlipSmoothed = Mathf.Lerp(summarySlipSmoothed,summarySlip, soundLerp * Time.deltaTime);

        //Debug.Log(Mathf.Abs(summarySlipSmoothed) > slipSoundMargin);
        tireSoundSource.volume = slipVolume.Evaluate(summarySlipSmoothed);

    }
}
