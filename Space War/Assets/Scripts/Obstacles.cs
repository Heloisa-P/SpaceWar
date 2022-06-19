using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public int hp;
    public float velocity;
    public int score;
    public Animator anim;
    public GameObject explosionEffect;

    private float limit = -4;
    private string damageKey = "damageKey";

    protected virtual void Update()
    {
        if(!GameManager.instance.isGamePaused)
        {
            if (gameObject.transform.position.x <= limit)
                Destroy(gameObject);
            else
                transform.Translate(Vector2.left * velocity * Time.deltaTime);
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Shot")
        {
            Destroy(collision.gameObject);

            if (hp > PlayerPrefs.GetInt(damageKey))
            {
                hp -= PlayerPrefs.GetInt(damageKey);
                anim.SetTrigger("damage");
            }
            else
            {
                Destroy();   
            }

            GameManager.instance.PlayDamageAudio();

        }
        if(collision.tag == "Player")
        {
           GameManager.instance.TakeDamage();
        }
    }

    public void Destroy()
    {
        GameObject explosion = Instantiate(explosionEffect);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
        GameManager.instance.Score(score);
    }
}
