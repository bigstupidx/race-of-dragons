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
    private bool isStartCountDown;

	// Use this for initialization
	void Start ()
    {
        text.text = "Ready!";
        startTime = PhotonNetwork.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isStartCountDown)
        {
            var listPlayer = GameObject.FindGameObjectsWithTag("Player");
            if (listPlayer.Length >= PhotonNetwork.room.playerCount)
            {
                isStartCountDown = true;
                startTime = PhotonNetwork.time;
            }
        }
        else
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
}
