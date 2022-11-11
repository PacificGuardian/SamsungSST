using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public GameObject SpwanParent;
    public GameObject Bullet;
    float fireRate = 0.25f;

    void Update() {
         fireRate -= Time.deltaTime;
        if(fireRate <= 0) {
            fireRate = 0.25f;
            GameObject b = Instantiate(Bullet, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
            b.transform.parent = SpwanParent.transform;
        }
    }
}
