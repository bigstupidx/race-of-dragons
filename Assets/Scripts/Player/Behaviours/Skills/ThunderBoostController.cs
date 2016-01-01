using UnityEngine;
using System.Collections;

public class ThunderBoostController : Photon.PunBehaviour
{
    public float timeExist = 5;    

    private float timer;    

	void Start ()
    {
        
	}
		
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= timeExist)
        {            
            timer = 0;
            PhotonNetwork.Destroy(this.gameObject);
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
                timeExist = 1;
            }
        }
    }
}
