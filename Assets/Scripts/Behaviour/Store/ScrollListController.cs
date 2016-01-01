using UnityEngine;
using System.Collections;
using Prime31.ZestKit;

public class ScrollListController : MonoBehaviour
{
    public float offsetX;
    public int currentIndex;
    //public int itemWidth;

    private int numOfItem;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

	void Start ()
    {
        numOfItem = transform.childCount;
	}
		
	void Update ()
    {
	
	}

    private void ResetItem()
    {
        DragonItemController dragonItem = transform.GetChild(currentIndex).GetComponent<DragonItemController>();
        if (dragonItem != null)
            dragonItem.Reload();
    }

    public void OnNext()
    {
        SoundManager.Instance.playButtonSound();
        if (currentIndex < numOfItem - 1)
        {
            currentIndex++;            
        }
        else
        {
            currentIndex = 0;
        }

        rectTransform.ZKanchoredPositionTo(GetCorrectPosition(), 0.5f).start();
        ResetItem();
    }
        
    public void OnPrev()
    {
        SoundManager.Instance.playButtonSound();
        if (currentIndex > 0)
        {
            currentIndex--;            
        }
        else
        {
            currentIndex = numOfItem - 1;
        }
        rectTransform.ZKanchoredPositionTo(GetCorrectPosition(), 0.5f).start();
        ResetItem();
    }

    public Vector2 GetCorrectPosition()
    {
        return new Vector2(-1 * currentIndex * offsetX, 0);
    }
}
