using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection9_1 : MonoBehaviour
{
    public GameObject CamTruePos;
    public GameObject CamRot;
    public GameObject OppTruePos;
    public GameObject TargetBody;
    public GameObject TargetBodyOri;
    public GameObject Field_rotation_1;
    public GameObject Field_rotation_2;
    public GameObject TheField;

    public bool stopBodyHTrack = false;
    public bool stopPosFeed = false;
    public bool stopOriFeed = false;
    public bool usePosDly = true;
    public bool useOriDly = true;

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
    Vector3 Cam;
    Vector3 Opp;
    Vector3 Proj;

    private List<Vector3> posLst = new List<Vector3>();
    private List<Quaternion> oriLst = new List<Quaternion>();

// shock
    private float fixedDeltaTime;
    [Range(15,30)]//0.02 -- 0.01 fixedDeltaTime
    public int delayFrame = 15;
    [Range(0.2f, 0.3f)]
    public float camdelay = 0.27695f;

//only for see and calculate
    [HeaderAttribute("only for See and Calculate")]
    public float angle;
    public float rob_angle_True;
    public float rob_angle;
    public float distance;
    public float angle_changed;
    public float high_changed;
    public float high_change_2;
    public float angle_changed_2;

//
    [HeaderAttribute("Can be changed")]
    public float high_change = 4.75f;
    public float angle_error = 0;
//
    [HeaderAttribute("Select the function")]
    public bool use_fix_1 = true;
    public bool use_fix_1_1 = true;

    void Start()
    {
        fixedDeltaTime = camdelay / delayFrame;
        if (!stopBodyHTrack)
            TargetBody.transform.localPosition = new Vector3(0, 0, CamTrackerDist / VORORatio);
    }

    void Update()
    {
        Time.fixedDeltaTime = fixedDeltaTime;
        if (!stopBodyHTrack && LockBodyHeight)
            TargetBody.transform.localPosition = new Vector3(0, 0, CamTrackerDist / VORORatio);
    }

    void FixedUpdate()
    {
        //Calculate position
        Cam = CamTruePos.transform.position;
        Opp = OppTruePos.transform.position;
        Proj = (Opp - Cam);

        //Input cam angle
        Quaternion qr; qr.x = CamRot.transform.localRotation.x; qr.y = CamRot.transform.localRotation.y; qr.z = CamRot.transform.localRotation.z; qr.w = CamRot.transform.localRotation.w;
        Vector3 pP = Quaternion.Euler(RotationX - qr.eulerAngles.x, RotationZ - qr.eulerAngles.z, qr.eulerAngles.y - RotationY) * Proj * Multiplier;

        //Calculate orientation
        TargetBodyOri.transform.rotation = OppTruePos.transform.rotation; //Local rotation
        TargetBodyOri.transform.Rotate(RotationX - qr.eulerAngles.x, RotationZ - qr.eulerAngles.z, qr.eulerAngles.y - RotationY, Space.World);

        posLst.Add(pP);
        oriLst.Add(TargetBodyOri.transform.rotation);
        if (delayFrame == 0)
        {
            posLst.RemoveAt(0);
            oriLst.RemoveAt(0);
        }
        else
            delayFrame--;
        if (!stopPosFeed)
        {
            if (usePosDly && posLst.Count > 0)
                transform.position = posLst[0];
            else
                transform.position = pP;
        }

        if (!stopOriFeed)
        {
            if (useOriDly && oriLst.Count > 0)
                TargetBody.transform.rotation = oriLst[0];
            else
                TargetBody.transform.rotation = TargetBodyOri.transform.rotation;
        }

        //calculate the float for seeing
        angle = (Mathf.Atan2(Proj.x, Proj.z) * Mathf.Rad2Deg);
        rob_angle_True = CamRot.transform.localRotation.eulerAngles.z;
        rob_angle = (rob_angle_True - angle+360)%360;
        distance =Mathf.Sqrt( Mathf.Pow(Proj.x,2)+Mathf.Pow(Proj.y,2)+Mathf.Pow(Proj.z,2));

        //using fix
        if(use_fix_1 && !use_fix_1_1)
           Fix_1();
        if(use_fix_1_1 && !use_fix_1)
           Fix_1_1();
        if(use_fix_1_1 && use_fix_1)
            Fix_1();   
    }
    private void Fix_1()//default
    {
        //distance error(the back of the 360 camera)
        high_change_2 = ((1.58f)*distance)+(0.39f);

        //calculate high
        if(rob_angle < 120 && rob_angle > 60)
        {
            high_changed = high_change+high_change_2*Mathf.Abs(((rob_angle-60)/60)-1);//0-1
        }
        else if(rob_angle < 300 && rob_angle > 240)
        {
            high_changed = high_change+high_change_2*((rob_angle-240)/60);//0-1
        }
        else if(rob_angle > 120 && rob_angle < 240)
        {
            high_changed = high_change;
        }
        else//>300  <60
        {
            high_changed = high_change +high_change_2;
        }

        //angle about the field(axis is the player)
        if(rob_angle < 120 && rob_angle > 0)
        {
            angle_changed = 3f*Mathf.Abs(Mathf.Abs((rob_angle-60)/60)-1)+angle_error;//0-1-0
            angle_changed_2 = -3f*Mathf.Abs((rob_angle/120)-1);//0-1
        }
        else if
        (
            rob_angle < 360 && rob_angle > 240){
            angle_changed = -3f*Mathf.Abs(Mathf.Abs((rob_angle-300)/60)-1)+angle_error;//0-1-0
            angle_changed_2 = -3f*((rob_angle-240)/120);//0-1
        }
        else
        {
            angle_changed = 0 + angle_error;
            angle_changed_2 = 0;
        }

        //set the number(flaot) to the game object
        Field_rotation_1.transform.position = CamTruePos.transform.position;
        Field_rotation_1.transform.localRotation = Quaternion.Euler(Field_rotation_1.transform.localRotation.eulerAngles.x,Field_rotation_1.transform.localRotation.eulerAngles.y,angle+90f);
        Field_rotation_2.transform.localRotation = Quaternion.Euler(angle_changed,angle_changed_2 ,Field_rotation_2.transform.localRotation.eulerAngles.z);
        TheField.transform.position = new Vector3(transform.position.x,transform.position.y+high_changed,transform.position.z);
        TheField.transform.localRotation = Quaternion.Euler(0,0,-(angle+90f));
    }
    private void Fix_1_1()//custom
    {
        //distance error(the back of the 360 camera)
        high_change_2 = ((1.58f)*distance)+(0.39f);

        //calculate high
        if(rob_angle < 120 && rob_angle > 30)
        {
            high_changed = high_change+high_change_2*Mathf.Abs(((rob_angle-30)/90)-1);//0-1
        }
        else if(rob_angle < 300 && rob_angle > 240)
        {
            high_changed = high_change+high_change_2*((rob_angle-240)/60);//0-1
        }
        else if(rob_angle > 120 && rob_angle < 240)
        {
            high_changed = high_change;
        }
        else//>300  <30
        {
            high_changed = high_change +high_change_2;
        }

        //angle about the field(axis is the player)
        if(rob_angle < 120 && rob_angle > 0)
        {
            angle_changed = 3.5f*Mathf.Abs(Mathf.Abs((rob_angle-60)/60)-1)+angle_error;//0-1-0
            angle_changed_2 = -3.5f*Mathf.Abs((rob_angle/120)-1);//0-1
        }
        else if
        (
            rob_angle < 360 && rob_angle > 240){
            angle_changed = -3.5f*Mathf.Abs(Mathf.Abs((rob_angle-300)/60)-1)+angle_error;//0-1-0
            angle_changed_2 = -3.5f*((rob_angle-240)/120);//0-1
        }
        else
        {
            angle_changed = 0 + angle_error;
            angle_changed_2 = 0;
        }

        //set the number(flaot) to the game object
        Field_rotation_1.transform.position = CamTruePos.transform.position;
        Field_rotation_1.transform.localRotation = Quaternion.Euler(Field_rotation_1.transform.localRotation.eulerAngles.x,Field_rotation_1.transform.localRotation.eulerAngles.y,angle+90f);
        Field_rotation_2.transform.localRotation = Quaternion.Euler(angle_changed,angle_changed_2 ,Field_rotation_2.transform.localRotation.eulerAngles.z);
        TheField.transform.position = new Vector3(transform.position.x,transform.position.y+high_changed,transform.position.z);
        TheField.transform.localRotation = Quaternion.Euler(0,0,-(angle+90f));
    }
}