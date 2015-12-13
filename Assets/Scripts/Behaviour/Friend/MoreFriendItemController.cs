using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoreFriendItemController : MonoBehaviour
{
    public Text name;
    public Image avatar;
    [HideInInspector] public FriendItem info;

	void Start ()
    {
	
	}
		
	void Update ()
    {
	
	}

    public void SetFriendInfo(FriendItem friend)
    {
        info = friend;

        name.text = friend.Name;      

        StartCoroutine(GameUtils.Instance._DownloadImage(friend.AvatarUrl, avatar));
    }

    public void OnShowInfoClick()
    {
        PlayerInfoDialogController.Instance.ShowInfo(info);
    }
}
