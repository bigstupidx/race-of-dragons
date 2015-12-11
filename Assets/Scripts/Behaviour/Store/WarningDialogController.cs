using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class WarningDialogController : Singleton<WarningDialogController>
{
    public Text title;    
    public Animator animator;
    public Action onAccept;

    private string[] info = new string[] { "Not enough coins. Do you want to buy more?",
                                            "Not enough gems. Do you want to buy more?"};

    void Start ()
    {
        
	}
	
    public void Show()
    {
        this.gameObject.SetActive(true);
        Reset();
    }

    public void Show(string info)
    {
        title.text = info;
        this.gameObject.SetActive(true);
        Reset();
    }

    public void ShowNotEnoughCoins()
    {
        onAccept = () =>
        {
            MoreCoinDialogController.Instance.Show();
        };
        Show(info[0]);
    }

    public void ShowNotEnoughGems()
    {
        onAccept = () =>
        {
            MoreCoinDialogController.Instance.Show();
        };
        Show(info[1]);
    }

    public void Reset()
    {
        animator.Play("warning_dialog_appear", -1, 0);
        animator.SetBool("isDisappear", false);
    }

    public void OnCancle()
    {
        animator.SetBool("isDisappear", true);
    }

    public void OnAccept()
    {
        animator.SetBool("isDisappear", true);
        if (onAccept != null)
            onAccept();
    }
}
