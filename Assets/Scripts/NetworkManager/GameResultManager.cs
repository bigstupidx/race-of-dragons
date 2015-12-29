using UnityEngine;
using System.Collections;

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

        //listItemResult[index].SetText(playerController.Name, playerController.Result.ToString("0.0"));

        player.transform.position = listPosition[index].position;

        for (int i = 0; i < PhotonNetwork.room.playerCount; i++)
        {
            int pos = (int)PhotonNetwork.playerList[i].customProperties["POSITION"] - 1;
            string name = (string)PhotonNetwork.playerList[i].customProperties["NAME"];
            float result = (float)PhotonNetwork.playerList[i].customProperties["RESULT"];
            string avatarUrl = (string)PhotonNetwork.playerList[i].customProperties["AVATAR_URL"];
            string parseId = (string)PhotonNetwork.playerList[i].customProperties["PARSE_ID"];

            listItemResult[pos].gameObject.SetActive(true);
            listItemResult[pos].SetText(name, result.ToString("0.0"));
            listItemResult[pos].LoadAvatar(avatarUrl);
        }


    }
		
	void Update ()
    {
	
	}

    public void OnHomeClick()
    {
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel("Scene_MainMenu");
    }

    public void OnReplayClick()
    {
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel("Scene_Select_Mode");
    }
}
