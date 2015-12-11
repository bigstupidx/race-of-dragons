using UnityEngine;
using System.Collections;

public class MoreCoinDialogController : Singleton<MoreCoinDialogController>
{
    public Animator animator;
	
	void Start ()
    {
	
	}

    public void Reset()
    {
        animator.SetBool("isDisappear", false);
        animator.Play("more_coin_dialog_appear", -1, 0);
    }

    public void Show()
    {
        Reset();
    }

    public void OnHide()
    {
        animator.SetBool("isDisappear", true);
    }

}
