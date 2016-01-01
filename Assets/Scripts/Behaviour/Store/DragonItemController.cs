using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public class DragonInfo
{
    public Element element;
    public string Name
    {
        get
        {
            return (string)GameModel.Instance.shopDragonConfig[element]["NAME"];
        }
    }
    public string Info
    {
        get
        {
            return (string)GameModel.Instance.shopDragonConfig[element]["DES"];
        }
    }

    public int Level { get; set; }  
    public bool IsLocked
    {
        get
        {
            return Level == 0;
        }
    }

    public bool useGem;
    public int Price
    {
        get
        {
            if (Level < 5)
                return (int)GameModel.Instance.shopDragonConfig[element]["LEVEL" + (Level + 1)];
            else
                return 9999;
        }
    }

    public void Load()
    {
        if (PlayerData.Current.dragons.ContainsKey(element.ToString()))
        {
            Level = PlayerData.Current.dragons[element.ToString()].level;
        }
    }
    public void Save()
    {
        if (PlayerData.Current.dragons.ContainsKey(element.ToString()))
        {
            PlayerData.Current.dragons[element.ToString()].level = Level;
            PlayerData.Current.Save();
        }
        else
        {
            DragonPropertie newDragon = new DragonPropertie(element);
            PlayerData.Current.dragons.Add(element.ToString(), newDragon);
            PlayerData.Current.Save();
        }
    }
}

public class DragonItemController : MonoBehaviour
{
    public DragonInfo itemInfo;

    [Header("Objects")]
    public Text name;
    public GameObject buyByCoin;
    public GameObject buyByGem;
    public GameObject upgradeByCoin;
    public GameObject upgradeByGem;

    public GameObject btnBuy;
    public GameObject levelFill;
    public GameObject selected;

    private Text price;
    private Button btnBuyScript;   

    void Start ()
    {
        itemInfo.Load();
        name.text = itemInfo.Name;        

        Reload();
	}    
		
	void Update ()
    {
	
	}

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            levelFill.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Reload()
    {
        if (btnBuyScript != null)
            btnBuyScript.onClick.RemoveAllListeners();

        buyByCoin.SetActive(false);
        buyByGem.SetActive(false);
        upgradeByCoin.SetActive(false);
        upgradeByGem.SetActive(false);

        if (itemInfo.IsLocked)
        {
            if (itemInfo.useGem)
            {
                buyByGem.SetActive(true);
            }
            else
            {
                buyByCoin.SetActive(true);
            }
        }
        else
        {
            if (itemInfo.useGem)
            {
                upgradeByGem.SetActive(true);
            }
            else
            {
                upgradeByCoin.SetActive(true);
            }
        }

        SetLevel(itemInfo.Level);

        price = btnBuy.GetComponentInChildren<Text>();
        price.text = itemInfo.Price + "";

        if (PlayerData.Current.CurrentDragon.element == itemInfo.element)
        {
            selected.SetActive(true);
        }
        else
        {
            selected.SetActive(false);
        }

        btnBuyScript = btnBuy.GetComponentInChildren<Button>();
        btnBuyScript.onClick.AddListener(OnBuyItem);
    }

    public void OnBuyItem()
    {        
        if (itemInfo.Level == 5)
            return;

        SoundManager.Instance.playButtonSound();

        if (itemInfo.useGem)
        {
            if (PlayerData.Current.gem >= itemInfo.Price)
            {
                PlayerData.Current.gem -= itemInfo.Price;
                GemController.Instance.SetGems(PlayerData.Current.gem);
                int newLevel = itemInfo.Level + 1;
                itemInfo.Level = newLevel;
                itemInfo.Save();
                Reload();
            }
            else // not enough gems
            {
                WarningDialogController.Instance.ShowNotEnoughGems();
            }
        }
        else
        {
            if (PlayerData.Current.gold >= itemInfo.Price)
            {
                PlayerData.Current.gold -= itemInfo.Price;
                CoinController.Instance.SetCoins(PlayerData.Current.gold);
                int newLevel = itemInfo.Level + 1;
                itemInfo.Level = newLevel;
                itemInfo.Save();
                Reload();
            }
            else // not enough coins
            {
                WarningDialogController.Instance.ShowNotEnoughCoins();              
            }
        }
    }

    public void OnSelected()
    {
        if (itemInfo.IsLocked == false)
        {
            SoundManager.Instance.playButtonSound();
            PlayerData.Current.currentDragonIndex = itemInfo.element.ToString();
            PlayerData.Current.Save();
            Reload();
        }
    }
}
