using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class FriendItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public int Level { get; set; }
    public DateTime? LastTimeAcess { get; set; }
    public int Rank { get; set; }
    public float WinRate { get; set; }
    public int Played { get; set; }
    public int Win { get; set; }

    public FriendItem()
    {
        Name = "User name";
        AvatarUrl = "http://res.cloudinary.com/thienle/image/upload/c_scale,r_30,w_128/v1449901077/Lord%20of%20Dragons/default_avatar.png";
        Level = 1;
        LastTimeAcess = null;
        Rank = 0;
        WinRate = 0;
        Played = 0;
        Win = 0;
    }

    public FriendItem(IDictionary<string, object> data)
    {
        Id = data["id"].ToString();
        Name = data["name"].ToString();
        AvatarUrl = data["avatarUrl"].ToString();
        Level = int.Parse(data["level"].ToString());
        Rank = int.Parse(data["rank"].ToString());
        Played = int.Parse(data["played"].ToString());
        Win = int.Parse(data["win"].ToString());
        if (Played > 0)
            WinRate = Win / Played;
    }
}

public class FriendDialogController : Singleton<FriendDialogController>
{
    public GameObject friendItem;
    public GameObject moreFriendDialogPrefab;
    public GameObject contain;

    private Animator animator;    
	
	void Start ()
    {
        animator = GetComponent<Animator>();
        Reload();
        Show();
	}
		
	void Update ()
    {
	
	}

    public void Reload()
    {
        for (int i = 0; i < contain.transform.childCount; i++)
        {
            Destroy(contain.transform.GetChild(i).gameObject);
        }

        var friendList = PlayerData.Current.friends;
        foreach (var item in friendList)
        {
            GameObject friend = Instantiate(friendItem) as GameObject;
            FriendItemController friendController = friend.GetComponent<FriendItemController>();
            friendController.SetFriendInfo(item.Value);
            friend.transform.parent = contain.transform;
            friend.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnBackClick()
    {
        animator.SetBool("isDisappear", true);
    }

    public void Show()
    {
        animator.SetBool("isDisappear", false);
        animator.Play("friend_dialog_appear", -1, 0);
    }

    public void OnMoreFriendClick()
    {
        Instantiate(moreFriendDialogPrefab);
    }
}
