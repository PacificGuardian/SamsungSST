using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour {
    public GameObject CamTarget;
    void Update() {
        transform.rotation = CamTarget.transform.rotation;
    }
}
