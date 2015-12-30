using UnityEngine;
using System.Collections;

public class BackgroundParalaxController : MonoBehaviour
{
    private Transform cam;
    private Vector3 previousCamPos;

    private Vector3 correctPos;

    private float parallaxScale = -15f;
    private float smoothing = 1f;
    
    void Awake()
    {
        cam = Camera.main.transform;
        correctPos = transform.position;
    }

    void Start()
    {
        previousCamPos = cam.position;
    }

    void Update()
    {
        if (Vector3.Distance(cam.position, previousCamPos) > 2)
        {
            previousCamPos = cam.position;
            return;
        }

        float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;
        float backgroundTargetPosX = transform.position.x + parallax;
        Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.position.y, transform.position.z);
        if (Vector3.Distance(backgroundTargetPos, transform.position) > 1)
        {
            correctPos = backgroundTargetPos;
            previousCamPos = cam.position;
        }
        transform.position = Vector3.Lerp(transform.position, correctPos, smoothing * Time.deltaTime);

    }
}
