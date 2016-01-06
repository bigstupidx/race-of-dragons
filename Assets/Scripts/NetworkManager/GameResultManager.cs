using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameResultManager : Photon.MonoBehaviour
{
    public Transform[] listPosition;
    public ResultItemBehaviour[] listItemResult;

	void Awake()
    {

    }

	void Start ()
    {
        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        string dragonType = PlayerData.Current.CurrentDragon.element + "Dragon";
        GameObject player = PhotonNetwork.Instantiate(dragonType, transform.position, Quaternion.identity, 0);
        var playerController = player.GetComponent<PlayerController>();
        int index = playerController.EndPosition - 1;               

        List<int> listPosX = new List<int>();
        for (int i = 0; i < PhotonNetwork.room.playerCount; i++)
        {
            float posX = (float)PhotonNetwork.playerList[i].customProperties["POS_X"];
            listPosX.Add((int)posX);
        }
        listPosX.Sort();

        if (index < 0)
        {
            index = listPosX.Count - 1 - listPosX.IndexOf((int)playerController.PosX);
        }
        player.transform.position = listPosition[index].position;

        for (int i = 0; i < PhotonNetwork.room.playerCount; i++)
        {
            int pos = -1;
            float result = 0;
            if (PhotonNetwork.playerList[i].customProperties.ContainsKey("POSITION"))
            {
                pos = (int)PhotonNetwork.playerList[i].customProperties["POSITION"] - 1;
                result = (float)PhotonNetwork.playerList[i].customProperties["RESULT"];
            }

            if (pos <= -1)
            {
                float posX = (float)PhotonNetwork.playerList[i].customProperties["POS_X"];
                pos = listPosX.Count - 1 - listPosX.IndexOf((int)posX);
            }
            
            string name = (string)PhotonNetwork.playerList[i].customProperties["NAME"];
            string avatarUrl = (string)PhotonNetwork.playerList[i].customProperties["AVATAR_URL"];
            string parseId = (string)PhotonNetwork.playerList[i].customProperties["PARSE_ID"];

            listItemResult[pos].gameObject.SetActive(true);
            listItemResult[pos].SetText(name, result.ToString("0.0"), parseId);
            listItemResult[pos].LoadAvatar(avatarUrl);
        }

        SoundManager.Instance.playMenuBackgroundMusic();
    }
		
	void Update ()
    {
	
	}

    public void OnHomeClick()
    {
        SoundManager.Instance.playButtonSound();
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel("Scene_MainMenu");
    }

    public void OnReplayClick()
    {
        SoundManager.Instance.playButtonSound();
        PhotonNetwork.LeaveRoom();        
        Application.LoadLevel("Scene_Select_Mode");
    }
}
