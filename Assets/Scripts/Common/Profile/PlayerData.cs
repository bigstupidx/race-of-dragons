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

    public string id;
    public string name;
    public string avatarUrl;

    public int level;
    public int exp;
    public int elo;
    public int gold;
    public int gem;
    public int played;
    public int win;

    public Dictionary<string, DragonPropertie> dragons;
    public Dictionary<string, ItemPropertie> items;
    public Dictionary<string, string> chats;    
    public Dictionary<string, FriendItem> friends;
    public List<string> friendList;

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
        gem = 0;
        elo = 0;
        played = 0;
        win = 0;

        InitDragons();
        InitItems();
        InitChat();
        InitFriendList();
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

    private void InitChat()
    {
        chats = new Dictionary<string, string>();
        var chatModel = GameModel.Instance.chatConfig;
        foreach (var item in chatModel)
        {
            chats.Add(item.Key.ToString(), item.Value);
        }
    }

    private void InitFriendList()
    {
        friendList = new List<string>();
        friends = new Dictionary<string, FriendItem>();
    }

    public void SetFriendList(IList<IDictionary<string, object>> list)
    {
        friends.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            FriendItem friend = new FriendItem(list[i]);
            friends.Add(friend.Id, friend);
        }
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

    public void SaveOnServer()
    {
        var param = new Dictionary<string, object>();
        param.Add("data", PlayerData.Current.ToDictionary());

        var taskSync = ParseCloud.CallFunctionAsync<ParseObject>("saveUserData", param).ContinueWith(t2 =>
        {
            if (t2.IsCompleted)
            {
                Debug.Log("Save user data to server done!");
            }
        });
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

        result.Add("name", name);
        result.Add("level", level);
        result.Add("exp", exp);
        result.Add("elo", elo);
        result.Add("gold", gold);
        result.Add("gem", gem);
        result.Add("played", played);
        result.Add("win", win);

        var dicDragons = new Dictionary<string, Dictionary<string, object>>();
        foreach (var item in dragons)
        {
            dicDragons.Add(item.Key.ToString(), item.Value.ToDictionary());
        }
        result.Add("dragons", dicDragons);

        var dicItems = new Dictionary<string, Dictionary<string, object>>();
        foreach (var item in items)
        {
            dicItems.Add(item.Key.ToString(), item.Value.ToDictionary());
        }
        result.Add("items", dicItems);

        result.Add("emojis", chats);

        var friendList = new List<string>();
        foreach (var item in friends)
        {
            friendList.Add(item.Key.ToString());
        }
        result.Add("friends", friendList);

        return result;
    }

    public void SyncData(ParseObject data)
    {
        name = data.Get<string>("name");
        avatarUrl = data.Get<string>("avatarUrl");
        level = data.Get<int>("level");
        exp = data.Get<int>("exp");
        gold = data.Get<int>("gold");
        elo = data.Get<int>("elo");
        gem = data.Get<int>("gem");
        played = data.Get<int>("played");
        win = data.Get<int>("win");

        var newDragons = data.Get<IDictionary<string, object>>("dragons");
        dragons.Clear();
        foreach (var item in newDragons)
        {
            var dragon = new DragonPropertie((IDictionary<string, object>)item.Value);
            dragons.Add(item.Key, dragon);
        }

        var newItems = data.Get<IDictionary<string, object>>("items");
        items.Clear();
        foreach (var item in newItems)
        {
            var itemProp = new ItemPropertie((IDictionary<string, object>)item.Value);
            items.Add(item.Key, itemProp);
        }

        var newEmoji = data.Get<IDictionary<string, object>>("emojis");
        chats.Clear();
        foreach (var item in newEmoji)
        {
            chats.Add(item.Key, item.Value.ToString());
        }

        var newFriendList = data.Get<IList<string>>("friends");
        friendList.Clear();
        foreach (var item in newFriendList)
        {
            friendList.Add(item.ToString());
        }

        //Save();
    }
}
