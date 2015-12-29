using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExpController : Singleton<ExpController>
{
    public Slider slider;


    float tmpValue;
    float correctValue;
    float speed = 10;

    int currentLevel;
    int currentExp;
    int expToLevelUp;

	void Start ()
    {        
        currentLevel = PlayerData.Current.level;
        if (currentLevel >= 30)
            return;

        currentExp = PlayerData.Current.exp;
        expToLevelUp = GameModel.Instance.expLevelUpConfig[currentLevel];
	}
	
	void Update ()
    {
	    if (tmpValue < correctValue)
        {
            tmpValue += speed * Time.deltaTime;
            slider.value = tmpValue;
        }
	}

    public void SetExpBonus(int expBonus)
    {

    }
}
