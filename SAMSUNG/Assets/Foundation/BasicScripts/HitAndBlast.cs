using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitAndBlast : MonoBehaviour
{
    public static int hitCnt = 0;
    public GameObject blast;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet") {
            GameObject c = GameObject.Find("InterceptCanvas");
            if(c != null) {
                if(c.transform.Find("Score") != null) {
                    hitCnt++;
                    c.transform.Find("Score").GetComponent<Text>().text = "攔截數量：" + hitCnt;
                }
            }
        }

        GameObject b = Instantiate(blast, collision.contacts[0].point, Quaternion.identity);
        Object.Destroy(b, 1000);
        Object.Destroy(gameObject);
    }
}
