using UnityEngine;
using System.Collections;

public class CreditBehaviour : MonoBehaviour
{

    public Animator animator;
	
	void Start ()
    {
	
	}
	
	public void OnBackClick()
    {
        animator.SetBool("isDisappear", true);
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
