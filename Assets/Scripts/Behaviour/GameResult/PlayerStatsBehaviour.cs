using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatsBehaviour : MonoBehaviour
{
    public Image avatar;
    public Slider slider;
    public Text level;

    int currentLevel;
    int currentExp;
    int expToLevelUp;

    float tmpValue;
    float correctValue;
    float speed = 10;

    void Awake()
    {
        string avatarUrl = PlayerData.Current.avatarUrl;
        StartCoroutine(GameUtils.Instance._DownloadImage(avatarUrl, avatar));

        currentLevel = PlayerData.Current.level;
        level.text = "Lv " + currentLevel.ToString();

        currentExp = PlayerData.Current.exp;
        expToLevelUp = GameModel.Instance.expLevelUpConfig[currentLevel];

        slider.maxValue = expToLevelUp;
        speed = expToLevelUp / 10;
        slider.value = currentExp;
        tmpValue = currentExp;
    }

    void Start()
    {

    }

	void Update ()
    {
        if (tmpValue < correctValue)
        {
            tmpValue += speed * Time.deltaTime;
            slider.value = tmpValue;

            if (tmpValue >= expToLevelUp)
            {
                correctValue -= expToLevelUp;
                level.text = "Lv " + currentLevel.ToString();
                expToLevelUp = GameModel.Instance.expLevelUpConfig[currentLevel];

                slider.maxValue = expToLevelUp;
                speed = expToLevelUp / 10;
                slider.value = 0;
                tmpValue = 0;                
            }
        }
    }

    public void SetExpBonus(int expBonus)
    {
        correctValue = currentExp + expBonus;

        if (correctValue >= expToLevelUp)
        {
            currentLevel++;
            PlayerData.Current.level = currentLevel;
            PlayerData.Current.exp = (int)correctValue - expToLevelUp;
            PlayerData.Current.Save();
            PlayerData.Current.SaveOnServer();
        }
        else
        {
            PlayerData.Current.exp = (int)correctValue;
            PlayerData.Current.Save();
            PlayerData.Current.SaveOnServer();
        }
    }
}
