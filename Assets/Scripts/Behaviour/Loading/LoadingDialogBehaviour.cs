using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadingDialogBehaviour : MonoBehaviour
{
    public Animator animator;
    public Text progressText;
    public Coroutine loadingCorotine;
    public bool hasError;

	void Start ()
    {
	
	}
		
	void Update ()
    {
	
	}

    public void StopLoadingAnimaton()
    {
        animator.SetBool("isDisappear", true);
    }

    public void ForceStopLoading()
    {
        StopCoroutine(loadingCorotine);
        StopAllCoroutines();
        StopLoadingAnimaton();      
    }

    public void SetUpToDoList(Dictionary<string, IEnumerator> todo)
    {
        loadingCorotine = StartCoroutine(_ToDo(todo));
    }

    private IEnumerator _ToDo(Dictionary<string, IEnumerator> todo)
    {
        foreach (var item in todo)
        {
            progressText.text = item.Key;

            yield return StartCoroutine(item.Value);
        }

        StopLoadingAnimaton();
    }
}
