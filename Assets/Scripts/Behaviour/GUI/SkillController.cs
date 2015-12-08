using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillController : MonoBehaviour
{
    public float timeCoolDown = 30;
    public Text textTime;
    public Image mask;    
    public GameTimeController gameTime;
    [HideInInspector] public bool canUse;
    [HideInInspector] public PlayerController player;

    private float timer;
    private Button button;

	// Use this for initialization
	void Start ()
    {
        button = GetComponent<Button>();
        button.enabled = false;       
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameTime.isStart)
        {
            timer += Time.deltaTime;
            if (timer >= timeCoolDown)
            {
                mask.enabled = false;
                canUse = true;
                textTime.text = "";
                button.enabled = true;
            }
            else
            {
                textTime.text = (int)(timeCoolDown - timer) + "s";
            }
        }
        else
        {
            textTime.text = (int)(timeCoolDown - timer) + "s";
        }
    }

    public void Reset()
    {
        timer = 0;
        button.enabled = false;
        canUse = false;
        mask.enabled = true;
    }

    public void OnUseSkill()
    {
        player.UserSkill();
    }

    public void ReduceTimeCoolDown(float time)
    {
        timer += time;
    }
}
