using UnityEngine;
using System;
using System.Collections;

public enum Element
{
    Fire = 0,
    Ice,
    Thunder
}

[Serializable]
public class DragonPropertie
{
    public int level = 1;
    public int energy = 100;    
    public int exp;
    public Element element = Element.Fire;
    public float timeCooldown = 30;
    public int speedRecoverEnergy = 3; // per second
}

public class PlayerController : MonoBehaviour
{
    private StateMachine<PlayerController> stateMachine;
    private float alpha;

    [HideInInspector]
    public bool controlable = false;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public Rigidbody2D body;

    public float speedAngle = GameConsts.Instance.PLAYER_SPEED_ANGLE_DEFAULT;

    [Space(10)]
    public DragonPropertie dragonPropertie;    

    #region Get & Set

    public StateMachine<PlayerController> GetStateMachine()
    {
        return stateMachine;
    }

    #endregion

    private float CalculateAlpha(float x, float y)
    {
        float result = Mathf.Atan2(y, x);
        result = GameUtils.Instance.RadianToDegree(result);

        return result;
    }

    public void Init()
    {
        GameObject.Destroy(transform.GetChild(0).gameObject);
        GameObject dragon = Instantiate(Resources.Load("Prefabs/Dragon/" + dragonPropertie.element.ToString())) as GameObject;
        dragon.transform.parent = transform;
    }

    #region MonoBehaviour
    void Awake()
    {
        // create new state machine
        stateMachine = new StateMachine<PlayerController>(this, new PlayerWaitingState());

        // add all of states to state machine so that we can switch to them
        stateMachine.AddState(new PlayerFlyState());
        stateMachine.AddState(new PlayerFallState());
        stateMachine.AddState(new PlayerRunState());

        stateMachine.ChangeState<PlayerWaitingState>();
        // listen for state changes
        stateMachine.onStateChanged += () =>
        {
            Debug.Log("state changed: " + stateMachine.CurrentState);
        };

        Init();

        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!controlable)
            return;

        alpha = CalculateAlpha(body.velocity.x, body.velocity.y);
        transform.rotation = Quaternion.Euler(0, 0, alpha);

        stateMachine.Update(Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Platform"))
        {
            stateMachine.ChangeState<PlayerRunState>();
        }
    } 

    #endregion


}
