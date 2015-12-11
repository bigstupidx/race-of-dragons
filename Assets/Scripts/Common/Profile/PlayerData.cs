using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Parse;

[Serializable]
public class PlayerData
{
    #region Singleton
    private static PlayerData current;

    private PlayerData()
    {
        
    }

    public static PlayerData Current
    {
        get
        {
            if (current == null)
            {
                current = new PlayerData();
                current.Load();
            }
                
            return current;
        }
    }
    #endregion

    #region Variable

    public int level;
    public int exp;
    public int elo;
    public int gold = 100;
    public int gems;

    public Dictionary<string, DragonPropertie> dragons;
    public Dictionary<string, ItemPropertie> items;
    public string currentDragonIndex;

    #endregion

    public DragonPropertie CurrentDragon
    {
        get
        {
            if (dragons != null && dragons.Count > 0)
                return dragons[currentDragonIndex];

            return null;
        }
    }

    public void InitDefaultData()
    {
        level = 1;
        exp = 0;
        gold = 0;
        elo = 0;

        InitDragons();
        InitItems();
    }

    private void InitDragons()
    {
        dragons = new Dictionary<string, DragonPropertie>();
        DragonPropertie fireDragon = new DragonPropertie();
        dragons.Add(fireDragon.element.ToString(), fireDragon);

        currentDragonIndex = fireDragon.element.ToString();
    }

    private void InitItems()
    {
        items = new Dictionary<string, ItemPropertie>();
        items.Add(Item.Shield.ToString(), new ItemPropertie(Item.Shield));
        items.Add(Item.SpeedUp.ToString(), new ItemPropertie(Item.SpeedUp));
        items.Add(Item.Energy.ToString(), new ItemPropertie(Item.Energy));
        items.Add(Item.Rocket.ToString(), new ItemPropertie(Item.Rocket));
    }
    #region Save & Load

    private string FilePath()
    {
        return Application.persistentDataPath + "/PlayerData.dat";
    }

    public void Save(PlayerData data)
    {
        Debug.Log("[PlayerData] Save()");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(FilePath());
        bf.Serialize(file, data);
        file.Close();
    }

    public void Save()
    {
        Save(current);
    }

    public void Load()
    {
        if (File.Exists(FilePath()))
        {
            Debug.Log("[PlayerData] Load()");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(FilePath(), FileMode.Open);
            current = bf.Deserialize(file) as PlayerData;
            file.Close();
        }
        else
        {
            LoadDefault();
        }
    }

    public void LoadDefault()
    {
        InitDefaultData();
        current = this;
        Save(current);
    }

    #endregion

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();

        result.Add("level", level);
        result.Add("exp", exp);
        result.Add("elo", elo);
        result.Add("gold", gold);

        var dicDragons = new Dictionary<string, Dictionary<string, object>>();

        foreach (var item in dragons)
        {
            dicDragons.Add(item.Key.ToString(), item.Value.ToDictionary());
        }
        result.Add("dragons", dicDragons);
        //result.Add("items", items);

        return result;
    }

    public void SyncData(ParseObject data)
    {
        level = data.Get<int>("level");
        exp = data.Get<int>("exp");
        gold = data.Get<int>("gold");
        elo = data.Get<int>("elo");

        var newDragons = data.Get<IDictionary<string, object>>("dragons");
        dragons.Clear();
        foreach (var item in newDragons)
        {
            var dragon = new DragonPropertie((IDictionary<string, object>)item.Value);
            dragons.Add(item.Key, dragon);
        }
    }
}
