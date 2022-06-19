using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private float timeStart;
    private void Start()
    {
        timeStart = Time.time;
    }
    void Update()
    {
        if(Time.time - timeStart >= 0.4f)
            Destroy(gameObject);
    }
}
