using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Item
{
    Shield = 0,
    SpeedUp,
    Energy
}

public class ItemController : MonoBehaviour
{
    public Image itemImage;
    public Animator animator;

    [Header("Items sprite")]
    public Sprite[] spriteItems;

    [HideInInspector] public PlayerController player;

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
        int rand = Random.Range(0, spriteItems.Length);
        itemImage.sprite = spriteItems[rand];
        animator.SetBool("isDisappear", false);
    }

    public void UseItem()
    {
        animator.SetBool("isDisappear", true);
    }
}
