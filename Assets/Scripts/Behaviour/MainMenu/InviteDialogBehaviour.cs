using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InviteDialogBehaviour : MonoBehaviour
{
    public Text title;
    public Animator animator;
    public GameObject inviteDialog;

    private string roomName;

	void Start ()
    {
	
	}
	
    public void SetInfo(string from, string roomName)
    {
        title.text = from + " has invited you to play a match!";
        this.roomName = roomName;
    }

    public void OnCancle()
    {
        animator.SetBool("isDisappear", true);
    }

    public void OnAccept()
    {
        animator.SetBool("isDisappear", true);
        InviteFriendManager.roomName = roomName;
        Application.LoadLevel("Scene_Wating_Friend");
    }

    public void DestroyItself()
    {
        Destroy(inviteDialog);
    }
}
