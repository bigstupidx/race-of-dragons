using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ParseHelper : Singleton<ParseHelper>
{
	
	void Start ()
    {
        // test Parse Cloud Code
        var test = new Dictionary<string, object>();
        test.Add("a", 1);
        test.Add("b", 2);

        ParseCloud.CallFunctionAsync<string>("hello", test).ContinueWith(t =>
        {
            Debug.Log("HAIZ" + t.Result);
        });

        // test RestAPI helper
        RestFactory.Instance.GET("http://jsonplaceholder.typicode.com/posts", (s) =>
        {
            Debug.Log(s);
        }, null);
	}
	
}
