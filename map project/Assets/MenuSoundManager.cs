using UnityEngine;
using UnityEngine.UI;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField]
    GameObject soundonImage;
    [SerializeField]
    GameObject soundoffImage;
    private bool muted = false;
    void Start()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
        if(muted == false)
        {
            soundonImage.SetActive(true);
            soundoffImage.SetActive(false);
        }
        else
        {
            soundonImage.SetActive(false);
            soundoffImage.SetActive(true);
        }
        if (PlayerPrefs.HasKey("muted"))
        {
            Load();
        }
        else
        {
            PlayerPrefs.SetInt("muted",1);
            Save();
        }

        AudioListener.pause = muted;
    }

    private void UpdateButtonIcon()
    {
        if(muted == false)
        {
            soundonImage.SetActive(true);
            soundoffImage.SetActive(false);
        }
        else
        {
            soundonImage.SetActive(false);
            soundoffImage.SetActive(true);
        }
    }

    public void OnButtonPress()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }
        UpdateButtonIcon();
        Save();
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}