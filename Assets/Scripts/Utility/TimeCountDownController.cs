using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeCountDownController : MonoBehaviour
{
    public Text text;
    public InGameNetworkManager gameController;
    public GameTimeController gameTime;
    private float timer;
    private double startTime;

	// Use this for initialization
	void Start ()
    {
        text.text = "Ready!";
        startTime = PhotonNetwork.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer = (float)(PhotonNetwork.time - startTime);
        if (timer > 1)
        {
            text.text = (int)(5 - timer) + "";
        }

        if (timer > 4)
        {
            text.text = "Go";
        }

        if (timer > 4.5f)
        {
            text.text = "";
            gameObject.SetActive(false);
            gameController.playerController.controlable = true;
            gameTime.SetTimeStart();
        }       
	}
}
