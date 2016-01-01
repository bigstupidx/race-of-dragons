using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionBehaviour : MonoBehaviour {

    public Text username;
    public Animator animator;

    #region UI Delegate

    public void OnBackClick()
    {
        SoundManager.Instance.playButtonSound();
        animator.SetBool("isDisAppear", true);
    }

    public void OnExitDialog()
    {        
        Destroy(this.gameObject);
    }

    public void OnMusicClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnSoundClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnHelpClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnLoginViaFacebookClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnLogoutFacebookClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnRemoveAdsClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnCreditClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnEmailFeedbackClick()
    {
        SoundManager.Instance.playButtonSound();
    }
    #endregion

    #region Mono Beaviour



    public void Start()
    {
        username.text = PlayerData.Current.name;
    } 
    #endregion

}
