using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FriendItemController : MonoBehaviour
{
    [HideInInspector] public FriendItem info;
    public Text name;
    public Image avatar;
    public GameObject status;
    public bool isOnline;
	
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
        status.SetActive(isOnline);

        StartCoroutine(GameUtils.Instance._DownloadImage(friend.AvatarUrl, avatar));
    }

    public void OnShowInfoClick()
    {
        PlayerInfoDialogController.Instance.ShowInfo(info);
    }
}
