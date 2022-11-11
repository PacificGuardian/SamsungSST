using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection2 : MonoBehaviour
{
    public GameObject CamTruePos;
    public GameObject OppTruePos;
    [Range(0.1f, 50)]
    public float Multiplier = 30;
    [Range(-180f, 180f)]
    public float Rotation = 0f;
    public int avg_r_len = 10;
    Vector3 Cam;
    Vector3 Opp;
    Vector3 Proj;
    float rotY = 0f;
    List<float> lstY = new List<float>();
    int r_cnt = 0;

    void Update()
    {
        lstY.Add(CamTruePos.transform.eulerAngles.y);
        rotY += CamTruePos.transform.eulerAngles.y;
        if (r_cnt >= avg_r_len)
        {
            rotY -= lstY[0];
            lstY.RemoveAt(0);
        }
        else
        {
            r_cnt++;
        }
        float rY = rotY / r_cnt;
        print("rY:" + rY);

        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = Multiplier * (Opp - Cam);
        //Proj = Quaternion.AngleAxis(CamTruePos.transform.eulerAngles.y, Vector3.up) * Proj;
        Proj = Quaternion.Euler(0, Rotation - rY, 0) * Proj;
        transform.position = Proj;
        transform.eulerAngles = OppTruePos.transform.eulerAngles;
        transform.Rotate(0, 0, Rotation - rY);
        //transform.eulerAngles = Quaternion.Euler(0, 0, 0);//Quaternion.Euler(OppTruePos.transform.eulerAngles.x, OppTruePos.transform.eulerAngles.y + Rotation - rY, OppTruePos.transform.eulerAngles.z);
    }
}
