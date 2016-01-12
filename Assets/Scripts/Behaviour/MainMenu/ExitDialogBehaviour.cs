using UnityEngine;
using System.Collections;

public class ExitDialogBehaviour : MonoBehaviour
{

    public Animator animator;

    public void OnAccept()
    {
        Application.Quit();
    }

    public void OnCancel()
    {
        animator.SetBool("isDisappear", true);
    }

    public void Show()
    {
        animator.SetBool("isDisappear", false);
        animator.Play("warning_dialog_appear", -1, 0);
    }
}
