using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public enum Item
{
    Shield = 0,
    SpeedUp,
    Energy,
    Rocket
}

[Serializable]
public class ItemPropertie
{
    public Item item;
    public int level = 1;

    public ItemPropertie()
    {
        
    }

    public ItemPropertie(Item item)
    {
        this.item = item;
        level = 1;
    }

    public float GetDuration()
    {
        level = PlayerData.Current.items[item.ToString()].level;
        return GameModel.Instance.itemLevelConfig[item][level];
    }

    public ItemPropertie(IDictionary<string, object> data)
    {
        level = int.Parse(data["level"].ToString());        
        item = (Item)(int.Parse(data["item"].ToString()));
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result.Add("level", level);
        result.Add("item", (int)item);

        return result;
    }

}

public class ItemController : MonoBehaviour
{
    public Image itemImage;
    public Animator animator;
    public GameObject timeBonus;
    public Animator timeBonusAnimator;
    public Text timeBonusText;

    [Header("Items sprite")]
    public Sprite[] spriteItems;

    [HideInInspector] public PlayerController player;

    private bool hasItem;
    private Item currentItem;

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void RandomItem()
    {
        if (hasItem == false)
        {
            int rand = UnityEngine.Random.Range(0, spriteItems.Length);
            currentItem = (Item)rand;
            itemImage.enabled = true;
            itemImage.sprite = spriteItems[rand];
            animator.Play("item_icon_appear", -1, 0);
            animator.SetBool("isDisappear", false);
            hasItem = true;
        }
    }

    public void UseItem()
    {
        if (hasItem)
        {
            animator.SetBool("isDisappear", true);
            hasItem = false;

            player.UserItem(currentItem);

            if (currentItem == Item.Energy)
            {
                timeBonus.SetActive(true);
                timeBonusText.text = "-5s";
                timeBonusAnimator.Play("reduce_time_cooldown_appear", -1, 0);
            }
        }
    }
}
