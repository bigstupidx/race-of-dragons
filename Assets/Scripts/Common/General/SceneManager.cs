using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum E_SCENE
{
    Scene_Init,
    Scene_Animated_Loading,
    Scene_Loading,
    Scene_Login,
    Scene_MainMenu,
    Scene_Option,
    Scene_Shop,
    Scene_Friend,
    Scene_Select_Mode,
    Scene_Waiting,
    Scene_Game
}

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField]
    private float m_minDuration = 1;

    public Image fadeSprite;

    void Start ()
    {
        //fadeSprite.transform.localScale = new Vector3(Screen.width, Screen.height, 1);
	}
		
	void Update ()
    {
	
	}
    
    public void ChangeScene(E_SCENE scene)
    {
        Application.LoadLevelAsync(scene.ToString());
    }

    public void LoadScene(E_SCENE scene, string music)
    {
        StartCoroutine(_LoadScene(scene.ToString(), music));
    }

    private IEnumerator _LoadScene(string sceneName, string music)
    {

        // Fade to black
        yield return StartCoroutine(_FadeIn());

        // Load loading screen
        yield return Application.LoadLevelAsync(E_SCENE.Scene_Animated_Loading.ToString());

        // !!! unload old screen (automatic)

        float endTime = Time.time + m_minDuration;

        if (Time.time < endTime)

            yield return new WaitForSeconds(endTime - Time.time);

        // Fade to loading screen
        yield return StartCoroutine(_FadeOut());

        // Load level async
        yield return Application.LoadLevelAsync(sceneName);

        // Load appropriate zone's music based on zone data
        //MusicManager.PlayMusic(music);

        // Fade to black
        yield return StartCoroutine(_FadeIn());

        // !!! unload loading screen
        //LoadingSceneManager.UnloadLoadingScene();

        // Fade to new screen
        //yield return StartCoroutine(_FadeOut());

    }

    private IEnumerator _FadeOut()
    {        
        fadeSprite.color = new Color(0, 0, 0, 0);
        float alpha = fadeSprite.color.a;
        while (alpha < 1)
        {
            alpha += 0.03f;
            fadeSprite.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

    }

    private IEnumerator _FadeIn()
    {
        fadeSprite.color = new Color(0, 0, 0, 1);
        float alpha = fadeSprite.color.a;
        while (alpha > 0)
        {
            alpha -= 0.03f;
            fadeSprite.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
