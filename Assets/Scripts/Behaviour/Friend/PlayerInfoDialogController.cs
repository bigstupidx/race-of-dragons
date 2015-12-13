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
    public GameObject btnInvite;
    public GameObject btnAdd;

    [HideInInspector] public FriendItem friend;
	
	void Start ()
    {
        animator = GetComponent<Animator>();
	}
		
	void Update ()
    {
	
	}

    public void ShowInfo(FriendItem info)
    {
        friend = info;
        name.text = info.Name;
        level.text = info.Level + "";
        rank.text = info.Rank + "";
        winrate.text = info.WinRate + "";

        if (PlayerData.Current.friendList.Contains(info.Id))
        {
            btnAdd.SetActive(false);
            btnInvite.SetActive(true);
        }
        else
        {
            btnAdd.SetActive(true);
            btnInvite.SetActive(false);
        }

        animator.SetBool("isDisappear", false);
        animator.Play("player_infor_dialog_appear", -1, 0);

        StartCoroutine(GameUtils.Instance._DownloadImage(info.AvatarUrl, avatar));
    }

    public void OnBackClick()
    {
        animator.SetBool("isDisappear", true);
        FriendDialogController.Instance.Reload();        
    }

    public void OnAddFriendClick()
    {
        animator.SetBool("isDisappear", true);
        PlayerData.Current.friendList.Add(friend.Id);
        PlayerData.Current.friends.Add(friend.Id, friend);
        PlayerData.Current.Save();

        FriendDialogController.Instance.Reload();
    }
}
