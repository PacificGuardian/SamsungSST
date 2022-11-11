using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionFormula2 : MonoBehaviour
{
    public GameObject CamTruePos;
    public GameObject OppTruePos;
    [Range(0.1f, 50)]
    public float Multiplier = 30;
    [Range(-180f, 180f)]
    public float Rotation = 0f;
    public int avg_len = 10;
    Vector3 Cam;
    Vector3 Opp;
    Vector3 Proj;
    float rotY = 0f;
    Vector3 posAvg;
    List<float> lstY = new List<float>();
    List<Vector3> lstPos = new List<Vector3>();
    int r_cnt = 0;

    void Update() {
        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = Opp - Cam;

        lstY.Add(CamTruePos.transform.eulerAngles.y);
        rotY += CamTruePos.transform.eulerAngles.y;
        //Vector3 rUp = Vector3.ProjectOnPlane(CamTruePos.transform.position, Vector3.up);
        //rotY += rUp.eulerAngles.y;
        lstPos.Add(Proj);
        if (r_cnt == 0)
            posAvg = Proj;
        else
            posAvg += Proj;
        if (r_cnt >= avg_len) {
            rotY -= lstY[0];
            lstY.RemoveAt(0);
            posAvg -= lstPos[0];
            lstPos.RemoveAt(0);
        }
        else {
            r_cnt++;
        }
        float rY = rotY / r_cnt;
        Vector3 pP = posAvg / r_cnt;
        print("rY:" + rY + " pos:" + posAvg);

        pP = Quaternion.Euler(0, Rotation - rY, 0) * pP * Multiplier;
        transform.position = pP;
        transform.eulerAngles = OppTruePos.transform.eulerAngles;
        transform.Rotate(0, 0, Rotation - rY);
    }
}
