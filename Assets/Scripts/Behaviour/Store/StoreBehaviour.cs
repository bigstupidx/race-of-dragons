using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreBehaviour : MonoBehaviour
{
   

    void Start()
    {
       
    }

	public void OnBackClick()
    {
        SoundManager.Instance.playButtonSound();
        PlayerData.Current.SaveOnServer();
        Application.LoadLevel("Scene_MainMenu");
    }

}
