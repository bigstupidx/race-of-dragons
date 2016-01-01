using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;

public class MoreFriendDialogController : MonoBehaviour
{
    private Animator animator;
    private List<FriendItem> suggestFriends;

    public GameObject moreFriendItemPrefab;
    public Transform contain;
    public Text loadingText;
	
	void Start ()
    {
        animator = GetComponent<Animator>();
        suggestFriends = new List<FriendItem>();

        var taskSync = ParseCloud.CallFunctionAsync<IList<IDictionary<string, object>>>("getSuggestFriends", null).ContinueWith(t2 =>
        {
            var suggestList = t2.Result;
            for (int i = 0; i < suggestList.Count; i++)
            {
                FriendItem friend = new FriendItem(suggestList[i]);
                suggestFriends.Add(friend);                
            }
        });

        StartCoroutine(_LoadSuggestFriend(taskSync));
    }

    private IEnumerator _LoadSuggestFriend(Task task)
    {
        while (!task.IsCompleted) yield return null;

        if (!task.IsCanceled && !task.IsFaulted)
        {
            for (int i = 0; i < suggestFriends.Count; i++)
            {
                GameObject go = Instantiate(moreFriendItemPrefab) as GameObject;
                MoreFriendItemController goScript = go.GetComponent<MoreFriendItemController>();
                goScript.SetFriendInfo(suggestFriends[i]);

                go.transform.parent = contain;
                go.transform.localScale = new Vector3(1, 1, 1);
            }

            loadingText.gameObject.SetActive(false);
        }
        else
        {
            loadingText.text = "Loading failed!";
        }
    }
	
	void Update ()
    {
	
	}

    public void OnBackClick()
    {
        SoundManager.Instance.playButtonSound();
        animator.SetBool("isDisappear", true);
    }

    public void DestroyItself()
    {
        Destroy(this.gameObject);
    }
}
