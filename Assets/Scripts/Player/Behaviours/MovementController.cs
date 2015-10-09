using UnityEngine;
using System.Collections;

public enum PLayerState
{
    Flying,
    Falling,
    Die
}
public class MovementController : MonoBehaviour {

    #region Variable    

    public Sprite spirte;
    
    public float speed;
    public float alpha;
    public float speedAlpha;

    public Vector2 vel;

    private Rigidbody2D body;
    private PLayerState state;

    #endregion

    public void onTouch()
    {
        
    }

    private Vector2 CaculateForce(float speed, float a)
    {
        a = GameUtils.Instance.DegreeToRadian(a);
        Vector2 result = new Vector2();
        result.x = speed * Mathf.Cos(a);
        result.y = speed * Mathf.Sin(a);

        Debug.Log(result);
        return result;
    }

    private Vector2 CaculateNForce(float speed, float x, float y)
    {
        float a = CaculateAlpha(x, y);
        
        Vector2 result = CaculateForce(speed, a + 180);
        return result;
    }

    private float CaculateAlpha(float x, float y)
    {
        float result = Mathf.Atan2(y, x);
        result = GameUtils.Instance.RadianToDegree(result);

        return result;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start ()
    {
        body.velocity = new Vector2(2, 0);
        //Run.After(4, () => {
        //    Debug.Log("4 Seconds later");
        //});
        //Debug.Log("See you in 4 seconds");
    }

	
	// Update is called once per frame
	void Update ()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, 0, Camera.main.transform.position.z);
        alpha = CaculateAlpha(body.velocity.x, body.velocity.y);
        transform.rotation = Quaternion.Euler(0, 0, alpha);

        if (Input.GetMouseButton(0))
        {
            state = PLayerState.Flying;
        }
        else
        {
            state = PLayerState.Falling;
        }

        if (Input.GetMouseButton(1))
        {
            vel = CaculateForce(3, alpha + 30);
            body.velocity = body.velocity + new Vector2(3 * Time.deltaTime, 0);
        }
        else
        {

        }

        if (state == PLayerState.Falling)
        {
            body.gravityScale = 1;
        }
        else if (state == PLayerState.Flying)
        {
            body.gravityScale = -1;
        }
    }
}
