using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModel
{

    private static GameModel mInstance = null;

    public static GameModel Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameModel();
                mInstance.Load();
            }

            return mInstance;
        }
    }


    public Dictionary<Element, Dictionary<string, object>> shopDragonConfig;
    public Dictionary<Item, Dictionary<string, object>> shopItemConfig;
    public Dictionary<Element, Dictionary<int, float>> dragonLevelConfig;
    public Dictionary<Item, Dictionary<int, float>> itemLevelConfig;

    public void Load()
    {
        LoadShopDragonConfig();
        LoadShopItemConfig();
        LoadDragonLevelConfig();
        LoadItemLevelConfig();
    }

    public void LoadShopDragonConfig()
    {
        TextAsset assetpoint = Resources.Load("Data/shop_dragon_info") as TextAsset;

        string[] data = assetpoint.text.Split('\n');
        string[] strarr;

        shopDragonConfig = new Dictionary<Element, Dictionary<string, object>>();

        for (int i = 1; i < data.Length; i++)
        {
            if (data[i] == "") break;

            strarr = data[i].Split(',');
            Element ele = (Element)(int.Parse(strarr[0]));

            Dictionary<string, object> contain = new Dictionary<string, object>();
            contain.Add("NAME", strarr[1]);
            contain.Add("DES", strarr[2]);
            contain.Add("LEVEL1", int.Parse(strarr[3]));
            contain.Add("LEVEL2", int.Parse(strarr[4]));
            contain.Add("LEVEL3", int.Parse(strarr[5]));
            contain.Add("LEVEL4", int.Parse(strarr[6]));
            contain.Add("LEVEL5", int.Parse(strarr[7]));

            shopDragonConfig.Add(ele, contain);
        }
    }

    public void LoadShopItemConfig()
    {
        TextAsset assetpoint = Resources.Load("Data/shop_item_info") as TextAsset;

        string[] data = assetpoint.text.Split('\n');
        string[] strarr;

        shopItemConfig = new Dictionary<Item, Dictionary<string, object>>();

        for (int i = 1; i < data.Length; i++)
        {
            if (data[i] == "") break;

            strarr = data[i].Split(',');
            Item item = (Item)(int.Parse(strarr[0]));

            Dictionary<string, object> contain = new Dictionary<string, object>();
            contain.Add("NAME", strarr[1]);
            contain.Add("DES", strarr[2]);
            contain.Add("LEVEL1", int.Parse(strarr[3]));
            contain.Add("LEVEL2", int.Parse(strarr[4]));
            contain.Add("LEVEL3", int.Parse(strarr[5]));
            contain.Add("LEVEL4", int.Parse(strarr[6]));
            contain.Add("LEVEL5", int.Parse(strarr[7]));

            shopItemConfig.Add(item, contain);
        }
    }

    public void LoadDragonLevelConfig()
    {
        TextAsset assetpoint = Resources.Load("Data/dragon_level_config") as TextAsset;

        string[] data = assetpoint.text.Split('\n');
        string[] strarr;

        dragonLevelConfig = new Dictionary<Element, Dictionary<int, float>>();
        dragonLevelConfig.Add(Element.Fire, new Dictionary<int, float>());
        dragonLevelConfig.Add(Element.Ice, new Dictionary<int, float>());
        dragonLevelConfig.Add(Element.Thunder, new Dictionary<int, float>());

        for (int i = 1; i < data.Length; i++)
        {
            if (data[i] == "") break;

            strarr = data[i].Split(',');

            dragonLevelConfig[Element.Fire].Add(i, float.Parse(strarr[1]));
            dragonLevelConfig[Element.Ice].Add(i, float.Parse(strarr[2]));
            dragonLevelConfig[Element.Thunder].Add(i, float.Parse(strarr[3]));
        }
    }

    public void LoadItemLevelConfig()
    {
        TextAsset assetpoint = Resources.Load("Data/item_level_config") as TextAsset;

        string[] data = assetpoint.text.Split('\n');
        string[] strarr;

        itemLevelConfig = new Dictionary<Item, Dictionary<int, float>>();
        
        itemLevelConfig.Add(Item.Shield, new Dictionary<int, float>());
        itemLevelConfig.Add(Item.SpeedUp, new Dictionary<int, float>());
        itemLevelConfig.Add(Item.Energy, new Dictionary<int, float>());
        itemLevelConfig.Add(Item.Rocket, new Dictionary<int, float>());

        for (int i = 1; i < data.Length; i++)
        {
            if (data[i] == "") break;

            strarr = data[i].Split(',');

            itemLevelConfig[Item.Shield].Add(i, float.Parse(strarr[1]));
            itemLevelConfig[Item.SpeedUp].Add(i, float.Parse(strarr[2]));
            itemLevelConfig[Item.Energy].Add(i, float.Parse(strarr[3]));
            itemLevelConfig[Item.Rocket].Add(i, float.Parse(strarr[4]));
        }
    }

}
