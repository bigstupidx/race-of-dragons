using UnityEngine;
using System.Collections;
using Parse;

public class MainMenuBehaviour : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject optionDialog;
    public GameObject friendDialogPrefab;
    public GameObject loginDialogPrefab;

    public ExitDialogBehaviour exitDialog;

    private FriendDialogController friendDialog;

    #region UI Delegate
    public void OnOptionClick()
    {
        SoundManager.Instance.playButtonSound();
        Instantiate(optionDialog);
    }

    public void OnStoreClick()
    {
        SoundManager.Instance.playButtonSound();
        Application.LoadLevel("Scene_Store");
    }

    public void OnFriendsClick()
    {
        SoundManager.Instance.playButtonSound();
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
        SoundManager.Instance.playButtonSound();
        Application.LoadLevel("Scene_Select_Mode");
    }
    #endregion

    #region Mono Behaviour    
    void Start()
    {
        SoundManager.Instance.playMenuBackgroundMusic();

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitDialog.Show();
        }
    }


    #endregion
}
