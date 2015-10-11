using UnityEngine;
using System.Collections;
using Facebook;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.MiniJSON;
using Parse;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour {
  
    public InputField username;
    public InputField password;
    public Text txtInfo;
    public GameObject loginPanel;
    public GameObject signUpPanel;
    
    void Awake()
    {
        FB.Init(delegate ()
       {
           Debug.Log("Init done!");
       });

        if (ParseUser.CurrentUser != null)
        {
            ParseUser.LogOut();
        }
        else
        {

        }
    }

    // Use this for initialization
    void Start() {

    }

    public void FacebookLogin()
    {
        FB.Login("user_about_me", delegate (FBResult result) 
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Login success");
                StartCoroutine("ParseLoginWithFacebook");
            }
            else
            {
                Debug.Log("FBLoginCallback: User canceled login");
            }
        });
    }

    private IEnumerator ParseLoginWithFacebook()
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

                    StartCoroutine("saveUserProfile", userProfile);

                    //StartCoroutine("UpdateProfilePictureTexture", userProfile["pictureURL"]);
                });

            }
        }
    }

    private IEnumerator UpdateProfilePictureTexture(string pictureURL)
    {
        string url = pictureURL + "&access_token=" + FB.AccessToken; ;
        WWW www = new WWW(url);
        yield return www;
        //avatar.texture = www.texture;
    }

    private IEnumerator saveUserProfile(Dictionary<string, string> profile)
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
        Task task = ParseUser.LogInAsync(username.text, password.text).ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
                Debug.Log(" The login failed. Check the error to see why.");
                txtInfo.text = GameConsts.Instance.STRING_LOGIN_FAIL;
            }
            else
            {
                Debug.Log("Login was successful.");                              
            }
        });
        StartCoroutine(ChangeScene(task));
    }

    private IEnumerator ChangeScene(Task t)
    {
        while (!t.IsCompleted) yield return null;
        Application.LoadLevel(GameConsts.Instance.LEVEL_WAITING_NAME);
    }

    public void ShowSignUpScreen()
    {
        if (ParseUser.CurrentUser != null)
        {
            ParseUser.LogOutAsync().ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    Debug.Log("Logout failed!");
                }
                else
                {
                    loginPanel.SetActive(false);
                    signUpPanel.SetActive(true);
                }
            });
        }
        else
        {
            loginPanel.SetActive(false);
            signUpPanel.SetActive(true);
        }
    }
}
