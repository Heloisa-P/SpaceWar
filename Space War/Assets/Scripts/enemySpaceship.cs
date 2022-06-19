using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpaceship : Obstacles
{
    public GameObject shotPrefab;
    public float coolDown = 5f;

    private void Start()
    {
        StartCoroutine(Shot());
    }

    private IEnumerator Shot()
    {
        GameObject shot = Instantiate(shotPrefab);
        shot.transform.position = gameObject.transform.position;

        yield return new WaitForSeconds(coolDown);

        StartCoroutine(Shot());
    }
}
