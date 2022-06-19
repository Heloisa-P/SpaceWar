using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [Header("Movement Properties")]
    public float speed;
    private Vector2 movementDirection = Vector2.up;

    [Header("ShotProperties")]
    public Transform[] shotSpawns;
    public GameObject shotPrefab;
    public float coolDown;

    [Header("Life Properties")]
    public float hp;
    public GameObject lifeBarPrefab;
    private float playerDamage;
    private GameObject lifeCanvas;
    private float oldScale = 1;
    private float originalHP;
    private GameObject lifeBar;
    private Transform life;
    private AudioSource damageAudio;

    [Header("Animation Properties")]
    public Animator anim;

    [Header("Defeat Properties")]
    public GameObject explosionEffect;
    public float score;

    private void Start()
    {
        damageAudio = GetComponent<AudioSource>();
        StartCoroutine(Shot());
        playerDamage = PlayerPrefs.GetInt("damageKey");

        lifeCanvas = GameObject.FindGameObjectWithTag("BossLifeCanvas");
        originalHP = hp;
        lifeBar = Instantiate(lifeBarPrefab, lifeCanvas.transform);
        life = lifeBar.transform.GetChild(0);
    }

    void Update()
    {
        if (transform.position.x > 2)
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.y > 0.8f)
           movementDirection = Vector2.down;

       else if(transform.position.y < -0.8f)
           movementDirection = Vector2.up;

       transform.Translate(movementDirection * speed * Time.deltaTime);

    }

    private IEnumerator Shot()
    {
        for (int i = 0; i < shotSpawns.Length; i++)
        {
            GameObject shot = Instantiate(shotPrefab);
            shot.transform.position = shotSpawns[i].transform.position;
        }

        yield return new WaitForSeconds(coolDown);

        StartCoroutine(Shot());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shot")
        {
            Destroy(collision.gameObject);

            if (hp > playerDamage)
            {
                hp -= playerDamage;
                anim.SetTrigger("damage");
                damageAudio.Play();
            }
            else
            {
                Die();
            }

            GameManager.instance.PlayDamageAudio();
            updateLifeBar();
        }
    }

    private void Die()
    {
        GameObject explosion = Instantiate(explosionEffect);
        explosion.transform.position = transform.position;
        explosion.transform.localScale = new Vector3(6, 6, 6);
        GameManager.instance.Score(score);
        Destroy(gameObject);
        Destroy(lifeBar);
        GameManager.instance.isBossOn = false;
        GameManager.instance.CallUpgPanel();
    }

    private void updateLifeBar()
    {
        float newScale;

        if (hp > 0)
        {
            newScale = oldScale - playerDamage / originalHP;
            oldScale = newScale;
        }
        else
        {
            newScale = 0;
        }

        life.localScale = new Vector3(newScale, 1, 1);

    }
}
