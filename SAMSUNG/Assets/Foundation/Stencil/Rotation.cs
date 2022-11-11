using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speedX = 0f;
    public float speedY = 0.1f;
    public float speedZ = 0f;
    void Update()
    {
        transform.Rotate(speedX, speedY, speedZ);
    }
}
