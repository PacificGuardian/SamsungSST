using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class GazeTester : SteamVR_GazeTracker{
    public override void OnGazeOn(GazeEventArgs gazeEventArgs)
    {
        base.OnGazeOn(gazeEventArgs);
        Debug.Log("IN");
    }
    public override void OnGazeOff(GazeEventArgs gazeEventArgs)
    {
        base.OnGazeOff(gazeEventArgs);
        Debug.Log("Out");
    }
}
