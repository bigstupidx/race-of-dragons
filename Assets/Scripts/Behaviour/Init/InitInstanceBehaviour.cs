using UnityEngine;
using System.Collections;

public class InitInstanceBehaviour : MonoBehaviour
{
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Application.targetFrameRate = 60;
    }

	// Use this for initialization
	void Start ()
    {
        PlayerData.Current.Load();

        StartCoroutine(_LoadScene());
	}

    private IEnumerator _LoadScene()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.Instance.LoadScene(E_SCENE.Scene_MainMenu, null);
    }
}
