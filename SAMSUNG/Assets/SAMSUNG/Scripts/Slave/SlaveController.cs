using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveController : SlaveBase {
    [SerializeField]
    GameObject Robot;    
    [SerializeField]
    GameObject PockyCase;
    [SerializeField]
    GameObject ColaCase;
    Collider pockyCase;
    Collider colaCase;
    public static bool responcePending = false;
    private void Awake() {
        MarketTrigs.MarketStart += Welcome;
        Triggers.CaseProc += TriggerHandler;
    }
    #region Whales
    private void Welcome(){
        StartCoroutine(Whalecome());
    }
    private void Q(){
        StartCoroutine(Question());
    }
    private void A(){
        StartCoroutine(Respond());
    }
    private void S(){
        StartCoroutine(Suggestion());
    }
    private void B(){
        StartCoroutine(Goodbye());
    }    
    private void I(){
        StartCoroutine(IceSuggestion());
    }
    #endregion
    internal override void Start() {
        base.Start();
        VarManager.animCall += animCall;
        MarketTrigs.MarketExit += B;
        try{
        if(!PockyCase.GetComponent<Collider>().isTrigger || !ColaCase.GetComponent<Collider>().isTrigger)
        Application.Quit();
        else{
            ColaCase.AddComponent<Triggers>().Type = "Cola";
            PockyCase.AddComponent<Triggers>().Type = "Pocky";

        }
        } catch(NullReferenceException){
            Debug.Log("Gameobject inserted does not have a Collider/ Collider is not a trigger");
        }
    }
    private void TriggerHandler(string Type){
        if(Type == "Cola")
        Debug.Log("Cola");
        if(Type == "Pocky")
        Debug.Log("Pocky");
    }
    private void animCall(string Action){
        if(Idle.slaveReady)
        switch(Action){
            case "Pocky":
            Q();
            responcePending = true;
            break;
            case "A":
            A();
            responcePending = false;
            break;
            case "Cola":
            S();
            break;
            default:
            Debug.Log("LDance");
            break;
            case "Ice":
            I();
            break;
            case "Bye":
            B();
            break;
        }
    }
}


