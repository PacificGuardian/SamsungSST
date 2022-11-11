using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
struct QRot {
    public float x, y, z, w;
}
*/
public class Projection4 : MonoBehaviour {
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
    QRot rotAvg; Vector3 posAvg;
    List<QRot> lstRot = new List<QRot>();
    List<Vector3> lstPos = new List<Vector3>();
    int r_cnt = 0;
    
    void Update() {
        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = (Opp - Cam) ;

        //QRot qr; qr.x = CamTruePos.transform.rotation.x; qr.y = CamTruePos.transform.rotation.y; qr.z = CamTruePos.transform.rotation.z; qr.w = CamTruePos.transform.rotation.w;
        //QRot qr; qr.x = CamRot.transform.rotation.x; qr.y = CamRot.transform.rotation.y; qr.z = CamRot.transform.rotation.z; qr.w = CamRot.transform.rotation.w;
        //Input cam angle
        QRot qr; qr.x = CamRot.transform.localRotation.x; qr.y = CamRot.transform.localRotation.y; qr.z = CamRot.transform.localRotation.z; qr.w = CamRot.transform.localRotation.w;
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
        //print("Pre-Rotated: rR:" + rR.eulerAngles + " pos:" + pP);
        Vector3 rpP = Quaternion.Euler(RotationX - rR.eulerAngles.x, RotationZ - rR.eulerAngles.z , rR.eulerAngles.y - RotationY) * pP * Multiplier;
        //print("Rotated: rR:" + rR.eulerAngles + " pos:" + pP);
        transform.position = rpP;
        //transform.eulerAngles = OppTruePos.transform.eulerAngles;
        //TargetBody.transform.localEulerAngles = OppTruePos.transform.eulerAngles;
        //transform.Rotate(0, 0, RotationZ - rR.eulerAngles.z);
        if(!stopFeed) {
            //TargetBody.transform.eulerAngles = new Vector3(0, 0, 0);
            print("Euler:" + OppTruePos.transform.eulerAngles + "Local Euler:" + OppTruePos.transform.localEulerAngles);
            //TargetBody.transform.Rotate(OppTruePos.transform.eulerAngles, Space.Self);
            TargetBody.transform.rotation = Quaternion.identity;
            TargetBody.transform.Rotate(OppTruePos.transform.eulerAngles, Space.Self);
            TargetBody.transform.Rotate(0, RotationZ - rR.eulerAngles.z, 0, Space.World);
            //TargetBody.transform.rotation = OppTruePos.transform.rotation;
            //TargetBody.transform.Rotate(0, 0, RotationZ - rR.eulerAngles.z);
        }

        /*float RotX = (Vector3.Angle(CamRot.transform.forward, Vector3.forward) - 90)/1.0f;
        float RotY = (Vector3.Angle(CamRot.transform.forward, Vector3.right) - 90)/1.0f;
        print("Y:" + RotY + " X:" + RotX);
        //Front
        //[tilt-up_down, tilt_left_right ,pan]
        //transform.Rotate(-RotX, -RotY, RotationZ - rR.eulerAngles.z);
        //Back
        //transform.Rotate(RotX, RotY, RotationZ - rR.eulerAngles.z);
        //Right
        //transform.Rotate(-RotY, RotX, RotationZ - rR.eulerAngles.z);
        //Left
        //transform.Rotate(RotY, -RotX, RotationZ - rR.eulerAngles.z);
        //ppP Direction
        float RotDir = Vector3.SignedAngle(pP, CamRot.transform.up, Vector3.up)+180;
        print("Dir:" + RotDir);
        Vector3 front = new Vector3(-RotX, -RotY, RotationZ - rR.eulerAngles.z);
        Vector3 right = new Vector3(-RotY, RotX, RotationZ - rR.eulerAngles.z);
        Vector3 back = new Vector3(RotX, RotY, RotationZ - rR.eulerAngles.z);
        Vector3 left = new Vector3(RotY, -RotX, RotationZ - rR.eulerAngles.z);
        Vector3 Rot;
        if (RotDir > 270 && RotDir <= 360) {
            //fonrt-right
            Rot = Vector3.Lerp(right, front, (RotDir-270)/90);
        }
        else if(RotDir >180 && RotDir <=270) {
            //right-back
            Rot = Vector3.Lerp(back, right, (RotDir - 180) / 90);
        }
        else if(RotDir >90 && RotDir <=180) {
            //back-left
            Rot = Vector3.Lerp(left, back, (RotDir - 90) / 90);
        }
        else if(RotDir >=0 && RotDir <=90) {
            //left-front
            Rot = Vector3.Lerp(front, left, RotDir / 90);
        }
        else
            Rot = new Vector3(0,0,0);
        print("ToBeRot:" + Rot);
        transform.Rotate(Rot);*/
    }
}
