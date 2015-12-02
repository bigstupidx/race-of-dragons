using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ParseHelper : Singleton<ParseHelper>
{
	
	void Start ()
    {
        //var param = new Dictionary<string, object>();
        //param.Add("userId", "U3sFSPkORb");
        //ParseCloud.CallFunctionAsync<int>("getRankOfUser", param).ContinueWith(t =>
        //{
        //    Debug.Log(t.Result);
        //});

        
        //ParseUser.LogInAsync("thienlt", "123456").ContinueWith(t =>
        //{
        //    if (t.IsFaulted || t.IsCanceled)
        //    {
        //        Debug.Log("The login failed. Check the error to see why.");
        //    }
        //    else
        //    {
        //        Debug.Log("Login was successful.");

        //        var test2 = new Dictionary<string, object>();
        //        test2.Add("level", 209);
        //        test2.Add("exp", 2);
        //        var data = new Dictionary<string, object>();
        //        data.Add("data", test2);
        //        ParseCloud.CallFunctionAsync<ParseObject>("mergeData", data).ContinueWith(t3 =>
        //        {
        //            var newData = t3.Result;
        //            Debug.Log(newData.Get<int>("level"));
        //        });
        //    }
        //});        



        // test RestAPI helper
        //RestFactory.Instance.GET("http://jsonplaceholder.typicode.com/posts", (s) =>
        //{
        //    Debug.Log(s);
        //}, null);
    }
	
}
