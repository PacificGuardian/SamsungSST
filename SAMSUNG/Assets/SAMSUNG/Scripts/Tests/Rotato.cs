using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotato : BscM
{
    [SerializeField]
    GameObject A;
    [SerializeField]
    GameObject B;
    [SerializeField]
    Vector3 Goal; 
    private void Start() {
        StartCoroutine(Rotate(A, B, 1));
    }
}
