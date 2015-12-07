using UnityEngine;
using System.Collections;

public class AvatarNetworkManager : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos;
    private RectTransform rectTransform;

    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data            
            stream.SendNext(rectTransform.localPosition);
        }
        else
        {
            //Network player, receive data           
            correctPlayerPos = (Vector3)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, correctPlayerPos, Time.deltaTime * 5);           
        }
    }
}

