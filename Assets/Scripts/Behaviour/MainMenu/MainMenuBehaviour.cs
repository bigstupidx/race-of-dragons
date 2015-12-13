using UnityEngine;
using System.Collections;
using Parse;

public class MainMenuBehaviour : MonoBehaviour
{

    public GameObject optionDialog;
    public GameObject friendDialogPrefab;
    public GameObject loginDialogPrefab;

    private FriendDialogController friendDialog;

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
        if (friendDialog != null)
        {
            friendDialog.Show();
        }
        else
        {
            GameObject go = Instantiate(friendDialogPrefab) as GameObject;
            friendDialog = go.GetComponentInChildren<FriendDialogController>();
        }

    }

    public void OnRaceClick()
    {
        Application.LoadLevel("Scene_Select_Mode");
    }
    #endregion

    #region Mono Behaviour    
    void Start()
    {
        if (ParseUser.CurrentUser == null)
        {
            Instantiate(loginDialogPrefab);
        }
        else
        {
            PlayerData.Current.SaveOnServer();
        }
    }

    void Update()
    {

    }


    #endregion
}
