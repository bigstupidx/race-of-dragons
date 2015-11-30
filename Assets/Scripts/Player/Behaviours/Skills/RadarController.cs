using UnityEngine;
using System.Collections;
using System;

public class RadarController : MonoBehaviour
{
    public GameObject target;
    public int playerId;


    void Start()
    {
          
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals("Player") && target == null && playerId != 0)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController.PlayerId != playerId)
                target = other.gameObject;
        }
    }

    void Update()
    {

    }
}
