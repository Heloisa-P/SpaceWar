    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject configPanel;
    public Text modeButtonText;

    [HideInInspector]
    public bool isConfigOpen;

    [Header("Audio")]
    public Slider musicValue;
    public Slider sfxValue;
    private AudioSource mainAudio;
    private float valueMusic;
    private string musicValueKey = "Music Value";
    private float valueSFX;
    private string sfxValueKey = "SFX Value";

    private string modeKey = "isSwipeMode";
    private bool isSwipeMode = true;

    [Header("Skin Screen")]
    public GameObject skinPanel;

    private void Start()
    {
        mainAudio = GameObject.FindGameObjectWithTag("MainAudio").GetComponent<AudioSource>();
    }

    public void LoadGame()
    {
        if(!isConfigOpen)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    #region "Config"
    public void OpenConfig()
    {
        isConfigOpen = true;
        configPanel.SetActive(isConfigOpen);

        if (PlayerPrefs.HasKey(modeKey))
            setModeText(bool.Parse(PlayerPrefs.GetString(modeKey)));       
        else
            setModeText(true);

        if (PlayerPrefs.HasKey(musicValueKey))
            musicValue.value = PlayerPrefs.GetFloat(musicValueKey);
        else
            musicValue.value = 1.0f;

        if (PlayerPrefs.HasKey(sfxValueKey))
            sfxValue.value = PlayerPrefs.GetFloat(sfxValueKey);
        else
            sfxValue.value = 1.0f;
    }

    public void CloseConfig()
    {
        isConfigOpen = false;
        configPanel.SetActive(isConfigOpen);
    }

    #endregion

    #region "Skin"

    public void OpenSkinScreen()
    {
        skinPanel.SetActive(true);
    }

    public void CloseSkinScreen()
    {
        skinPanel.SetActive(false);
    }

    #endregion

    #region "Change Mode"
    public void changeMode()
    {
        if (PlayerPrefs.HasKey(modeKey))
        {
            isSwipeMode = bool.Parse(PlayerPrefs.GetString(modeKey));

            if (!isSwipeMode)
            {
                isSwipeMode = true;
                setModeText(true);
            }
            else
            {
                isSwipeMode = false;
                setModeText(false);
            }
        }
        else
        {
            isSwipeMode = false;
            setModeText(false);
        }
          
        SavePreferences();
    }

    public void SavePreferences()
    {
        PlayerPrefs.SetString(modeKey, isSwipeMode.ToString());
    }

    private void setModeText(bool isSwipe)
    {
        if (isSwipe)
            modeButtonText.text = "Swipe Mode";
        else
            modeButtonText.text = "Button Mode";
    }

    #endregion

    #region "Audio Control"

    public void MusicValueChanged()
    {
        valueMusic = musicValue.value;
        PlayerPrefs.SetFloat(musicValueKey, valueMusic);
        mainAudio.volume = valueMusic;
    }

    public void SFXValueChanged()
    {
        valueSFX = sfxValue.value;
        PlayerPrefs.SetFloat(sfxValueKey, valueSFX);
    }

    #endregion
}
