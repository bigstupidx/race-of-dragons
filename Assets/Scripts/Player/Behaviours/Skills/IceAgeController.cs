using UnityEngine;
using System.Collections;

public class IceAgeController : Photon.PunBehaviour
{
    public float timeExist;

    private Animator animator;
    private float timer;

    public int ID
    {
        get
        {
            return photonView.ownerId;
        }
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeExist)
        {
            animator.SetBool("isDisappear", true);
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
                timeExist = 15;//player.dragonPropertie.time;
            }
        }
    }
}
