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
    [Range(1,200)]//0.02 -- 0.01 fixedDeltaTime
    public int delayFrame = 15;
    [Range(0.2f, 0.3f)]
    public float camdelay = 0.27695f;

    public float angle;
    public float calculate_angle_areaf;
    public List<int> Calculate_Classification;
    public float rob_angle_True;
    public float rob_angle;
    public List<int> X1_X2 = new List<int>();
    public List<int> Y1_Y2 = new List<int>();
    public float angle_fix = 3f;
   /* public float seen_angle = 0f;
//
    public float check_X;
    public float check_Y;
//
*/
    public bool use_fix = true;
    private float fixedDeltaTime;

    void Start()
    {
        X1_X2.Add(1);X1_X2.Add(-3);
        Y1_Y2.Add(1);Y1_Y2.Add(-1);
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
        if(use_fix)
           Fix_10();
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
        angle = (Mathf.Atan2(Proj.x, Proj.z) * Mathf.Rad2Deg);
        calculate_angle_areaf = Calculate_angle_area(angle);
        Calculate_Classification = Calculate_ClassificationXY(calculate_angle_areaf);
        rob_angle_True = CamRot.transform.localRotation.eulerAngles.z + angle_fix;
        rob_angle = (rob_angle_True - angle+360)%360;
    }
/*        private void Fix_9()
    {
        //List<float> XY = ListXY();
        //print(XY[0] +""+ XY[1]+""+XY[2] +""+ XY[3]);
        float RotationX_fix = check_X;
        float RotationY_fix = check_Y;
        float angle = (Mathf.Atan2(Proj.x, Proj.z) * Mathf.Rad2Deg);
        //float rob_angle_True = CamRot.transform.localRotation.eulerAngles.z + angle_fix;
        //float rob_angle = (rob_angle_True - angle+360)%360;
        //RotationX_fix_public = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Mathf.Sin((Mathf.PI /180)*(angle+180));//180+90//changed+90
        /////RotationX_fix_public = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,0);//180,90
        /////RotationY_fix_public = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,90);//270,180
        //RotationY_fix_public = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Mathf.Sin((Mathf.PI /180)*angle+90);//2 - Mathf.Abs(RotationX_fix_public);
        RotationX = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,0);
        //RotationY = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,90);
        /*if(rob_angle < 90 -seen_angle || rob_angle > 270 + seen_angle)
        {
            RotationX = -(RotationX_fix_public);
            RotationY = -(RotationY_fix_public);
        }
        if(rob_angle > 90 + seen_angle && rob_angle < 270 - seen_angle)
        {
            RotationX = (RotationX_fix_public);
            RotationY = (RotationY_fix_public);
        }
        if (rob_angle%180 > 90 - seen_angle && rob_angle%180 < 90 + seen_angle )
        {
            RotationX = Mathf.Max(Mathf.Abs(RotationX_fix_public),Mathf.Abs(RotationY_fix_public))*(Calculate_ra_sa(rob_angle,seen_angle,angle-270));//+270,180
            RotationY = Mathf.Max(Mathf.Abs(RotationX_fix_public),Mathf.Abs(RotationY_fix_public))*(Calculate_ra_sa(rob_angle,seen_angle,angle));//+180,90
            //RotationX = Mathf.Abs(RotationX_fix_public-RotationY_fix_public)*(Calculate_ra_sa(rob_angle,seen_angle,angle+180));
            //RotationY = Mathf.Abs(RotationX_fix_public-RotationY_fix_public)*(Calculate_ra_sa(rob_angle,seen_angle,angle+90));
            
        }
        //RotationX += 2f*Mathf.Abs(((Mathf.Abs(angle%90-45)-45)/(-45))*Mathf.Abs(rob_angle-180)/180);
        //RotationY += 2f*Mathf.Abs(((Mathf.Abs(angle%90-45)-45)/(-45))*Mathf.Abs(rob_angle-180)/180);
        print(angle+"   "+Angle_change(angle,180));
        //print(rob_angle180d + "," + CamRot.transform.localRotation.eulerAngles.z + "," + angle +", " +  direction);
    }*/
    
    private float Angle_change(float a , float ac)
    {
        float angle = a - ac;
        
        if(angle<-180)
            angle += 360;
        
        if(angle>180)
            angle -= 360;
        
        return 2*Mathf.Abs(angle)/180-1;
    }
    private void Fix_10()
    {
        float angle = (Mathf.Atan2(Proj.x, Proj.z) * Mathf.Rad2Deg);
        //RotationX_fix_public = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,0);//180,90
        //RotationY_fix_public = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,90);//270,180
        //RotationX = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,0);
        //RotationY = Mathf.Abs(Mathf.Max(check_X,check_Y)) * Angle_change(angle,90);
        float rob_angle_True = CamRot.transform.localRotation.eulerAngles.z + angle_fix;
        float rob_angle = (rob_angle_True - angle+360)%360;
        List<int> XYint = Calculate_ClassificationXY(Calculate_angle_area(angle));
        RotationX =  RotationXY_set(rob_angle,XYint[0]);
        RotationY =  RotationXY_set(rob_angle,XYint[1]);
        //print(rob_angle - 180);
        
    }
     private float Calculate_angle_area(float angle )
    {
        float step2_1;
        float step2_2;
        step2_1 = Mathf.Abs(Mathf.Floor((angle+180)/45));
        step2_2 = (Mathf.Floor(step2_1/2)+step2_1%2 + 1);
        step2_2 +=1;
        if(step2_2 >= 4)
            step2_2 -= 4;
        if(step2_2 <0)
            step2_2 += 4;
        //print(/*ra + "   " +sa+"     "+ */step2_1 +"   "+step2_2 + "     "+ a);
        return step2_2;
    }
    private List<int> Calculate_ClassificationXY(float angle_area)
    {
        int Calculate_x = 0;
        int Calculate_y = 0;//1 = hight(+)/low(-) , 2 = right(+)/left(-)
        if(angle_area == 0)
        {
            Calculate_x = 1;
            Calculate_y = 2;//2
        }
        else if(angle_area == 1)
        {
            Calculate_x = -2;//-2
            Calculate_y = 1;
        }
        else if(angle_area == 2)
        {
            Calculate_x = -1;
            Calculate_y = -2;//-2
        }
        else if(angle_area == 3)
        {
            Calculate_x = 2;//2
            Calculate_y = -1;
        }
        List<int> XYint = new List<int>();
        XYint.Add(Calculate_x);XYint.Add(Calculate_y);
        //print(XYint);
        return XYint;
    }
    private float Calculate_high_Change(float rob_angle , float x , float y)//-180<ra<180
    {
        float diff = x - y;
        return x - diff *Mathf.Abs(rob_angle - 180) / 180;
    }
    private float RotationXY_set(float rob_angle, int a ) 
    {
        if(Mathf.Abs(a) == 1)
            return Calculate_high_Change(rob_angle , (X1_X2[0]) *  a , (X1_X2[1]) * a );
        else if (Mathf.Abs(a) == 2)
            return (-a)*Angle_change(rob_angle,270);
        else
            return 0;
    }
}