using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisolveScript : MonoBehaviour
{
    Animator animator;
    public float changeSpeedStep = 0.2f;
    public Text speedText;
    public Button raiseSpeedButton;
    public Button lowerSpeedButton;

    private void Start()
    {
        animator=GetComponent<Animator>();
    }
    private void OnGUI()
    {
        speedText.text = animator.speed.ToString();
        if(animator.speed - changeSpeedStep > 0.05f)
        {
            lowerSpeedButton.interactable = true;
        }
        else
        {
            lowerSpeedButton.interactable = false;
        }
    }
    public void ShowOrHide()
    {

        if(animator.GetBool("visible"))
        {
            animator.SetBool("visible", false);
        }
        else
        {
            animator.SetBool("visible", true);
        }
    }
    public void RaiseSpeed()
    {
        animator.speed += changeSpeedStep;
    }
    public void LowerSpeed()
    {
        animator.speed -= changeSpeedStep;
    }

}
