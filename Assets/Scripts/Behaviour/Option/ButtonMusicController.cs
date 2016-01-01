using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonMusicController : MonoBehaviour
{
    private Image image;

    public string key;
    public Sprite btnOn;
    public Sprite btnOff;
	
    void Awake()
    {
        image = GetComponent<Image>();
    }

	void Start ()
    {
        ReloadState();
    }

    public void ReloadState()
    {
        if (PlayerPrefs.GetInt(key, 1) == 1)
        {
            image.sprite = btnOn;
        }
        else
        {
            image.sprite = btnOff;
        }
    }

    public void OnClick()
    {
        SoundManager.Instance.playButtonSound();
        if (PlayerPrefs.GetInt(key, 1) == 1)
        {
            PlayerPrefs.SetInt(key, 0);
            if (key == "MUSIC")
            {
                SoundManager.Instance.backgroundSound.fadeOutAndStop(1);
                SoundManager.Instance.isPlayMenuBgm = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt(key, 1);
            if (key == "MUSIC")
            {
                SoundManager.Instance.playMenuBackgroundMusic(true);
            }
        }

        ReloadState();
    }
	
}
