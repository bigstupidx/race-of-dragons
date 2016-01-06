using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Parse;

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
        rank.text = "-";
        winrate.text = info.WinRate + "%";

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

        StartCoroutine(_GetRank(info.Id, rank));
        StartCoroutine(GameUtils.Instance._DownloadImage(info.AvatarUrl, avatar));
    }

    private IEnumerator _GetRank(string id, Text rankText)
    {
        var param = new Dictionary<string, object>();
        param.Add("userId", id);

        var task = ParseCloud.CallFunctionAsync<int>("getRankOfUser", param);

        while (!task.IsCompleted) yield return null;

        rankText.text = task.Result + "";
    }
    
    public void OnBackClick()
    {
        SoundManager.Instance.playButtonSound();
        animator.SetBool("isDisappear", true);
        FriendDialogController.Instance.Reload();        
    }

    public void OnAddFriendClick()
    {
        SoundManager.Instance.playButtonSound();
        animator.SetBool("isDisappear", true);
        PlayerData.Current.friendList.Add(friend.Id);
        PlayerData.Current.friends.Add(friend.Id, friend);
        PlayerData.Current.Save();

        FriendDialogController.Instance.Reload();
    }

    public void OnInviteClick()
    {
        SoundManager.Instance.playButtonSound();
        if (Application.loadedLevelName != "Scene_Wating_Friend")
        {
            InviteFriendManager.firstInviteTo = friend.Id;
            Application.LoadLevel("Scene_Wating_Friend");
        }
        else
        {
            var param = new Dictionary<string, object>();
            param.Add("from", PlayerData.Current.name);
            param.Add("to", friend.Id);
            param.Add("room", PhotonNetwork.room.name);

            var taskSync = ParseCloud.CallFunctionAsync<string>("invite", param).ContinueWith(t2 =>
            {
                if (t2.IsCompleted)
                {
                    
                }
            });

            animator.SetBool("isDisappear", true);
        }        
    }
}
