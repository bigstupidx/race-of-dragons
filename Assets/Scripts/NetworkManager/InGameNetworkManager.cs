using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameNetworkManager : Photon.MonoBehaviour
{
    public Text position;
    public Image skillHolder;
    public Image itemHolder;
    public PhotonView playerView;
    public PlayerController playerController;

    void Start()
    {
        // in case we started this demo with the wrong scene being active, simply load the menu scene
        if (!PhotonNetwork.connected)
        {
            //Application.LoadLevel(WaitingMenu.SceneNameMenu);
            return;
        }

        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        string dragonType = PlayerData.Current.CurrentDragon.element + "Dragon";
        GameObject player = PhotonNetwork.Instantiate(dragonType, transform.position, Quaternion.identity, 0);
        SmoothCamera2D smoothCamera = Camera.main.GetComponent<SmoothCamera2D>();
        smoothCamera.target = player;

        playerController = player.GetComponent<PlayerController>();
        playerController.controlable = true;
        
    }

    void Update()
    {
        position.text = GetPositionOfCurrentPlayer();
    }

    string GetPositionOfCurrentPlayer()
    {
        GameObject[] listPlayer = GameObject.FindGameObjectsWithTag("Player");
        List<float> listPosX = new List<float>();
        for (int i = 0; i < listPlayer.Length; i++)
        {
            var player = listPlayer[i].GetComponent<PlayerController>();
            listPosX.Add(player.PosX);
        }
        listPosX.Sort();

        int position = listPosX.Count - listPosX.IndexOf(playerController.PosX);

        switch (position)
        {
            case 1:
                return "1st";                
            case 2:
                return "2nd";
            case 3:
                return "3rd";
            default:
                return "4th";
        }
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.GetPing() + "");
    }

    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        Debug.Log("OnMasterClientSwitched: " + player);

        string message;
        InRoomChat chatComponent = GetComponent<InRoomChat>();  // if we find a InRoomChat component, we print out a short message

        if (chatComponent != null)
        {
            // to check if this client is the new master...
            if (player.isLocal)
            {
                message = "You are Master Client now.";
            }
            else
            {
                message = player.name + " is Master Client now.";
            }


            chatComponent.AddLine(message); // the Chat method is a RPC. as we don't want to send an RPC and neither create a PhotonMessageInfo, lets call AddLine()
        }
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom (local)");

        // back to main menu        
        //Application.LoadLevel(WaitingMenu.SceneNameMenu);
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");

        // back to main menu        
        //Application.LoadLevel(WaitingMenu.SceneNameMenu);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate " + info.sender);    // you could use this info to store this or react
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected: " + player);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("OnPlayerDisconneced: " + player);
    }

    public void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton");

        // back to main menu        
        //Application.LoadLevel(WaitingMenu.SceneNameMenu);
    }
}
