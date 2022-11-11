using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection6 : MonoBehaviour {
    public GameObject CamTruePos;
    public GameObject CamRot;
    public GameObject OppTruePos;
    public GameObject TargetBody;

    public bool stopFeed = false;

    [Range(0.1f, 50)]
    public float Multiplier = 30;
    [Range(-180f, 180f)]
    public float RotationX = 0f;
    [Range(-180f, 180f)]
    public float RotationY = 0f;
    [Range(-180f, 180f)]
    public float RotationZ = 0f;
    public int avg_len = 10;
    Vector3 Cam;
    Vector3 Opp;
    Vector3 Proj;
    Quaternion rotAvg; Vector3 posAvg;
    List<Quaternion> lstRot = new List<Quaternion>();
    List<Vector3> lstPos = new List<Vector3>();
    int r_cnt = 0;
    
    void Update() {
        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = (Opp - Cam) ;

        //Input cam angle
        Quaternion qr; qr.x = CamRot.transform.localRotation.x; qr.y = CamRot.transform.localRotation.y; qr.z = CamRot.transform.localRotation.z; qr.w = CamRot.transform.localRotation.w;
        lstRot.Add(qr);
        lstPos.Add(Proj);
        if (r_cnt == 0) {
            posAvg = Proj;
            rotAvg = qr;
        }
        else {
            posAvg += Proj;
            rotAvg.x += qr.x; rotAvg.y += qr.y; rotAvg.z += qr.z; rotAvg.w += qr.w;
        }
        if (r_cnt >= avg_len) {
            rotAvg.x -= lstRot[0].x; rotAvg.y -= lstRot[0].y; rotAvg.z -= lstRot[0].z; rotAvg.w -= lstRot[0].w;
            lstRot.RemoveAt(0);
            posAvg -= lstPos[0];
            lstPos.RemoveAt(0);
        }
        else
            r_cnt++;

        Quaternion rR;
        float k = 1.0f / Mathf.Sqrt(rotAvg.x * rotAvg.x + rotAvg.y * rotAvg.y + rotAvg.z * rotAvg.z + rotAvg.w * rotAvg.w);
        rR.x = rotAvg.x * k; rR.y = rotAvg.y * k; rR.z = rotAvg.z * k; rR.w = rotAvg.w * k;

        Vector3 pP = posAvg / r_cnt;
        Vector3 rpP = Quaternion.Euler(RotationX - rR.eulerAngles.x, RotationZ - rR.eulerAngles.z , rR.eulerAngles.y - RotationY) * pP * Multiplier;

        transform.position = rpP;
        
        if(!stopFeed) {
            TargetBody.transform.rotation = Quaternion.identity;

            TargetBody.transform.Rotate(OppTruePos.transform.eulerAngles, Space.Self);

            TargetBody.transform.Rotate(RotationX - rR.eulerAngles.x, RotationZ - rR.eulerAngles.z, rR.eulerAngles.y - RotationY, Space.World);
        }
    }
}
