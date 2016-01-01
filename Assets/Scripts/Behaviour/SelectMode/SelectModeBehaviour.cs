using UnityEngine;
using System.Collections;

public class SelectModeBehaviour : MonoBehaviour
{

    #region UI Delegate
    public void OnBackClick()
    {
        SoundManager.Instance.playButtonSound();
        Application.LoadLevel("Scene_MainMenu");
    }

    public void OnPracticeModeClick()
    {
        SoundManager.Instance.playButtonSound();
        Application.LoadLevel("Scene_Wating_Practice");
    }

    public void OnQuickRaceClick()
    {
        SoundManager.Instance.playButtonSound();
        Application.LoadLevel("Scene_Wating"); 
    }

    public void OnInviteFriendClick()
    {
        SoundManager.Instance.playButtonSound();
        Application.LoadLevel("Scene_Wating_Friend");
    } 
    #endregion

    #region MonoBehaviour
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
