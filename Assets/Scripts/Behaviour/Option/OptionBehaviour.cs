using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Parse;
using Facebook.Unity;
using System.Collections.Generic;

public class OptionBehaviour : MonoBehaviour {

    public Text username;
    public Animator animator;
    public Image avatar;
    public GameObject buttonLinkFacebook;
    public GameObject loadingDialogPrefab;
    public GameObject creditDialogPrefab;
    public GameObject tutorialDialogPrefab;

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
        Instantiate(tutorialDialogPrefab);
    }

    public void OnLoginViaFacebookClick()
    {
        SoundManager.Instance.playButtonSound();
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, delegate (ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Login success");
                StartCoroutine(_LinkFacebook());
            }
            else
            {
                Debug.Log("FBLoginCallback: User canceled login");
            }
        });
    }

    private IEnumerator _LinkFacebook()
    {
        if (FB.IsLoggedIn)
        {
            var aToken = AccessToken.CurrentAccessToken;
            // Logging in with Parse
            var linkTask = ParseFacebookUtils.LinkAsync(ParseUser.CurrentUser, aToken.UserId,
                                                          aToken.TokenString,
                                                          System.DateTime.Now);
            while (!linkTask.IsCompleted) yield return null;

            // Login completed, check results
            if (linkTask.IsFaulted || linkTask.IsCanceled)
            {
                // There was an error logging in to Parse
                foreach (var e in linkTask.Exception.InnerExceptions)
                {
                    ParseException parseException = (ParseException)e;
                    Debug.Log("ParseLogin: error message " + parseException.Message);
                    Debug.Log("ParseLogin: error code: " + parseException.Code);
                }
            }
            else
            {
                // Log in to Parse successful
                // Get user info
                FB.API("/me?fields=id,first_name", HttpMethod.GET, delegate (IGraphResult result)
                {
                    var resultObject = result.ResultDictionary;

                    if (resultObject != null)
                    {
                        var userProfile = new Dictionary<string, string>();

                        userProfile["facebookId"] = resultObject["id"].ToString();
                        userProfile["name"] = resultObject["first_name"].ToString();
                        userProfile["avatarUrl"] = "https://graph.facebook.com/" + userProfile["facebookId"] + "/picture?type=large&return_ssl_resources=1";

                        GameObject loadingDialog = Instantiate(loadingDialogPrefab) as GameObject;
                        var loadingDialogBehaviour = loadingDialog.GetComponent<LoadingDialogBehaviour>();
                        Dictionary<string, IEnumerator> listToDo = new Dictionary<string, IEnumerator>();

                        listToDo.Add("Sync data ...", _SaveUserProfile(userProfile));
                        listToDo.Add("Reloading ...", _PrepareSomeThing());
                        loadingDialogBehaviour.SetUpToDoList(listToDo);                        
                    }
                });
            }
        }
    }

    private IEnumerator _SaveUserProfile(Dictionary<string, string> profile)
    {
        var param = new Dictionary<string, object>();
        param.Add("data", profile);

        PlayerData.Current.name = profile["name"];
        PlayerData.Current.avatarUrl = profile["avatarUrl"];
        PlayerData.Current.Save();

        var taskSync = ParseCloud.CallFunctionAsync<ParseObject>("updateData", param).ContinueWith(t2 =>
        {

        });

        while (!taskSync.IsCompleted) yield return null;

        Debug.Log("Update data was successful.");                
    }

    private IEnumerator _PrepareSomeThing()
    {
        username.text = PlayerData.Current.name;
        yield return StartCoroutine(GameUtils.Instance._DownloadImage(PlayerData.Current.avatarUrl, avatar));
    }

    public void OnRemoveAdsClick()
    {
        SoundManager.Instance.playButtonSound();
    }

    public void OnCreditClick()
    {
        SoundManager.Instance.playButtonSound();
        Instantiate(creditDialogPrefab);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
    public void OnEmailFeedbackClick()
    {
        SoundManager.Instance.playButtonSound();
        string email = "trongthien18@gmail.com";
        string subject = MyEscapeURL("Race of Dragons");
        string body = MyEscapeURL("I love you :3");
#if UNITY_IOS || UNITY_ANDROID
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
#endif
    }
    #endregion

    #region Mono Beaviour



    public void Start()
    {
        username.text = PlayerData.Current.name;
        StartCoroutine(GameUtils.Instance._DownloadImage(PlayerData.Current.avatarUrl, avatar));

        if (ParseFacebookUtils.IsLinked(ParseUser.CurrentUser))
        {
            buttonLinkFacebook.SetActive(false);
        }
    } 
    #endregion

}
