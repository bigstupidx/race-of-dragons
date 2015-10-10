using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private StateMachine<PlayerController> stateMachine;
    private float alpha;
    private Rigidbody2D body;

    public bool controlable = false;
    public float speedAngle = GameConsts.Instance.PLAYER_SPEED_ANGLE_DEFAULT;

    [HideInInspector]
    public Animator animator;

    #region Get & Set

    public StateMachine<PlayerController> GetStateMachine()
    {
        return stateMachine;
    }

    #endregion

    private float CaculateAlpha(float x, float y)
    {
        float result = Mathf.Atan2(y, x);
        result = GameUtils.Instance.RadianToDegree(result);

        return result;
    }

    void Awake ()
    {
        // create new state machine
        stateMachine = new StateMachine<PlayerController>(this, new PlayerWaitingState());

        // add all of states to state machine so that we can switch to them
        stateMachine.AddState(new PlayerFlyState());
        stateMachine.AddState(new PlayerFallState());

        stateMachine.ChangeState<PlayerWaitingState>();
        // listen for state changes
        stateMachine.onStateChanged += () =>
        {
            Debug.Log("state changed: " + stateMachine.CurrentState);
        };

        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
	}

    // Use this for initialization
    void Start()
    {

    }

	// Update is called once per frame
	void Update ()
    {
        if (!controlable)
            return;
        Camera.main.transform.position = new Vector3(transform.position.x, 0, Camera.main.transform.position.z);
        alpha = CaculateAlpha(body.velocity.x, body.velocity.y);
        transform.rotation = Quaternion.Euler(0, 0, alpha);

        stateMachine.Update(Time.deltaTime);
	}
}
