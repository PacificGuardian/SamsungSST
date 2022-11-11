using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection7_invC : MonoBehaviour {
    public GameObject CamTruePos;
    public GameObject CamRot;
    public GameObject OppTruePos;
    public GameObject TargetBody;
    public GameObject TargetBodyOri;

    public bool stopPosFeed = false;
    public bool stopOriFeed = false;
    public bool usePosAvg = true;
    public bool useOriAvg = true;

    [Range(0.1f, 50)]
    public float Multiplier = 30;
    [Range(-180f, 180f)]
    public float RotationX = 0f;
    [Range(-180f, 180f)]
    public float RotationY = 0f;
    [Range(-180f, 180f)]
    public float RotationZ = 0f;
    public int avg_len_pos = 10;
    public int avg_len_ori = 10;
    Vector3 Cam;
    Vector3 Opp;
    Vector3 Proj;
    Quaternion rotAvg; Vector3 posAvg;
    List<Quaternion> lstRot = new List<Quaternion>();
    List<Vector3> lstPos = new List<Vector3>();
    Quaternion rotOriAvg;
    int r_cnt_pos = 0;

    List<Quaternion> lstOriRot = new List<Quaternion>();
    public float ori_change_th = 0.5f;
    int o_cnt_change = 0;

    void Update() {
        //Calculate position
        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = (Opp - Cam) ;

        //Input cam angle
        Quaternion qr; qr.x = CamRot.transform.localRotation.x; qr.y = CamRot.transform.localRotation.y; qr.z = CamRot.transform.localRotation.z; qr.w = CamRot.transform.localRotation.w;

        //Apply average to position
        lstRot.Add(qr);
        lstPos.Add(Proj);
        if (r_cnt_pos == 0) {
            posAvg = Proj;
            rotAvg = qr;
        }
        else {
            posAvg += Proj;
            rotAvg.x += qr.x; rotAvg.y += qr.y; rotAvg.z += qr.z; rotAvg.w += qr.w;
        }
        if (r_cnt_pos >= avg_len_pos) {
            rotAvg.x -= lstRot[0].x; rotAvg.y -= lstRot[0].y; rotAvg.z -= lstRot[0].z; rotAvg.w -= lstRot[0].w;
            lstRot.RemoveAt(0);
            posAvg -= lstPos[0];
            lstPos.RemoveAt(0);
        }
        else
            r_cnt_pos++;

        Quaternion rR;
        float k = 1.0f / Mathf.Sqrt(rotAvg.x * rotAvg.x + rotAvg.y * rotAvg.y + rotAvg.z * rotAvg.z + rotAvg.w * rotAvg.w);
        rR.x = rotAvg.x * k; rR.y = rotAvg.y * k; rR.z = rotAvg.z * k; rR.w = rotAvg.w * k;

        Vector3 pP = posAvg / r_cnt_pos;
        Vector3 rpP = Quaternion.Euler(RotationX - rR.eulerAngles.x, RotationZ - rR.eulerAngles.z , rR.eulerAngles.y - RotationY) * pP * Multiplier;

        if(!stopPosFeed) { 
            if(usePosAvg)
                transform.position = rpP;
            else
                transform.position = Quaternion.Euler(RotationX - qr.eulerAngles.x, RotationZ - qr.eulerAngles.z, qr.eulerAngles.y - RotationY) * Proj * Multiplier;
        }

        //Calculate orientation
        TargetBodyOri.transform.rotation = Quaternion.identity;
        TargetBodyOri.transform.Rotate(OppTruePos.transform.eulerAngles, Space.Self);
        TargetBodyOri.transform.Rotate(RotationX - rR.eulerAngles.x, RotationZ - rR.eulerAngles.z, rR.eulerAngles.y - RotationY, Space.World);

        //Apply average to orientation
        if (lstOriRot.Count == 0) {
            lstOriRot.Add(TargetBodyOri.transform.rotation);
            rotOriAvg = TargetBodyOri.transform.rotation;
        }
        bool b_compute = false;
        if (lstOriRot.Count < avg_len_ori-1)
            b_compute = true;
        else {
            float aDist = Quaternion.Angle(TargetBodyOri.transform.rotation, lstOriRot[0]);
            if(aDist >= ori_change_th) {
                //print("Dist:" + aDist);
                o_cnt_change++;
                if (o_cnt_change >= 5)
                    b_compute = true;
            }
            else {
                o_cnt_change = 0;
                b_compute = true;
            }
        }
        if(b_compute) { 
            lstOriRot.Add(TargetBodyOri.transform.rotation);
            rotOriAvg.x += TargetBodyOri.transform.rotation.x;
            rotOriAvg.y += TargetBodyOri.transform.rotation.y;
            rotOriAvg.z += TargetBodyOri.transform.rotation.z;
            rotOriAvg.w += TargetBodyOri.transform.rotation.w;
            if (lstOriRot.Count >= avg_len_ori) {
                rotOriAvg.x -= lstOriRot[0].x; rotOriAvg.y -= lstOriRot[0].y; rotOriAvg.z -= lstOriRot[0].z; rotOriAvg.w -= lstOriRot[0].w;
                lstOriRot.RemoveAt(0);
            }
        }

        if (!stopOriFeed) {
            if (useOriAvg) {
                Quaternion oR;
                float o = 1.0f / Mathf.Sqrt(rotOriAvg.x * rotOriAvg.x + rotOriAvg.y * rotOriAvg.y + rotOriAvg.z * rotOriAvg.z + rotOriAvg.w * rotOriAvg.w);
                if (o < 0.0001f)
                    print("Zero:" + o);
                oR.x = rotOriAvg.x * o; oR.y = rotOriAvg.y * o; oR.z = rotOriAvg.z * o; oR.w = rotOriAvg.w * o;
                TargetBody.transform.rotation = oR;
            }
            else
                TargetBody.transform.rotation = TargetBodyOri.transform.rotation;
        }
    }
}
