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