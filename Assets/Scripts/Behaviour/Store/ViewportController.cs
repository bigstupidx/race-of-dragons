using UnityEngine;
using System.Collections;

public class ViewportController : MonoBehaviour
{
    private ScrollListController scroll;

    void Start()
    {
        scroll = GetComponentInChildren<ScrollListController>();
    }

    public void OnPrev()
    {
        if (scroll != null)
        {
            scroll.OnPrev();
        }
    }

    public void OnNext()
    {
        if (scroll != null)
        {
            scroll.OnNext();
        }
    }
}
