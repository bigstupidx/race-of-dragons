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
    public int gold;

    public List<DragonPropertie> listDragon;
    public int currentDragonIndex;

    #endregion

    public DragonPropertie CurrentDragon
    {
        get
        {
            if (listDragon != null && listDragon.Count > 0)
                return listDragon[currentDragonIndex];

            return null;
        }
    }

    public void InitDefaultData()
    {
        level = 1;
        exp = 0;
        gold = 0;

        listDragon = new List<DragonPropertie>();
        DragonPropertie fireDragon = new DragonPropertie();
        listDragon.Add(fireDragon);

        currentDragonIndex = 0;
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
}
