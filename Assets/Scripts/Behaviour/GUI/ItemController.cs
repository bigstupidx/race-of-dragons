using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Item
{
    Shield = 0,
    SpeedUp,
    Energy,
    Rocket
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
            int rand = Random.Range(0, spriteItems.Length);
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
