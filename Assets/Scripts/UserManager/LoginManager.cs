using UnityEngine;
using System.Collections;
using Facebook;
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
    
    void Awake()
    {
        FB.Init(delegate ()
       {
           Debug.Log("Init done!");
       });
    }
    
    void Start()
    {
        ParseUser.LogOutAsync();
    }

    public void FacebookLogin()
    {
        FB.Login("user_about_me,user_friends", delegate (FBResult result) 
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
            // Logging in with Parse
            var loginTask = ParseFacebookUtils.LogInAsync(FB.UserId,
                                                          FB.AccessToken,
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
                FB.API("/me", HttpMethod.GET, delegate (FBResult result)
                {
                    var resultObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
                    var userProfile = new Dictionary<string, string>();

                    userProfile["facebookId"] = getDataValueForKey(resultObject, "id");
                    userProfile["name"] = getDataValueForKey(resultObject, "name");
                    if (userProfile["facebookId"] != "")
                    {
                        userProfile["pictureURL"] = "https://graph.facebook.com/" + userProfile["facebookId"] + "/picture?type=large&return_ssl_resources=1";
                    }                  

                    StartCoroutine(_SaveUserProfile(userProfile));
                });

            }
        }
    }

    private IEnumerator _UpdateProfilePictureTexture(string pictureURL)
    {
        string url = pictureURL + "&access_token=" + FB.AccessToken; ;
        WWW www = new WWW(url);
        yield return www;
        //avatar.texture = www.texture;
    }

    private IEnumerator _SaveUserProfile(Dictionary<string, string> profile)
    {
        var user = ParseUser.CurrentUser;
        user["profile"] = profile;
        // Save if there have been any updates
        if (user.IsKeyDirty("profile"))
        {
            var saveTask = user.SaveAsync();
            while (!saveTask.IsCompleted) yield return null;            
        }
        Debug.Log("Login was successful.");
    }


    private string getDataValueForKey(Dictionary<string, object> dict, string key)
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

    public void ShowSignUpScreen()
    {
        Instantiate(signUpDialogPrefab);
    }
}
