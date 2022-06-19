using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("MainAudio").Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("Music Value"))
            audioSource.volume = PlayerPrefs.GetFloat("Music Value");
        else
            audioSource.volume = 1.0f;
    }
}
