using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{
    [Header("Target")]
    public GameObject target;
    
    [Header("Camera Properties")]
    public float smoothTime = 0.2f;
    public float speed;
    public float speedY;
    public Vector2 velocity;
    public float minSize = 3.6f;
    public float maxSize = 7.0f;
    public float maxY = 19.0f;

    private float x, y;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {                
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, CalculateSize(), ref speed, smoothTime);
        y = CalculateY();
        x = CalculateX();
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private float CalculateSize()
    {
        float size = 5;

        size = minSize + target.transform.position.y / minSize;
        size = Mathf.Clamp(size, minSize, maxSize);
        return size;
    }

    private float CalculateY()
    {
        float yPos = 0;

        yPos = target.transform.position.y - minSize;
        yPos = Mathf.Clamp(yPos, 0, maxY);
        return yPos;
    }

    private float CalculateX()
    {
        float xPos = 0;

        xPos = target.transform.position.x;
        xPos = Mathf.Max(xPos, cam.orthographicSize - minSize);
        return xPos;
    }
}
