using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sceneCfg : MonoBehaviour
{
    public Image imgGaze;
    public Camera came;
    public float time;
    public int distanceOfRay;
    public static Image ImgGaze;
    public static Camera cam;
    public static float Time;
    public static int rayDistance;
    private void Awake() {
        ImgGaze = imgGaze;
        Time = time;
        rayDistance = distanceOfRay;
        cam = came;
        imgGaze.fillAmount = 0;
    }
}
