using UnityEngine;
using System.Collections;
using Facebook.Unity;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.MiniJSON;
using Parse;
using System.Threading.Tasks;
using System;

public class LoginManager : MonoBehaviour {
  
    public InputField username;
    public InputField password;
    public Text txtInfo;
    public GameObject signUpDialogPrefab;
    public GameObject loadingDialogPrefab;

    private LoadingDialogBehaviour loadingDialogBehaviour;
    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();

        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }
    }    

    void Start()
    {
        
    }

    public void FacebookLogin()
    {
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, delegate (ILoginResult result)
        {            
            if (FB.IsLoggedIn)
            {
                Debug.Log("Login success");
                StartCoroutine(_ParseLoginWithFacebook());
            }
            else
            {
                Debug.Log("FBLoginCallback: User canceled login");
            }
        });       
    }

    private IEnumerator _ParseLoginWithFacebook()
    {
        if (FB.IsLoggedIn)
        {
            var aToken = AccessToken.CurrentAccessToken;
            // Logging in with Parse
            var loginTask = ParseFacebookUtils.LogInAsync(aToken.UserId,
                                                          aToken.TokenString,
                                                          System.DateTime.Now);
            while (!loginTask.IsCompleted) yield return null;
            // Login completed, check results
            if (loginTask.IsFaulted || loginTask.IsCanceled)
            {
                // There was an error logging in to Parse
                foreach (var e in loginTask.Exception.InnerExceptions)
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
                FB.API("/me", HttpMethod.GET, delegate (IGraphResult result)
                {
                    var resultObject = result.ResultDictionary;
                    var userProfile = new Dictionary<string, string>();

                    userProfile["facebookId"] = resultObject["id"].ToString();
                    userProfile["name"] = resultObject["name"].ToString();                    
                    userProfile["avatarUrl"] = "https://graph.facebook.com/" + userProfile["facebookId"] + "/picture?type=large&return_ssl_resources=1";
                    

                    StartCoroutine(_SaveUserProfile(userProfile));
                });

            }
        }
    }

    private IEnumerator _SaveUserProfile(Dictionary<string, string> profile)
    {
        var param = new Dictionary<string, object>();
        param.Add("data", profile);

        PlayerData.Current.name = profile["name"];

        var taskSync = ParseCloud.CallFunctionAsync<ParseObject>("updateData", param).ContinueWith(t2 =>
        {
            
        });

        while (!taskSync.IsCompleted) yield return null;

        Debug.Log("Update data was successful.");


        GameObject loadingDialog = Instantiate(loadingDialogPrefab) as GameObject;
        loadingDialogBehaviour = loadingDialog.GetComponent<LoadingDialogBehaviour>();
        Dictionary<string, IEnumerator> listToDo = new Dictionary<string, IEnumerator>();
        
        listToDo.Add("Sync data ...", _SyncData());
        listToDo.Add("Get friends list ...", _GetFriendList());
        listToDo.Add("Done!...", _PrepareSomeThing());

        loadingDialogBehaviour.SetUpToDoList(listToDo);
    }


    private string getDataValueForKey(IDictionary<string, object> dict, string key)
    {
        object objectForKey;
        if (dict.TryGetValue(key, out objectForKey))
        {
            return (string)objectForKey;
        }
        else
        {
            return "";
        }
    }

    public void ParseLogin()
    {
        GameObject loadingDialog = Instantiate(loadingDialogPrefab) as GameObject;
        loadingDialogBehaviour = loadingDialog.GetComponent<LoadingDialogBehaviour>();
        Dictionary<string, IEnumerator> listToDo = new Dictionary<string, IEnumerator>();
        listToDo.Add("Checking server...", _ConfirmLogin());
        listToDo.Add("Sync data ...", _SyncData());
        listToDo.Add("Get friends list ...", _GetFriendList());
        listToDo.Add("Done!...", _PrepareSomeThing());

        loadingDialogBehaviour.SetUpToDoList(listToDo);
    }

    private IEnumerator _ConfirmLogin()
    {
        Task loginTask = ParseUser.LogInAsync(username.text, password.text).ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
                Debug.Log(" The login failed. Check the error to see why.");
                //txtInfo.text = GameConsts.Instance.STRING_LOGIN_FAIL;
            }
            else
            {
                Debug.Log("Login was successful.");
            }
        });

        while (!loginTask.IsCompleted) yield return null;

        // Login completed, check results
        if (ParseUser.CurrentUser == null)
        {
            txtInfo.text = GameConsts.Instance.STRING_LOGIN_FAIL;
            loadingDialogBehaviour.ForceStopLoading();
        }
    }

    private IEnumerator _SyncData()
    {
        if (ParseUser.CurrentUser != null)
        {
            PlayerData.Current.id = ParseUser.CurrentUser.ObjectId;
            if (string.IsNullOrEmpty(PlayerData.Current.name))
                PlayerData.Current.name = ParseUser.CurrentUser.Username;
            PlayerData.Current.Save();

            var param = new Dictionary<string, object>();
            param.Add("data", PlayerData.Current.ToDictionary());

            var taskSync = ParseCloud.CallFunctionAsync<ParseObject>("mergeData", param).ContinueWith(t2 =>
            {
                if (t2.IsCompleted)
                {
                    var newData = t2.Result;
                    PlayerData.Current.SyncData(newData);
                }
            });

            while (!taskSync.IsCompleted) yield return null;

            if (!taskSync.IsFaulted && !taskSync.IsCanceled)
            {
                PlayerData.Current.Save();
            }
        }
    }

    private IEnumerator _GetFriendList()
    {
        if (ParseUser.CurrentUser != null)
        {
            var taskSync = ParseCloud.CallFunctionAsync<IList<IDictionary<string, object>>>("getFriendList", null).ContinueWith(t2 =>
            {
                if (t2.IsCompleted)
                {
                    var friendList = t2.Result;
                    PlayerData.Current.SetFriendList(friendList);
                }
            });

            while (!taskSync.IsCompleted) yield return null;

            if (!taskSync.IsFaulted && !taskSync.IsCanceled)
            {
                PlayerData.Current.Save();
            }
        }
    }

    private IEnumerator _PrepareSomeThing()
    {
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isDisappear", true);
    }

    public void ShowSignUpScreen()
    {
        Instantiate(signUpDialogPrefab);
    }
}
