using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionBehaviour : MonoBehaviour {

    public Text username;
    public Animator animator;

    #region UI Delegate

    public void OnBackClick()
    {
        animator.SetBool("isDisAppear", true);
    }

    public void OnExitDialog()
    {
        Destroy(this.gameObject);
    }

    public void OnMusicClick()
    {

    }

    public void OnSoundClick()
    {

    }

    public void OnHelpClick()
    {

    }

    public void OnLoginViaFacebookClick()
    {

    }

    public void OnLogoutFacebookClick()
    {

    }

    public void OnRemoveAdsClick()
    {

    }

    public void OnCreditClick()
    {

    }

    public void OnEmailFeedbackClick()
    {

    }
    #endregion

    #region Mono Beaviour



    public void Start()
    {
        username.text = PlayerData.Current.name;
    } 
    #endregion

}
