using UnityEngine;
using System.Collections;

public class ConfirmQuitMatchDialogBehaviour : Singleton<ConfirmQuitMatchDialogBehaviour>
{
    public Animator animator;

	void Start ()
    {
	
	}
		
	public void OnQuit()
    {
        SoundManager.Instance.playButtonSound();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        Application.LoadLevel("Scene_MainMenu");
    }

    public void OnCancle()
    {
        SoundManager.Instance.playButtonSound();
        animator.SetBool("isDisappear", true);
    }    

    public void Show()
    {
        animator.SetBool("isDisappear", false);
        animator.Play("warning_dialog_appear", -1, 0);
    }
}
