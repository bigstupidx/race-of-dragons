using UnityEngine;
using System.Collections;
using System;

public class RadarController : MonoBehaviour
{
    public GameObject target;
    [NonSerialized]
    public int parentId;


    void Start()
    {
          
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals("Player") && target == null && parentId != null && other.gameObject.GetInstanceID() != parentId)
        {
            target = other.gameObject;
        }
    }

    void Update()
    {

    }
}
