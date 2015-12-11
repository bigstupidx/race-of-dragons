using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public enum Emoji
{
    Happy = 0,
    Fun,
    Sweet,
    Troll,
    Grim,
    Joy,
    Cry,
    Humm
}

[Serializable]
public class ChatItem
{
    public Emoji emoji;
    public string Text { get; set; }
    public int Price = 1000;

    public ChatItem()
    {

    }

    public ChatItem(string text)
    {
        Text = text;
    }

    public void Load()
    {
        Text = PlayerData.Current.chats[emoji.ToString()];
    }
    public void Save()
    {
        PlayerData.Current.chats[emoji.ToString()] = Text;
        PlayerData.Current.Save();
    }
}


public class ChatEmojiController : MonoBehaviour
{
    public ChatItem chatItem;

    [Header("Object")]
    public Text textChat;

	void Start ()
    {
        chatItem.Load();
        textChat.text = chatItem.Text;
	}
		
	void Update ()
    {
	
	}

    public void Reload()
    {
        textChat.text = chatItem.Text;
    }

    public void OnEditClick()
    {
        var keyboard = TouchScreenKeyboard.Open(chatItem.Text, TouchScreenKeyboardType.ASCIICapable);
        StartCoroutine(_OnChangeTextDone(keyboard));
    }

    private IEnumerator _OnChangeTextDone(TouchScreenKeyboard keyboard)
    {
        while (!keyboard.done)
            yield return null;

        if (PlayerData.Current.gold > chatItem.Price)
        {
            CoinController.Instance.SetCoins(PlayerData.Current.gold - chatItem.Price);
            chatItem.Text = keyboard.text;
            chatItem.Save();
            Reload();
        }
        else
        {
            WarningDialogController.Instance.ShowNotEnoughCoins();
        }
    }
}
