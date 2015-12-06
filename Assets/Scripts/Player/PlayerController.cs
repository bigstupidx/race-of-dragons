using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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

    public DragonPropertie()
    {

    }

    public DragonPropertie(IDictionary<string, object> data)
    {        
        level = int.Parse(data["level"].ToString());
        energy = int.Parse(data["energy"].ToString());
        exp = int.Parse(data["exp"].ToString());
        element = (Element)(int.Parse(data["element"].ToString()));
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result.Add("level", level);
        result.Add("energy", energy);
        result.Add("exp", exp);
        result.Add("element", (int)element);

        return result;
    }

}

public class PlayerController : Photon.PunBehaviour
{
    private StateMachine<PlayerController> stateMachine;
    private float alpha;

    //[HideInInspector]
    public bool controlable = false;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D body;

    public float speedAngle = GameConsts.Instance.PLAYER_SPEED_ANGLE_DEFAULT;
    public float maxSpeedAngle = 20;

    [Space(10)]
    public DragonPropertie dragonPropertie;

    [Header("Skills")]
    public Transform skillPlaceHolder;
    public bool isSlow;
    public Sprite imageOfSkill;

    #region Get & Set

    public StateMachine<PlayerController> GetStateMachine()
    {
        return stateMachine;
    }

    public int PlayerId
    {
        get
        {
            return photonView.ownerId;
        }
    }

    // PositionX in server not client
    public float PosX
    {
        get
        {
            return GameUtils.GetCustomProperty<float>(photonView, "POS_X", 0.0f);
        }
        set
        {
            GameUtils.SetCustomProperty<float>(photonView, "POS_X", value);
        }
    }
    #endregion

    private float CalculateAlpha(float x, float y)
    {
        float result = Mathf.Atan2(y, x);
        result = GameUtils.Instance.RadianToDegree(result);

        return result;
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
        stateMachine.AddState(new PlayerBurningState());
        stateMachine.AddState(new PlayerThunderBoostState());

        stateMachine.ChangeState<PlayerWaitingState>();
        // listen for state changes
        stateMachine.onStateChanged += () =>
        {
            //Debug.Log("state changed: " + stateMachine.CurrentState);
        };

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

        if (isSlow)
            Time.timeScale = 0.5f;
        else
            Time.timeScale = 1.0f;
        // test skill
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserSkill();
        }

        alpha = CalculateAlpha(body.velocity.x, body.velocity.y);
        transform.rotation = Quaternion.Euler(0, 0, alpha);

        stateMachine.Update(Time.deltaTime);

        speedAngle = Mathf.Min(speedAngle, maxSpeedAngle);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Platform"))
        {
            stateMachine.ChangeState<PlayerRunState>();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("IceAge"))
        {
            IceAgeController iceAge = collision.transform.parent.GetComponent<IceAgeController>();
            if (iceAge.ID != PlayerId)
                isSlow = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("IceAge"))
        {
            IceAgeController iceAge = collision.transform.parent.GetComponent<IceAgeController>();
            if (iceAge.ID != PlayerId)
                isSlow = false;
        }
    }

    #endregion


    public void UserSkill()
    {
        if (dragonPropertie.element == Element.Fire)
        {
            GameObject fireball = PhotonNetwork.Instantiate("Fireball", skillPlaceHolder.position, skillPlaceHolder.rotation, 0) as GameObject;
            //FireballController fireballController = fireball.GetComponent<FireballController>();            
        }

        if (dragonPropertie.element == Element.Thunder)
        {
            GameObject thunderBoost = PhotonNetwork.Instantiate("ThunderBoost", transform.position, Quaternion.identity, 0) as GameObject;

            ThunderBoostController thunderController = thunderBoost.GetComponent<ThunderBoostController>();

            stateMachine.ChangeState<PlayerThunderBoostState>();
        }

        if (dragonPropertie.element == Element.Ice)
        {
            GameObject iceAge = PhotonNetwork.Instantiate("IceAge", transform.position, Quaternion.identity, 0) as GameObject;

            IceAgeController iceAgeController = iceAge.GetComponent<IceAgeController>();
            iceAgeController.timeExist = 15;
        }
    }
}
