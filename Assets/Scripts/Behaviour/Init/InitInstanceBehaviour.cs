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

        SceneManager.Instance.LoadScene(E_SCENE.Scene_MainMenu, null);
	}

}
