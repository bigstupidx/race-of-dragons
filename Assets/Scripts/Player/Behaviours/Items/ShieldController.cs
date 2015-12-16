using UnityEngine;
using System.Collections;

public class ShieldController : Photon.PunBehaviour
{
    public float timeExist = 5;
    public PlayerController player;

    private float timer;

    void Start()
    {
        timeExist = PlayerData.Current.items[Item.Shield.ToString()].GetDuration();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeExist)
        {
            timer = 0;
            PhotonNetwork.Destroy(this.gameObject);
            if (player != null)
                player.hasShield = false;
        }
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GameObject[] listPlayer = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < listPlayer.Length; i++)
        {
            PlayerController player = listPlayer[i].GetComponent<PlayerController>();
            if (player.PlayerId == info.sender.ID)
            {
                this.transform.parent = player.transform;
                transform.localPosition = Vector3.zero;
                this.player = player;
            }
        }
    }
}