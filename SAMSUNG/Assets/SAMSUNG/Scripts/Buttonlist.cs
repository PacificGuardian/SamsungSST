using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Buttonlist : MonoBehaviour {
    [SerializeField]
    int stage = 0;
    VarManager varManager = null;
    private void Start() {
        varManager = VarManager.Singleton;
    }
    /*
    List of actions:
    >"How can I help you?"
    
    >" This shit was made in China."

    >Add pocky to item list
    
    >"Cum in your ass is currently on sale! Would you like one?"

    >Add cream up his ass;

    >"Our big fat giant cocks are on a buy 2 get one free sale! Would you like one up your ass?"

    >Add cocks

    >Exit
    */
    private void Update() {
        if(Input.anyKeyDown && Idle.slaveReady && stage < 9){
            switch(stage){
                case 0:
                varManager.AnimCall("Pocky");
                break;
                case 1:
                varManager.AnimCall("A");
                break;
                case 2:
                VarManager.AddItem("Pocky");
                break;
                case 3:
                varManager.AnimCall("Ice");
                break;
                case 4:
                VarManager.AddItem("Ice");
                break;
                case 5:
                varManager.AnimCall("Cola");
                break;
                case 6:
                VarManager.AddItem("Coke", 3);
                break;
                case 7:
                VarManager.Checkout();
                varManager.GUICanvas.enabled = false;
                break;
                case 8:
                varManager.AnimCall("Bye");
                break;
            }
            Debug.Log("Stage " + stage);
            stage++;
            Debug.Log("Fuck");
        }
    }


}