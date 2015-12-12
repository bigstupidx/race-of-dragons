using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FriendItem
{
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public int Level { get; set; }
    public DateTime? LastTimeAcess { get; set; }
    public int Rank { get; set; }
    public int WinRate { get; set; }

    public FriendItem()
    {
        Name = "User name";
        AvatarUrl = "http://res.cloudinary.com/thienle/image/upload/c_scale,r_30,w_128/v1449901077/Lord%20of%20Dragons/default_avatar.png";
        Level = 1;
        LastTimeAcess = DateTime.Now;
        Rank = 0;
        WinRate = 0;
    }
}

public class FriendDialogController : Singleton<FriendDialogController>
{
    public GameObject friendItem;
    public GameObject contain;

    private Animator animator;    
	
	void Start ()
    {
        animator = GetComponent<Animator>();
        var friendList = PlayerData.Current.friends;
        foreach (var item in friendList)
        {
            GameObject friend = Instantiate(friendItem) as GameObject;
            FriendItemController friendController = friend.GetComponent<FriendItemController>();
            friendController.SetFriendInfo(item.Value);
            friend.transform.parent = contain.transform;
            friend.transform.localScale = new Vector3(1, 1, 1);
        }

        Show();
	}
		
	void Update ()
    {
	
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
}
