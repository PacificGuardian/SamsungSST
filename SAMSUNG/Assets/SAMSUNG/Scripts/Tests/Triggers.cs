using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour {
    public static MarketTrigger CaseProc;
    public string Type = "";
    private void OnTriggerEnter() {
        //CaseProc.Invoke(Type);
        VarManager.Singleton.AnimCall(Type);
    }

}