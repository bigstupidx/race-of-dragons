using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfoDialogController : Singleton<PlayerInfoDialogController>
{
    private Animator animator;

    public Image avatar;
    public Text name;
    public Text level;
    public Text rank;
    public Text winrate;
	
	void Start ()
    {
        animator = GetComponent<Animator>();
	}
		
	void Update ()
    {
	
	}

    public void ShowInfo(FriendItem info)
    {        
        name.text = info.Name;
        level.text = info.Level + "";
        rank.text = info.Rank + "";
        winrate.text = info.WinRate + "";

        animator.SetBool("isDisappear", false);
        animator.Play("player_infor_dialog_appear", -1, 0);

        StartCoroutine(GameUtils.Instance._DownloadImage(info.AvatarUrl, avatar));
    }

    public void OnBackClick()
    {
        animator.SetBool("isDisappear", true);
    }    
}
