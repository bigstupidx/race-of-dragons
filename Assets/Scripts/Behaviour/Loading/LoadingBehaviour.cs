using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBehaviour : MonoBehaviour {

    public Slider progressBar;
    public Text loadingText;
    public int levelToLoad;

    private int loadProgress;

	// Use this for initialization
	void Start () {
        StartCoroutine(DisplayLoadingScreen());
	}
	
	IEnumerator DisplayLoadingScreen()
    {
        AsyncOperation asyncLoad = Application.LoadLevelAsync(levelToLoad);
        while (!asyncLoad.isDone)
        {
            loadProgress = (int)(asyncLoad.progress * 100);
            progressBar.value = loadProgress;
            loadingText.text = "Loading... " + loadProgress + "%";

            yield return null;
        }
    }
}
