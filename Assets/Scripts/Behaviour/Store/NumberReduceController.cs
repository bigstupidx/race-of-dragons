using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumberReduceController : MonoBehaviour
{
    public Text text;
    [HideInInspector] public float tmpNumber;
    [HideInInspector] public float number;
    private float speed = 10;
	
	void Start ()
    {
	
	}
		
	void Update ()
    {
	    if (tmpNumber < number)
        {
            tmpNumber += speed;
        }
        if (tmpNumber > number)
        {
            tmpNumber -= speed;
        }

        text.text = (int)tmpNumber + "";
	}
}
