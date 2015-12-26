using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PlayerNetworkManager : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    private PhotonView playerView;
    private PlayerController playerController;

    void Awake()
    {
        //playerView = GetComponent<PhotonView>();
        playerController = GetComponent<PlayerController>();
    }

	// Use this for initialization
	void Start ()
    {
	
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data            
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            playerController.PosX = transform.position.x;
        }
        else
        {
            //Network player, receive data            
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update () {
        if (!photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);            
        }
    }
}
