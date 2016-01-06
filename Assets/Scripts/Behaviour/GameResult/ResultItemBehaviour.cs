using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultItemBehaviour : MonoBehaviour
{
    public Text textName;
    public Text textTime;
    public Image avatar;
    public GameObject btnAddFriend;

    private string id;
	
	void Start ()
    {
	
	}
		
    public void SetText(string name, string time, string id)
    {
        this.id = id;
        this.textName.text = name;
        this.textTime.text = time;
        if (id == PlayerData.Current.id || PlayerData.Current.friendList.Contains(id))
        {
            btnAddFriend.SetActive(false);
        }
    }	

    public void LoadAvatar(string avatarUrl)
    {
        StartCoroutine(GameUtils.Instance._DownloadImage(avatarUrl, avatar));
    }

    public void OnAddFriendClick()
    {
        SoundManager.Instance.playButtonSound();
        btnAddFriend.SetActive(false);
        PlayerData.Current.friendList.Add(id);
        PlayerData.Current.Save();
    }
}
