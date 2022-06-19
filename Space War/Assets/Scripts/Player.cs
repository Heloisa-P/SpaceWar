using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform[] lines = new Transform[3];

    private Animator anim;
    private int positionNow = 1;
    private string damageKey = "damageKey";

    [Header("References")]
    public GameObject explosionEffect;

    [Header("Shot Properties")]
    public float shotSpeed;
    public GameObject normalShotPrefab;
    public GameObject buffShotPrefab;
    public float coolDown = 1f;

    private float lastShot;
    private GameObject shotPrefab;

    [Header("Audio Control")]
    public AudioClip[] audios; // 0 - shot ; 1 - damage ; 2 - death
    private AudioSource audioSource;

    [HideInInspector]
    public bool isInvencible = false;

    private void Start()
    {
        shotPrefab = normalShotPrefab;
        PlayerPrefs.SetInt(damageKey, 1);
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.instance.isGamePaused)
            anim.speed = 0;
        else
            anim.speed = 1;
    }

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
