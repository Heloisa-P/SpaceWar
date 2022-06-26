using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform[] lines = new Transform[3];

    private Animator anim;
    private int positionNow = 1;
    private string damageKey = "damageKey";

    private string skinPrefsKey = "Skin Prefs";
    [HideInInspector]
    public int hp = 3;
    [HideInInspector]
    public float coolDown = 1;
    [HideInInspector]
    public float shotSpeed = 1;

    [Header("References")]
    public GameObject explosionEffect;

    [Header("Shot Properties")]
    public GameObject normalShotPrefab;
    public GameObject buffShotPrefab;

    private float lastShot;
    private GameObject shotPrefab;

    [Header("Audio Control")]
    public AudioClip[] audios;
    private AudioSource audioSource;

    [Header("Skin Prefs")]
    public RuntimeAnimatorController[] controllers;
    public GameObject[] defaultShots;
    public GameObject[] buffedShots;
    private int selectedIndex = 0;

    [HideInInspector]
    public bool isInvencible = false;

    #region "Start"

    private void Start()
    {
        shotPrefab = normalShotPrefab;
        PlayerPrefs.SetInt(damageKey, 1);
        audioSource = GetComponent<AudioSource>();

        shotPrefab = defaultShots[selectedIndex];
        buffShotPrefab = buffedShots[selectedIndex];
    }

    private void Update()
    {
        if (GameManager.instance.isGamePaused)
            anim.speed = 0;
        else
            anim.speed = 1;
    }

    public void GetPrefs()
    {
        if (PlayerPrefs.HasKey(skinPrefsKey))
        {
            string[] data = PlayerPrefs.GetString(skinPrefsKey).Split('|');

            selectedIndex = int.Parse(data[0]);
            hp = int.Parse(data[1]);
            coolDown = float.Parse(data[2]) / 10;
            shotSpeed = float.Parse(data[3]) / 10;
        }

        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = controllers[selectedIndex];
    }

    #endregion

    #region "Movement"

    public void goUp()
    {
        if (positionNow != 2)
        {
            positionNow += 1;
            transform.position = lines[positionNow].position;
        }
    }

    public void goDown()
    {
        if (positionNow != 0)
        {
            positionNow -= 1;
            transform.position = lines[positionNow].position;
        }
    }

    #endregion

    #region "Shot"

    public void Shot()
    {
        if (Time.time - lastShot > coolDown)
        {
            GameObject shot = Instantiate(shotPrefab);
            shot.GetComponent<Shot>().speed = shotSpeed;
            shot.transform.position = gameObject.transform.position;
            lastShot = Time.time;

            PlayAudio(0);
        }
    }

    public void NewShotSpeed(float newSpeed)
    {
        shotSpeed = newSpeed;
    }

    public IEnumerator FastShot(float duration)
    {
        float oldCoolDown = coolDown;
        coolDown = coolDown / 2;

        yield return new WaitForSeconds(duration);

        coolDown = oldCoolDown;
    }

    public IEnumerator ShotBuff(int newDamage, float duration)
    {
        shotPrefab = buffShotPrefab;
        int oldDamage = PlayerPrefs.GetInt(damageKey);
        NewDamage(newDamage);

        yield return new WaitForSeconds(duration);

        shotPrefab = normalShotPrefab;
        NewDamage(oldDamage);
    }

    #endregion

    #region "Others"

    public void NewDamage(int newDamage)
    {
        PlayerPrefs.SetInt(damageKey, newDamage);
    }

    public IEnumerator Die()
    {
        PlayAudio(2);
        
        yield return new WaitForSeconds(0.3f);
        
        GameObject explosion = Instantiate(explosionEffect);
        explosion.transform.position = transform.position;
        Destroy(gameObject);


    }

    public IEnumerator GetProtection(float duration)
    {
        isInvencible = true;
        anim.SetBool("takeDamage", true);

        yield return new WaitForSeconds(duration);

        anim.SetBool("takeDamage", false);
        isInvencible = false;
    }

    #endregion

    #region "Audio"

    public void PlayAudio(int index)
    {
        audioSource.clip = audios[index];
        audioSource.Play();
    }

    #endregion
}
