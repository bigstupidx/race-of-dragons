using UnityEngine;
using System.Collections;

public class BackgroundParalaxController : MonoBehaviour
{
    private Transform cam;
    private Vector3 previousCamPos;

    private float parallaxScale = -18f;
    private float smoothing = 1f;
    
    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;
    }

    void Update()
    {
        float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;
        float backgroundTargetPosX = transform.position.x + parallax;
        Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, backgroundTargetPos, smoothing * Time.deltaTime);

        previousCamPos = cam.position;
    }
}
