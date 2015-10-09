// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkerMenu.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMatchMaker : Photon.PunBehaviour
{
    public GUISkin skin;
    public Vector2 widthAndHeight = new Vector2(600, 400);
    public TextMesh textCountDown;

    private bool connectFailed = false;
    private bool joined = false;

    public static readonly string SceneNameMenu = "Scene_Waiting";
    public static readonly string SceneNameGame = "Scene_Game";

    private string errorDialog;
    private double timeToClearDialog;
    private float timeCountDown;

    public string ErrorDialog
    {
        get { return this.errorDialog; }
        private set
        {
            this.errorDialog = value;
            if (!string.IsNullOrEmpty(value))
            {
                this.timeToClearDialog = Time.time + 4.0f;
            }
        }
    }

    public void Awake()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;

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
            PhotonNetwork.playerName = "Guest" + Random.Range(1, 9999);            
        }

        // if you wanted more debug out, turn this on:
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
    }

    public void OnGUI()
    {
        if (this.skin != null)
        {
            GUI.skin = this.skin;
        }
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        
        if (PhotonNetwork.room != null)
        {
            GUILayout.Label(PhotonNetwork.room.playerCount.ToString());
        }
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        GUILayout.Space(15);

        if (!string.IsNullOrEmpty(ErrorDialog))
        {
            GUILayout.Label(ErrorDialog);

            if (this.timeToClearDialog < Time.time)
            {
                this.timeToClearDialog = 0;
                ErrorDialog = "";
            }
        }

    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        joined = true;
    }


    public void OnPhotonRandomJoinFailed()
    {
        ErrorDialog = "Error: Can't join random room (none found).";
        Debug.Log("OnPhotonRandomJoinFailed got called. Happens if no room is available (or all full or invisible or closed). JoinrRandom filter-options can limit available rooms.");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon.");
    }

    public void OnFailedToConnectToPhoton(object parameters)
    {
        this.connectFailed = true;
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
    }

    void Update()
    {
        if (joined)
        {
            if (PhotonNetwork.playerList.Length >= PhotonNetwork.room.maxPlayers)
            {
                timeCountDown += Time.deltaTime;
                textCountDown.text = (int)(GameConsts.Instance.TIME_COUNT_DOWN_TO_PLAY + 1 - timeCountDown) + "";

                if (timeCountDown >= GameConsts.Instance.TIME_COUNT_DOWN_TO_PLAY)
                {
                    PhotonNetwork.room.visible = false;
                    Debug.Log("TEST: " + timeCountDown);
                    Application.LoadLevel(1);
                    timeCountDown = 0;
                }
            }
        }
        
    }
}
