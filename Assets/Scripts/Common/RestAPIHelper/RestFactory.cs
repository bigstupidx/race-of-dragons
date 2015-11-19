using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RestFactory : Singleton<RestFactory>
{

	public WWW GET(string url, Action<string> onComplete, Action onFail)
    {
        WWW www = new WWW(url);

        StartCoroutine(WaitForRequest(www, onComplete, onFail));
        return www;
    }

    public WWW POST(string url, Dictionary<string, string> post, Action<string> onComplete, Action onFail)
    {
        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }

        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www, onComplete, onFail));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www, Action<string> onComplete, Action onFail)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            string result = www.text;
            if (onComplete != null)
                onComplete(result);
        }
        else
        {
            Debug.Log(www.error);
            if (onFail != null)
                onFail();
        }
    }
}
