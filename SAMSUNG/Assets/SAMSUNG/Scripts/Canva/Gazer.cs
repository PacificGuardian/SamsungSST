using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;

public class Gazer : SteamVR_GazeTracker{
    [SerializeField]
    Image imgGaze;
    bool vrStatus;
    float vrTimer;
    bool Active = false;
    public float time = 2;
    public static bool startReady = false;
    private void Start() {
        imgGaze = GameObject.FindGameObjectWithTag("Target").GetComponent<Image>();
        imgGaze.fillAmount = 0;
    }
    public override void OnGazeOn(GazeEventArgs gazeEventArgs)
    {
        base.OnGazeOn(gazeEventArgs);
        vrStatus = true;
        Debug.Log("IN");
    }
    public override void OnGazeOff(GazeEventArgs gazeEventArgs)
    {
        base.OnGazeOff(gazeEventArgs);
        vrStatus = false;
        Debug.Log("Out");
        vrTimer = 0;
        imgGaze.fillAmount = 0;
        Active = false;
    }
    
    protected override void Update() {
        base.Update();
        if(vrStatus){
            vrTimer += Time.deltaTime;
            imgGaze.fillAmount = vrTimer / time;
        }
        if(vrTimer > time)
        Activate();
    }
    public void Activate(){
        if(!Active && startReady)
        {
            Active = true;
            GameObject.Find("TargetCanvas").SetActive(false);
            MarketTrigs.AppliStart();
        }        
    }
}
