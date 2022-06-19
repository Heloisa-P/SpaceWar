using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private bool groundIstance;

    private void Update()
    {
        if(!GameManager.instance.isGamePaused)
        {
            if (!groundIstance)
            {
                if (transform.position.x <= 0)
                {
                    groundIstance = true;
                    GameObject temporaryObj = Instantiate(GameManager.instance.backgroundPrefab);

                    temporaryObj.transform.position = new Vector3(
                        transform.position.x + GameManager.instance.backgroundSize,
                        transform.position.y,
                        0);
                }
            }

            if (transform.position.x < GameManager.instance.destroyPosition)
                Destroy(gameObject);
        }        
    }

    private void FixedUpdate()
    {
        if(!GameManager.instance.isGamePaused)
            MoveBack();
    }

    private void MoveBack()
    {
        transform.Translate(Vector2.left * GameManager.instance.backgroundSpeed * Time.deltaTime);
    }
}
