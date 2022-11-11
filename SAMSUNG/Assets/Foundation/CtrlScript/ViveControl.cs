using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveControl : MonoBehaviour
{
    public SteamVR_Action_Vector2 TouchPad;
    public SteamVR_Action_Boolean Servo;
    public float P1;
    public float P2;
    public float P3;
    public float P4;
    public bool HandState;
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        bool Hand = Servo.GetState(SteamVR_Input_Sources.Any);
        Vector2 TouchPadValue = TouchPad.GetAxis(SteamVR_Input_Sources.Any);
        if (TouchPadValue != Vector2.zero)
        {
            positsion(xy(TouchPadValue));
            P1 = pwm((int)P1);
            P2 = pwm((int)P2);
            P3 = pwm((int)P3);
            P4 = pwm((int)P4);
            //Debug.Log(TouchPadValue+"Output: "+P1);
        }
        else
        {
            P1 = 0;
            P2 = 0;
            P3 = 0;
            P4 = 0;
        }
        if (Hand == true)
        {
            HandState = true;
            //print(HandState);
        }
        else
        {
            HandState = false;
            //print(HandState);
        }
        if (time >= 0.5)
        {
            //print("P1: " + P1 + " P2: " + P2 + " P3: " + P3 + " P4: " + P4);
        }
    }

    private Vector2 xy(Vector2 position)
    {
        float x;
        float y;
        y = Mathf.Sqrt(Mathf.Pow(position.x, 2) + Mathf.Pow(position.y, 2)) * 255;
        if (y < 50)
            y = 0;
        x = Mathf.Atan2(position.x, position.y)* Mathf.Rad2Deg;
        if (x < 0)
            x = 360 + x;
        Vector2 processed = new Vector2(x, y);
        //print("vx:" + position.x + " vy:" + position.y + " r:" + y + " theta:" + x);
        return processed;
        
    }

    public void positsion(Vector2 pos)
    {
        float x = pos.x;
        float y = pos.y;

        if (x >= 0 && x <= 90)
        {
            P1 = y;
            P2 = y * ((90 - x) / 90);
            P3 = 0;
            P4 = y * ((x - 0) / 90);
        }
        else if (x >= 90 && x <= 180)
        {
            P1 = y * ((180 - x) / 90);
            P2 = 0;
            P3 = y * ((x - 90) / 90);
            P4 = y;
        }
        else if (x >= 180 && x <= 270)
        {
            P1 = 0;
            P2 = y * ((x - 180) / 90);
            P3 = y;
            P4 = y * ((270 - x) / 90);

        }
        else if (x >= 270 && x <= 360)
        {
            P1 = y * ((x - 270) / 90);
            P2 = y;
            P3 = y * ((360 - x) / 90);
            P4 = 0;
        }

        //Debug.Log("P1:" + P1 + " P2:" + P2 + " P3:" + P3 + " P4:" + P4 + " r:" + y + " theta:" + x);
    }

    public int pwm(int x)
    {
        if(x >= 127)
        {
            x = 255;
        }
        else
        {
            x = 0;
        }
        return (x);
    }
}
