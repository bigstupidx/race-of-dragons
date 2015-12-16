using UnityEngine;
using System.Collections;

public class SelectModeBehaviour : MonoBehaviour
{

    #region UI Delegate
    public void OnBackClick()
    {
        Application.LoadLevel("Scene_MainMenu");
    }

    public void OnPracticeModeClick()
    {
        Application.LoadLevel("Scene_Wating_Practice");
    }

    public void OnQuickRaceClick()
    {
        Application.LoadLevel("Scene_Wating"); 
    }

    public void OnInviteFriendClick()
    {

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
