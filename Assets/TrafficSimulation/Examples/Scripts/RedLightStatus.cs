// Traffic Simulation
// https://github.com/mchrbn/unity-traffic-simulation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficSimulation;

public class RedLightStatus : MonoBehaviour
{

    public int lightGroupId;  // Belong to traffic light 1 or 2?
    public Intersection intersection;

    GameObject redOn;
    GameObject orangeOn;
    GameObject greenOn;
    GameObject redOff;
    GameObject orangeOff;
    GameObject greenOff;
    
    bool isGreen;

    void Start(){
        AssingObjects();
        SetTrafficLightColor();
    }

    void AssingObjects()
    {
        redOn = transform.GetChild(0).GetChild(0).gameObject;
        orangeOn = transform.GetChild(0).GetChild(1).gameObject;
        greenOn = transform.GetChild(0).GetChild(2).gameObject;
        redOff = transform.GetChild(0).GetChild(3).gameObject;
        orangeOff = transform.GetChild(0).GetChild(4).gameObject;
        greenOff = transform.GetChild(0).GetChild(5).gameObject;
    }

    void Update(){
        SetTrafficLightColor();
    }

    void SetTrafficLightColor()
    {
        if(lightGroupId == intersection.curLightRed)
        {
            if (isGreen)
            {
                isGreen = false;
                //Change to red
                StartCoroutine(SwitchRed());
            }
        }
        else
        {
            if(!isGreen)
            {
                isGreen = true;
                //Change to green
                StartCoroutine(SwitchGreen());
            }
        }      
    }

    IEnumerator SwitchGreen()
    {
        RedStatus(true);
        OrangeStatus(true);
        GreenStatus(false);

        yield return new WaitForSeconds(intersection.orangeLightDuration);

        RedStatus(false);
        OrangeStatus(false);
        GreenStatus(true);
    }

    IEnumerator SwitchRed()
    {
        RedStatus(false);
        OrangeStatus(true);
        GreenStatus(false);

        yield return new WaitForSeconds(intersection.orangeLightDuration);

        RedStatus(true);
        OrangeStatus(false);
        GreenStatus(false);
    }

    void GreenStatus(bool condition)
    {
        greenOn.SetActive(condition);
        greenOff.SetActive(!condition);
    }

    void OrangeStatus(bool condition)
    {
        orangeOn.SetActive(condition);
        orangeOff.SetActive(!condition);
    }

    void RedStatus(bool condition)
    {
        redOn.SetActive(condition);
        redOff.SetActive(!condition);
    }
}
