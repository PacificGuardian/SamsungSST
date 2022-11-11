using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    float duration = 10;

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Object.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Object.Destroy(gameObject);
    }
}
