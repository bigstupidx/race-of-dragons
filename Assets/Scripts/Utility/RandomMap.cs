using UnityEngine;
using System.Collections;

public class RandomMap : MonoBehaviour
{
    public GameObject[] listMap;

    void Awake()
    {
        int mapId = GameUtils.GetRoomCustomProperty<int>("MAP_ID", 0);
        listMap[mapId].SetActive(true);
    }

	// Use this for initialization
	void Start ()
    {
	
	}
}
