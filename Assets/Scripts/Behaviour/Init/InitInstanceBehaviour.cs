using UnityEngine;
using System.Collections;

public class InitInstanceBehaviour : MonoBehaviour
{
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        PlayerData.Current.Load();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
