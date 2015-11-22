using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Parse;
using System.Threading.Tasks;
using System.Collections.Generic;

public class SignUpManager : MonoBehaviour {

    public InputField username;
    public InputField password;
    public InputField email;
    public Text txtInfo;
    public GameObject loginPanel;
    public GameObject signUpPanel;

    public void GoBack()
    {
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    private IEnumerator ShowLoginScreen(Task task)
    {
        while (!task.IsCompleted) yield return null;
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);       
    }


    public void ParseSignUp()
    {
        if (ParseUser.CurrentUser != null)
        {
            Debug.Log("Parse has been logined!");
        }
        else
        {
            if (username.text != "" && username.text != "Username" && password.text != "")
            {
                ParseUser user;

                if (email.text != "" && email.text != "email")
                {
                    user = new ParseUser()
                    {
                        Username = username.text,
                        Password = password.text,
                        Email = email.text
                    };                   
                }
                else
                {
                    user = new ParseUser()
                    {
                        Username = username.text,
                        Password = password.text
                    };
                }

                Task signUpTask = user.SignUpAsync().ContinueWith(t =>
                {
                    if (t.IsCompleted)
                    {
                        Debug.Log("Sign Up success!");
                    }
                    else
                    {                        
                        Debug.Log("Sign up failed!");

                        // Errors from Parse Cloud and network interactions
                        using (IEnumerator<System.Exception> enumerator = t.Exception.InnerExceptions.GetEnumerator())
                        {
                            if (enumerator.MoveNext())
                            {
                                ParseException error = (ParseException)enumerator.Current;
                                // error.Message will contain an error message
                                // error.Code will return "OtherCause"
                            }
                        }
                    }
                });
               
                StartCoroutine(ShowLoginScreen(signUpTask));
            }
        }

    }
}
