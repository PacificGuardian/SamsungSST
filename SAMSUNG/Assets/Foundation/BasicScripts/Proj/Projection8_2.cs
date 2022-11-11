using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection8_2 : MonoBehaviour {
    public GameObject CamTruePos;
    public GameObject CamRot;
    public GameObject OppTruePos;
    public GameObject TargetBody;
    public GameObject TargetBodyOri;

    public bool stopBodyHTrack = false;
    public bool stopPosFeed = false;
    public bool stopOriFeed = false;
    public bool usePosAvg = true;
    public bool useOriAvg = true;

    public float VORORatio = 3.05F;
    public float CamTrackerDist = 16F;
    public bool LockBodyHeight = true;

    private float Multiplier = 31.5F;
    [Range(-180f, 180f)]
    public float RotationX = 0f;
    [Range(-180f, 180f)]
    public float RotationY = 0f;
    [Range(-180f, 180f)]
    public float RotationZ = 0f;
    [Range(1f, 150f)]
    public float avg_len_pos = 10f;
    [Range(1f, 150f)]
    public float avg_len_ori = 10;
    Vector3 Cam;
    Vector3 Opp;
    Vector3 Proj;

    Vector3 posAvg = Vector3.zero;
    int pos_iniSkipFrame = 10;

    Vector3 rotOriAvg; Vector3 rotOriLast;
    Vector3 rotOriLoopCnt = Vector3.zero;
    int ori_iniSkipFrame = 10;

    List<Quaternion> i_dbgRotLst = new List<Quaternion>();
    List<Quaternion> o_dbgRotLst = new List<Quaternion>();
    List<Vector3> i_dbgPosLst = new List<Vector3>();
    List<Vector3> o_dbgPosLst = new List<Vector3>();

    void Start() { 
        if(!stopBodyHTrack)
            TargetBody.transform.localPosition = new Vector3(0,0,CamTrackerDist / VORORatio);
    }

    void Update() {
        if (!stopBodyHTrack && LockBodyHeight)
            TargetBody.transform.localPosition = new Vector3(0, 0, CamTrackerDist / VORORatio);
    }

    void FixedUpdate() {  
        //Calculate position
        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = (Opp - Cam) ;

        //Input cam angle
        Quaternion qr; qr.x = CamRot.transform.localRotation.x; qr.y = CamRot.transform.localRotation.y; qr.z = CamRot.transform.localRotation.z; qr.w = CamRot.transform.localRotation.w;
        Vector3 pP = Quaternion.Euler(RotationX - qr.eulerAngles.x, RotationZ - qr.eulerAngles.z, qr.eulerAngles.y - RotationY) * Proj * Multiplier;

        //Apply average to position
        if (pos_iniSkipFrame > 0)
            pos_iniSkipFrame--;
        else if (pos_iniSkipFrame == 0) {
            posAvg = pP;
            pos_iniSkipFrame--;
        }
        else {
            float f_sfPos = 2.0f / (avg_len_pos + 1);
            posAvg = f_sfPos * pP + (1 - f_sfPos) * posAvg;
        }

        //i_dbgPosLst.Add(pP);
        //o_dbgPosLst.Add(posAvg);

        if (!stopPosFeed) { 
            if(usePosAvg)
                transform.position = posAvg;
            else
                transform.position = pP;
        }

        //Calculate orientation
        rotOriLast = TargetBodyOri.transform.eulerAngles;
        TargetBodyOri.transform.rotation = OppTruePos.transform.rotation; //Local rotation
        TargetBodyOri.transform.Rotate(RotationX - qr.eulerAngles.x, RotationZ - qr.eulerAngles.z, qr.eulerAngles.y - RotationY, Space.World);

        //Apply average to orientation
        if (ori_iniSkipFrame > 0)
            ori_iniSkipFrame--;
        else if (ori_iniSkipFrame == 0) {
            rotOriAvg = TargetBodyOri.transform.eulerAngles;
            ori_iniSkipFrame--;
        }
        else {
            float f_sfOri = 2.0f / (avg_len_ori + 1);

            //print(TargetBodyOri.transform.eulerAngles);
            if (TargetBodyOri.transform.eulerAngles.x < 90 && rotOriLast.x > 270)
                rotOriLoopCnt.x += 1;
            else if (TargetBodyOri.transform.eulerAngles.x > 270 && rotOriLast.x < 90)
                rotOriLoopCnt.x -= 1;
            if (TargetBodyOri.transform.eulerAngles.y < 90 && rotOriLast.y > 270)
                rotOriLoopCnt.y += 1;
            else if (TargetBodyOri.transform.eulerAngles.y > 270 && rotOriLast.y < 90)
                rotOriLoopCnt.y -= 1;
            if (TargetBodyOri.transform.eulerAngles.z < 90 && rotOriLast.z > 270)
                rotOriLoopCnt.z += 1;
            else if(TargetBodyOri.transform.eulerAngles.z > 270 && rotOriLast.z < 90)
                rotOriLoopCnt.z -= 1;
            rotOriAvg.x = f_sfOri * (TargetBodyOri.transform.eulerAngles.x + 360 * rotOriLoopCnt.x) + (1 - f_sfOri) * rotOriAvg.x;
            rotOriAvg.y = f_sfOri * (TargetBodyOri.transform.eulerAngles.y + 360 * rotOriLoopCnt.y) + (1 - f_sfOri) * rotOriAvg.y;
            rotOriAvg.z = f_sfOri * (TargetBodyOri.transform.eulerAngles.z + 360 * rotOriLoopCnt.z) + (1 - f_sfOri) * rotOriAvg.z;
        }

        if (!stopOriFeed) {
            if (useOriAvg)
                TargetBody.transform.rotation = Quaternion.Euler(rotOriAvg);
            else
                TargetBody.transform.rotation = TargetBodyOri.transform.rotation;
        }

        if (Input.GetMouseButtonDown(0)) {
            //GetComponent<ExportFile>().StreamOutListToFile(i_dbgRotLst, o_dbgRotLst);
            //i_dbgRotLst.Clear();
            //o_dbgRotLst.Clear();
        }
    }
}
