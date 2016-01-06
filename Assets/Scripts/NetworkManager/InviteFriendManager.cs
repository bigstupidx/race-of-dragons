using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Parse;

public class InviteFriendManager : Photon.MonoBehaviour
{
    public static string roomName = "";
    public static string firstInviteTo = "";

    public GameObject friendDialogPrefab;
    public Text textCountDown;

    public Transform[] cardHolders;
    public Transform backgrounds;

    public GameObject playButton;
    public GameObject inviteFriendButton;

    private bool connectFailed = false;
    private bool joined = false;
    private float timeCountDown;
    private bool isStartGame;
    private float startTime;

    public void Awake()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.offlineMode = false;

        // the following line checks if this client was just created (and not yet online). if so, we connect
        if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
        {
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings("0.1");
            //PhotonNetwork.ConnectToMaster("192.168.1.18", 5055, "26dafad4-ed7b-42be-a64d-26ce161fede3", "0.1");
        }

        // generate a name for this player, if none is assigned yet
        if (String.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            PhotonNetwork.playerName = PlayerData.Current.name;
        }

        // if you wanted more debug out, turn this on:
        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
    }

    public void OnJoinedLobby()
    {
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 4, isVisible = false }, TypedLobby.Default);
        }
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        roomName = null;        

        joined = true;
        string dragonType = PlayerData.Current.CurrentDragon.element.ToString();

        string dragonPrefab = dragonType + "Dragon";
        GameObject player = PhotonNetwork.Instantiate(dragonPrefab, cardHolders[PhotonNetwork.playerList.Length - 1].position, Quaternion.identity, 0);
        var playerController = player.GetComponent<PlayerController>();
        playerController.Name = PlayerData.Current.name;
        playerController.ParseId = PlayerData.Current.id;
        playerController.AvatarUrl = PlayerData.Current.avatarUrl;

        player.transform.parent = cardHolders[PhotonNetwork.playerList.Length - 1];
        player.transform.localPosition = Vector3.zero;

        if (PhotonNetwork.isMasterClient)
        {
            playButton.SetActive(true);
            inviteFriendButton.SetActive(true);
            int rand = UnityEngine.Random.Range(0, 4);
            GameUtils.SetRoomCustomProperty<int>("MAP_ID", rand);
            backgrounds.GetChild(rand).gameObject.SetActive(true);
        }
        else
        {
            int mapId = GameUtils.GetRoomCustomProperty<int>("MAP_ID", 0);
            backgrounds.GetChild(mapId).gameObject.SetActive(true);
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate: " + info.sender);
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected: " + newPlayer.ID);
        GameObject[] listPlayer = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(listPlayer.Length);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
    }

    //public void OnPhotonRandomJoinFailed()
    //{       
    //    Debug.Log("OnPhotonRandomJoinFailed got called. Happens if no room is available (or all full or invisible or closed). JoinrRandom filter-options can limit available rooms.");
    //    PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 4 }, TypedLobby.Default);
    //}

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        var param = new Dictionary<string, object>();
        param.Add("from", PlayerData.Current.name);
        param.Add("to", firstInviteTo);
        param.Add("room", PhotonNetwork.room.name);

        var taskSync = ParseCloud.CallFunctionAsync<string>("invite", param).ContinueWith(t2 =>
        {
            if (t2.IsCompleted)
            {
                            
            }
        });

        firstInviteTo = "";
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon.");
    }

    public void OnFailedToConnectToPhoton(object parameters)
    {
        this.connectFailed = true;
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
    }

    public void OnMasterClientSwitched(PhotonPlayer player)
    {

    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom (local)");

        // back to main menu        
        //Application.LoadLevel(WaitingMenu.SceneNameMenu);
    }

    void Update()
    {
        if (joined && PhotonNetwork.connected)
        {
            startTime = (float)GameUtils.GetRoomCustomProperty<double>("START_TIME", 0);
            if (startTime != 0)
            {
                isStartGame = true;                
            }

            if (isStartGame)
            {
                timeCountDown = (float)(PhotonNetwork.time - startTime);
                if (timeCountDown <= GameConsts.Instance.TIME_COUNT_DOWN_TO_PLAY + 1)
                    textCountDown.text = "Game Starting in " + (int)(GameConsts.Instance.TIME_COUNT_DOWN_TO_PLAY + 1 - timeCountDown) + "";

                if (timeCountDown >= GameConsts.Instance.TIME_COUNT_DOWN_TO_PLAY)
                {
                    PhotonNetwork.room.visible = false;

                    if (PhotonNetwork.isMasterClient)
                    {
                        //int rand = UnityEngine.Random.Range(0, 4);
                        //GameUtils.SetRoomCustomProperty<int>("MAP_ID", rand);
                        PhotonNetwork.LoadLevel("Scene_Game");
                    }

                    timeCountDown = GameConsts.Instance.TIME_COUNT_DOWN_TO_PLAY;
                }
            }
        }

    }    

    #region UI Delegate
    public void OnBackClick()
    {
        SoundManager.Instance.playButtonSound();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        Application.LoadLevel("Scene_MainMenu");
    }

    public void OnPlayClick()
    {
        SoundManager.Instance.playButtonSound();
        if (PhotonNetwork.isMasterClient && isStartGame == false)
        {
            isStartGame = true;
            startTime = (float)PhotonNetwork.time;
            GameUtils.SetRoomCustomProperty<double>("START_TIME", PhotonNetwork.time);
        }
    }

    public void OnInviteClick()
    {
        SoundManager.Instance.playButtonSound();
        Instantiate(friendDialogPrefab);
    }
    #endregion
}
