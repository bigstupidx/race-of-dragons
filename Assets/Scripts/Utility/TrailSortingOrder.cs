using UnityEngine;
using System.Collections;

public class TrailSortingOrder : MonoBehaviour
{

    public string sortingLayerName;
    public int sortingOrder;
    public TrailRenderer trail;
    
    void Start ()
    {
        trail.sortingLayerName = sortingLayerName;
        trail.sortingOrder = sortingOrder;
    }	
}
