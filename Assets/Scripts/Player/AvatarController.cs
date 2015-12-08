using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarController : Photon.PunBehaviour
{
    [HideInInspector] public PlayerController player;
    public float mapWidth = 360.0f;
    public Image arrow;

    private float ratio = 650.0f;
    private RectTransform rectTransform;
    private PhotonView playerView;
    private bool inited;

	void Start ()
    {
        rectTransform = GetComponent<RectTransform>();
    }
		
	void Update ()
    {
        // update posX
        if (inited)
        {
            float playerPosX = GameUtils.GetCustomProperty<float>(playerView, "POS_X", 0);            
            float posX = (playerPosX / mapWidth) * ratio - ratio * 0.5f;
            rectTransform.localPosition = new Vector3(posX, 0, 0);
        }
	}

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
        playerView = player.GetComponent<PhotonView>();       

        inited = true;
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GameObject progressBar = GameObject.FindGameObjectWithTag("ProgressBar");
        Debug.Log("Progress Bar: " + progressBar);
        transform.parent = progressBar.transform;

        rectTransform = GetComponent<RectTransform>();        
        rectTransform.localScale = new Vector3(0.5f, 0.5f, 1);
        rectTransform.localPosition = Vector3.zero;

        if (info.sender.ID != PhotonNetwork.player.ID)
        {
            Image image = GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0.7f);
            arrow.enabled = false;
        }
        else
        {
            arrow.enabled = true;
        }
    }
}
