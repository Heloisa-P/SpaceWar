using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float speed;
    private float DestroyPosition = 10f;

    private void FixedUpdate()
    {
        if(!GameManager.instance.isGamePaused)
            if (transform.position.x < DestroyPosition)
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            else
                Destroy(gameObject);
    }
}
