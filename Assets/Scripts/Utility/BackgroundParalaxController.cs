using UnityEngine;
using System.Collections;

public class BackgroundParalaxController : MonoBehaviour
{
    private Camera cam;
    private float lastCamPosX;
    private float ratioSpeed = 0.8f;
    private float minDistanceToMove = 0.5f;
    private Vector3 correctPos;

    private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;


    void Start()
    {
        cam = Camera.main;
        lastCamPosX = cam.transform.position.x;
        correctPos = transform.position;
    }

    void Update()
    {
        float distanceX = cam.transform.position.x - lastCamPosX;
        if (distanceX >= minDistanceToMove)
        {
            correctPos += new Vector3(distanceX * ratioSpeed, 0, 0);
            lastCamPosX = cam.transform.position.x;           
        }        
        transform.position = Vector3.SmoothDamp(transform.position, correctPos, ref velocity, smoothTime);
    }  
}
