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
        if (PlayerPrefs.GetInt(key, 1) == 1)
        {
            PlayerPrefs.SetInt(key, 0);
        }
        else
        {
            PlayerPrefs.SetInt(key, 1);
        }

        ReloadState();
    }
	
}
