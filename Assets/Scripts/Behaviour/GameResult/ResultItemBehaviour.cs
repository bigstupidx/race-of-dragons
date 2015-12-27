using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultItemBehaviour : MonoBehaviour
{
    public Text textName;
    public Text textTime;
    public Image avatar;
	
	void Start ()
    {
	
	}
		
    public void SetText(string name, string time)
    {
        this.textName.text = name;
        this.textTime.text = time;
    }	
}
