using UnityEngine;
using System.Collections;

public class ToggleButtonController : MonoBehaviour 
{
    private Animator animator;
    private bool isButtonOn;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnClick()
    {        
        if (!isButtonOn)
        {
            animator.Play("Pressed", -1, 0);
            isButtonOn = true;
        }
    }

    public void ResetNormal()
    {
        animator.Play("Normal", -1, 0);
        isButtonOn = false;
    }
}
