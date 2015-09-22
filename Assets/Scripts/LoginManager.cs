using UnityEngine;
using System.Collections;
using Facebook;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.MiniJSON;
using Parse;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour {

    public Text info;
    public Text username;
    public Text password;
    public RawImage avatar;
    
    void Awake()
    {
        FB.Init(delegate ()
       {
           Debug.Log("Init done!");
       });
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
                    info.text = userProfile["name"];

                    StartCoroutine("saveUserProfile", userProfile);

                    StartCoroutine("UpdateProfilePictureTexture", userProfile["pictureURL"]);
                });

            }
        }
    }

    private IEnumerator UpdateProfilePictureTexture(string pictureURL)
    {
        string url = pictureURL + "&access_token=" + FB.AccessToken; ;
        WWW www = new WWW(url);
        yield return www;
        avatar.texture = www.texture;
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
        ParseUser.LogInAsync(username.text, password.text).ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
                Debug.Log(" The login failed. Check the error to see why.");
            }
            else
            {
                Debug.Log("Login was successful.");                
            }
        });
    }

    public void ParseSignUp()
    {
        if (ParseUser.CurrentUser != null)
        {
            ParseUser.LogOut();
        }
        else
        {
            
        }

        if (username.text != "" && username.text != "Username" && password.text != "")
        {
            var user = new ParseUser()
            {
                Username = username.text,
                Password = password.text,
                Email = "test@abc.com"
            };

            Task signUpTask = user.SignUpAsync();
        }
        else
        {
            Debug.Log("");
        }
        Debug.Log("Sign Up Click: " + username.text);

    }
}
