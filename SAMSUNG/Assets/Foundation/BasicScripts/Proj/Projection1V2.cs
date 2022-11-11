using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection1V2 : MonoBehaviour
{
    public GameObject CamTruePos;
    public GameObject OppTruePos;
    

 
    [Range(-180f, 180f)]
    public float RotationY = 0f;
    /*

    
    [Range(-180f, 180f)]
    public float RotationX = 0f;
   
    [Range(-180f, 180f)]
    public float RotationZ = 0f;
    */
    public int avg_len = 10;
    Vector3 Cam;
    Vector3 Opp;
    Vector3 Proj;
    Vector3 rotAvg; Vector3 posAvg;
    List<Vector3> lstRot = new List<Vector3>();
    List<Vector3> lstPos = new List<Vector3>();
    int r_cnt = 0;

    void Update()
    {
        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = (Opp - Cam);

        lstRot.Add(CamTruePos.transform.eulerAngles);
        lstPos.Add(Proj);
        if (r_cnt == 0)
        {
            posAvg = Proj;
            rotAvg = CamTruePos.transform.eulerAngles;
        }
        else
        {
            posAvg += Proj;
            rotAvg += CamTruePos.transform.eulerAngles;
        }
        if (r_cnt >= avg_len)
        {
            rotAvg -= lstRot[0];
            lstRot.RemoveAt(0);
            posAvg -= lstPos[0];
            lstPos.RemoveAt(0);
        }
        else
            r_cnt++;
        /*
        
        pP = Quaternion.Euler(0, RotationY - rR.y, 0) * pP * Multiplier;
        print("rR:" + rR + " pos:" + pP);
        transform.position = pP;
        transform.eulerAngles = OppTruePos.transform.eulerAngles;
        transform.Rotate(0, 0, RotationY - rR.y);
        */
        Vector3 rR = rotAvg / r_cnt;
        Vector3 pP = posAvg / r_cnt;
        
        pP = Quaternion.Euler(0, RotationY - rR.y, 0) * pP ;
        print("rR:" + rR + " pos:" + pP);
        transform.localPosition = pP;
        transform.eulerAngles = OppTruePos.transform.eulerAngles;
        transform.Rotate(0, 0, RotationY - rR.y);
    }


}
