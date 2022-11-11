using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingCube : MonoBehaviour
{
    private GameObject CamTruePos;
    private GameObject OppTruePos;

    private float Distance, MagCam, MagOpp, ScaleCap;
    private Vector3 ScalingV;
    [Range(10f, 30f)]
    public float Divisor = 25;

    // Start is called before the first frame update
    void Start()
    {
        ScaleCap = 2;
        CamTruePos = GameObject.FindWithTag("Camera");
        OppTruePos = GameObject.FindWithTag("Opponent");
    }

    // Update is called once per frame
    void Update()
    {



        MagCam = CamTruePos.transform.position.magnitude;
        MagOpp = OppTruePos.transform.position.magnitude;

        Distance = Mathf.Abs(MagOpp) - Mathf.Abs(MagCam);

      //  print(Distance);
        
        //Distance = Distance/Divisor;

        
        Distance = ScaleCap - Distance;
        Distance /= Divisor;
        if(Mathf.Abs(Distance) > ScaleCap || Distance < 0.0002f)
        {
            print("Distance too great!");
        }
        else
        {
            ScalingV.Set(Distance, Distance, Distance);
            print(ScalingV);
            transform.localScale = ScalingV;
        
        }
        
        
        
        
        
    }
}
