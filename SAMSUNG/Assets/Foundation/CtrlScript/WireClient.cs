using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class WireClient : MonoBehaviour
{
    [SerializeField] float delay = 0.25f;
    public static SerialController wired;
    ViveControl vive;
    float time = 0;
    static public bool newbuttonstate = false;
    float newstop;

    bool lastP1 =false;
    bool lastP2 =false;
    bool lastP3 =false;
    bool lastP4 =false;
    //bool tempHandState = false;
    
    // Use this for initialization
    void Start()
    {
        wired = GameObject.Find("SerialController").GetComponent<SerialController>();
        vive = GetComponent<ViveControl>();
        wired.SendSerialMessage("d");
    }


    // Update is called once per frame
    void Update()
    {
       time += Time.deltaTime;
       if ( vive && time >= delay)
       { 
            HandleControl();
            time = 0;
       }
    }

    public void stop()
    {
        wired.SendSerialMessage("0");
        Debug.Log("sent");
    }

    public void HandleControl()
    {
        if (vive.P1 == 255 && vive.P2 == 255 && lastP1 == false)
        {
            lastP1 =true;
            lastP2 =false;
            lastP3 =false;
            lastP4 =false;
            wired.SendSerialMessage("1" + vive.P1.ToString());
            //print("1" + vive.P1);
        }
        if (vive.P3 == 255 && vive.P4 == 255 && lastP3 == false)
        {
            lastP1 =false;
            lastP2 =false;
            lastP3 =true;
            lastP4 =false;
            wired.SendSerialMessage("2"+vive.P3.ToString());
            //print("2" + vive.P3);
        }
        if (vive.P3 == 255 && vive.P2 == 255 && lastP2 == false)
        {
            lastP1 =false;
            lastP2 =true;
            lastP3 =false;
            lastP4 =false;
            wired.SendSerialMessage("3"+vive.P3.ToString());
            //print("3" + vive.P3);
        }
        if (vive.P4 == 255 && vive.P1 == 255 && lastP4 == false)
        {
            lastP1 =false;
            lastP2 =false;
            lastP3 =false;
            lastP4 =true;
            wired.SendSerialMessage("4"+vive.P1.ToString());
            //print("4" + vive.P1);
        }
        
        if (vive.P1 == 255 && vive.P2 == 0 && vive.P3 == 0 && vive.P4 == 0)
        {
            wired.SendSerialMessage("5" + vive.P1.ToString());
        }
        if (vive.P2 == 255 && vive.P1 == 0 && vive.P3 == 0 && vive.P4 == 0)
        {
            wired.SendSerialMessage("6" + vive.P2.ToString());
        }
        if (vive.P3 == 255 && vive.P1 == 0 && vive.P2 == 0 && vive.P4 == 0)
        {
            wired.SendSerialMessage("7" + vive.P3.ToString());
        }
        if (vive.P4 == 255 && vive.P1 == 0 && vive.P2 == 0 && vive.P3 == 0)
        {
            wired.SendSerialMessage("8" + vive.P4.ToString());
        }
        
        if ( vive.P1 == 0 && vive.P2 == 0 && vive.P3 == 0 && vive.P4 == 0)
        {
            lastP1 =false;
            lastP2 =false;
            lastP3 =false;
            lastP4 =false;
            wired.SendSerialMessage("0");
            //print("0");
        }
        
        if (vive.HandState == true && newbuttonstate != vive.HandState)
        {
            wired.SendSerialMessage("u");
            newbuttonstate = vive.HandState;
            //print("hands up");
        }
        if (vive.HandState == false && newbuttonstate != vive.HandState)
        {
            wired.SendSerialMessage("d");
            newbuttonstate = vive.HandState;
            //print("hands down");
        }
        /*
        bool tempBool = RobotAttacher.HandsUp;
        if(tempHandState != tempBool){
            if(tempBool){
            wired.SendSerialMessage("u");
            Debug.Log("Up");
            }
            else if(!tempBool){
            wired.SendSerialMessage("d");
            Debug.Log("Down");
            }
            tempHandState = RobotAttacher.HandsUp;
            */
        
    }
}

