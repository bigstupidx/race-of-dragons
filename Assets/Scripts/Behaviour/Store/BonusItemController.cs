using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class BonusInfo
{
    public Item item;
    public string Name
    {
        get
        {
            return (string)GameModel.Instance.shopItemConfig[item]["NAME"];
        }
    }
    public string Info
    {
        get
        {
            return (string)GameModel.Instance.shopItemConfig[item]["DES"];
        }
    }

    public int Level { get; set; }
    public bool useGem;
    public int Price
    {
        get
        {
            if (Level < 5)
                return (int)GameModel.Instance.shopItemConfig[item]["LEVEL" + (Level + 1)];
            else
                return 9999;
        }
    }

    public void Load()
    {
        if (PlayerData.Current.items.ContainsKey(item.ToString()))
        {
            Level = PlayerData.Current.items[item.ToString()].level;
        }
    }
    public void Save()
    {
        if (PlayerData.Current.items.ContainsKey(item.ToString()))
        {
            PlayerData.Current.items[item.ToString()].level = Level;
            PlayerData.Current.Save();
        }
    }
}

public class BonusItemController : MonoBehaviour
{
    public BonusInfo itemInfo;

    [Header("Objects")]
    public Text name;
    public GameObject upgradeByCoin;
    public GameObject upgradeByGem;

    public GameObject btnBuy;
    public GameObject levelFill;    

    private Text price;
    private Button btnBuyScript;

    void Start()
    {
        itemInfo.Load();
        name.text = itemInfo.Name;

        Reload();
    }

    void Update()
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
       
        upgradeByCoin.SetActive(false);
        upgradeByGem.SetActive(false);

        if (itemInfo.useGem)
        {
            upgradeByGem.SetActive(true);
        }
        else
        {
            upgradeByCoin.SetActive(true);
        }
        

        SetLevel(itemInfo.Level);

        price = btnBuy.GetComponentInChildren<Text>();
        price.text = itemInfo.Price + "";       

        btnBuyScript = btnBuy.GetComponentInChildren<Button>();
        btnBuyScript.onClick.AddListener(OnBuyItem);
    }

    public void OnBuyItem()
    {
        if (itemInfo.Level == 5)
            return;

        if (itemInfo.useGem)
        {
            if (PlayerData.Current.gems > itemInfo.Price)
            {
                PlayerData.Current.gems -= itemInfo.Price;
                GemController.Instance.SetGems(PlayerData.Current.gems);
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
            if (PlayerData.Current.gold > itemInfo.Price)
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
}
