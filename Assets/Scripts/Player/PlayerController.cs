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

public enum PlayerState
{
    Flying,
    Failing,
    Running,
    Burning,
    Slowing,
    FallInWater,
    FinishRaceState
}

[Serializable]
public class DragonPropertie
{
    public int level = 1;     
    public Element element;    

    public DragonPropertie()
    {
        element = (Element)(UnityEngine.Random.Range(0, 3));        
    }

    public DragonPropertie(Element ele)
    {
        element = ele;        
    }

    public void Load()
    {
        if (PlayerData.Current.dragons.ContainsKey(element.ToString()))
        {
            level = PlayerData.Current.dragons[element.ToString()].level;
        }
    }

    public float TimeCooldown
    {
        get
        {
            return GameModel.Instance.dragonLevelConfig[element][level];
        }
    }

    public DragonPropertie(IDictionary<string, object> data)
    {        
        level = int.Parse(data["level"].ToString());       
        element = (Element)(int.Parse(data["element"].ToString()));
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result.Add("level", level);      
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
    [HideInInspector] public TextMesh textName;

    public float speedAngle = GameConsts.Instance.PLAYER_SPEED_ANGLE_DEFAULT;
    public float maxSpeedAngle = 20;

    [Space(10)]
    public DragonPropertie dragonPropertie;

    [Header("Skills")]
    public Transform skillPlaceHolder;
    [HideInInspector] public SkillController skillController;
    public Sprite imageOfSkill;
    [HideInInspector] public bool hasShield;
    [HideInInspector] public PlayerState playerState;
    [HideInInspector] public Vector3 safePos;
    [HideInInspector] public Vector3 victoryPos = Vector3.zero;

    private bool isSlow;
    private float timer;
    [HideInInspector] public bool isFinish;

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

    public int Result { get; set; }

    public string Name
    {
        get
        {
            return GameUtils.GetCustomProperty<string>(photonView, "NAME", "");
        }
        set
        {
            GameUtils.SetCustomProperty<string>(photonView, "NAME", value);
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
        stateMachine = new StateMachine<PlayerController>(this, new PlayerInitState());

        // add all of states to state machine so that we can switch to them
        stateMachine.AddState(new PlayerWaitingState());
        stateMachine.AddState(new PlayerFlyState());
        stateMachine.AddState(new PlayerFallState());
        stateMachine.AddState(new PlayerRunState());
        stateMachine.AddState(new PlayerBurningState());
        stateMachine.AddState(new PlayerThunderBoostState());
        stateMachine.AddState(new PlayerFallInWaterState());
        stateMachine.AddState(new PlayerFinishRaceState());

        stateMachine.ChangeState<PlayerWaitingState>();
        // listen for state changes
        stateMachine.onStateChanged += () =>
        {
            //Debug.Log("state changed: " + stateMachine.CurrentState);
        };

        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        textName = GetComponentInChildren<TextMesh>();
        dragonPropertie.Load();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!controlable)
            return;

        // check victory
        if (victoryPos != Vector3.zero)
        {
            if (transform.position.x > victoryPos.x && isFinish == false)
            {
                isFinish = true;
                List<Dictionary<int, string>> result = GameUtils.GetRoomCustomProperty<List<Dictionary<int, string>>>("RESULT", null);

                if (result == null)
                    result = new List<Dictionary<int, string>>();

                GameTimeController.Instance.isStart = false;
                string timeFinish = GameTimeController.Instance.text.text;
                var dict = new Dictionary<int, string>();
                dict.Add(photonView.ownerId, timeFinish);
                result.Add(dict);
                Result = result.Count + 1;
                GameUtils.SetRoomCustomProperty<List<Dictionary<int, string>>>("RESULT", result);
                
                stateMachine.ChangeState<PlayerFinishRaceState>();
            }
        }

        if (isSlow)
            Time.timeScale = 0.5f;
        else
            Time.timeScale = 1.0f;

        // test skill
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserSkill();
        }

        if (playerState != PlayerState.Running)
        {
            alpha = CalculateAlpha(body.velocity.x, body.velocity.y);
            transform.rotation = Quaternion.Euler(0, 0, alpha);
        }
        
        stateMachine.Update(Time.deltaTime);

        speedAngle = Mathf.Min(speedAngle, maxSpeedAngle);

        //PosX = transform.position.x;

        timer += Time.deltaTime;
        if (timer >= 2 && playerState == PlayerState.Flying && transform.position.y > 0)
        {
            timer = 0;
            safePos = transform.position;
        }

        if (transform.position.y < -10)
        {
            ResetPosition();
        }

        if (PhotonNetwork.offlineMode == true)
        {
            PosX = transform.position.x;
        }
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
            if (iceAge.ID != PlayerId && hasShield == false)
                isSlow = true;
        }
        else if (collision.tag.Equals("Water"))
        {
            stateMachine.ChangeState<PlayerFallInWaterState>();
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

    internal void UserItem(Item currentItem)
    {
        Debug.Log("Use: " + currentItem.ToString());
        if (currentItem == Item.Shield)
        {
            GameObject shield = PhotonNetwork.Instantiate("BubbleShield", transform.position, Quaternion.identity, 0) as GameObject;            
            hasShield = true;
        }

        if (currentItem == Item.Energy)
        {
            if (skillController != null)
            {
                float duration = PlayerData.Current.items[currentItem.ToString()].GetDuration();
                skillController.ReduceTimeCoolDown(duration); // defaul is 5 sec
            }
        }

        if (currentItem == Item.SpeedUp)
        {
            speedAngle *= 1.5f;
        }

        if (currentItem == Item.Rocket)
        {
            GameObject rocket = PhotonNetwork.Instantiate("Rocket", skillPlaceHolder.position, skillPlaceHolder.rotation, 0) as GameObject;
        }
    }

    public void ResetPosition()
    {
        transform.position = safePos;
        stateMachine.ChangeState<PlayerWaitingState>();
    }
}
