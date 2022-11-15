using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTrigs : MonoBehaviour
{
    public static event MarketTriggers MarketStart;
    public static event MarketTriggers MarketExit;
    private VarManager varManager;
    private void Awake() {

    }
    private void Start() {
        varManager = VarManager.Singleton;
        //MarketStart.Invoke();
        //Speech + description
        //add languageseleced to butt
    }

    public void LanguageSelected(){
        AppliStart();
    }
    
    private void AppliStart(){
        varManager.StartCanvas.enabled = false;
        varManager.GUICanvas.enabled = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            MarketExit.Invoke();
        }
    }
}
