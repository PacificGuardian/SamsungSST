using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketTrigs : MonoBehaviour
{
    public static event MarketTriggers MarketStart;
    public static event MarketTriggers MarketExit;
    private static VarManager varManager;
    [SerializeField]
    GameObject PockyCase;
    [SerializeField]
    GameObject ColaCase;
    Collider pockyCase;
    Collider colaCase;
    static TextMeshProUGUI totalText;
    private void Start() {
        varManager = VarManager.Singleton;
        //triggersSetup();
        VarManager.itemUpdate += updateCanvas;
    }    
    public static void AppliStart(){
        varManager.StartCanvas.enabled = false;
        MarketStart.Invoke();
        varManager.GUICanvas.enabled = true;
        Idle.slaveReady = false;
        totalText = GameObject.Find("total").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            MarketExit.Invoke();
        }
    }
    private void triggersSetup(){
        try{
        if(!PockyCase.GetComponent<Collider>().isTrigger || !ColaCase.GetComponent<Collider>().isTrigger)
        Application.Quit();
        else{
            ColaCase.AddComponent<Triggers>().Type = "Cola";
            PockyCase.AddComponent<Triggers>().Type = "Pocky";
        }
        } catch(NullReferenceException){
            Debug.Log("Gameobject inserted does not have a Collider");
        }
    }
    /*
    private void Update() {
        if(Input.anyKeyDown && SlaveController.responcePending){
            varManager.AnimCall("A");
            Debug.Log("Answer");
        }
    }
    */
    private void updateCanvas(){
        totalText.text = "Total items: " + varManager.totalItems();
    }
}
