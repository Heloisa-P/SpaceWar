using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public float velocity;
    public bool redPowerUp;

    protected int index = 0;
    private float limit = -4;

    private void Awake()
    {
        if (redPowerUp)
            index = Random.Range(0, 3);
        else
            index = Random.Range(3, 6);
    }

    protected virtual void Update()
    {
        if (!GameManager.instance.isGamePaused)
        {
            if (gameObject.transform.position.x <= limit)
                Destroy(gameObject);
            else
                transform.Translate(Vector2.left * velocity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.instance.GivePowerUp(index);

            Destroy(gameObject);
        }
    }
}
