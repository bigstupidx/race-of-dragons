using UnityEngine;
using System.Collections;
using System;

public class PracticeManager : MonoBehaviour
{
    private bool inited;

    void Awake()
    {        
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.offlineMode = true;
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

        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 1 }, TypedLobby.Default);
    }

	// Use this for initialization
	void Start ()
    {

    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        inited = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (inited)
        {
            //PhotonNetwork.room.visible = false;

            //if (PhotonNetwork.isMasterClient)
            {
                int rand = UnityEngine.Random.Range(0, 4);
                GameUtils.SetRoomCustomProperty<int>("MAP_ID", rand);
                PhotonNetwork.LoadLevel("Scene_Game");
            }
        }
    }
}
