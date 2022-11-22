using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour{
    [SerializeField]
    GameObject PockyCase;
    [SerializeField]
    GameObject ColaCase;
    Collider pockyCase;
    Collider colaCase;
    void Start() {
        try{
        if(!PockyCase.GetComponent<Collider>().isTrigger || !ColaCase.GetComponent<Collider>().isTrigger)
        Application.Quit();
        else{
            ColaCase.AddComponent<Triggers>().Type = "Cola";
            PockyCase.AddComponent<Triggers>().Type = "Pocky";
            Triggers.CaseProc += TriggerHandler;
        }
        } catch(NullReferenceException){
            Debug.Log("Gameobject inserted does not have a Collider/ Collider is not a trigger");
        }
    }
    private void TriggerHandler(string Type){
        //Debug.Log("Cum");
        if(Type == "Cola")
        Debug.Log("Cola");
        if(Type == "Pocky")
        Debug.Log("Pocky");
    }
}