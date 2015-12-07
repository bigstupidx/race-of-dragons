using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Item
{
    Shield = 0,
    SpeedUp,
    Energy,
    None
}

public class ItemController : MonoBehaviour
{
    public Image itemImage;
    public Animator animator;

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
            itemImage.sprite = spriteItems[rand];
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
        }
    }
}
