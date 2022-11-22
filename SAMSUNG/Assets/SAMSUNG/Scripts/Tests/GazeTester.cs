using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;

public class GazeTester : SteamVR_GazeTracker{
    [SerializeField]
    Image imgGaze;
    bool vrStatus;
    float vrTimer;
    bool Active = false;
    public float time = 2;
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
    private void Activate(){
        if(!Active)
        {
            Active = true;
            Debug.Log("Activate");
        }        
    }
}
