using UnityEngine;
using System.Collections;

public class MainMenuBehaviour : MonoBehaviour {

    public GameObject optionDialog;

    #region UI Delegate
    public void OnOptionClick()
    {
        Instantiate(optionDialog);
    }

    public void OnStoreClick()
    {

    }

    public void OnFriendsClick()
    {

    }

    public void OnRaceClick()
    {

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
