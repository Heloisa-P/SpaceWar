using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public float speed = 1f;

    private float DestroyPosition = -10f;

    private void FixedUpdate()
    {
        if (!GameManager.instance.isGamePaused)
        {
            if (transform.position.x > DestroyPosition)
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.instance.TakeDamage();
            Destroy(gameObject);
        }
    }
}
