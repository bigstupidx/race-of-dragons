using UnityEngine;
using System.Collections;

public class MainMenuBehaviour : MonoBehaviour {

    public GameObject optionDialog;
    public GameObject friendDialog;

    #region UI Delegate
    public void OnOptionClick()
    {
        Instantiate(optionDialog);
    }

    public void OnStoreClick()
    {
        Application.LoadLevel("Scene_Store");        
    }

    public void OnFriendsClick()
    {
        Instantiate(friendDialog);
    }

    public void OnRaceClick()
    {
        Application.LoadLevel("Scene_Select_Mode");
    }
    #endregion

    #region Mono Behaviour
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
