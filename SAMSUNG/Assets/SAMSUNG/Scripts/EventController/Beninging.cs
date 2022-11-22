using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beninging : BscM
{
    [SerializeField]
    float WalkSpeed = 0.05f;
    public GameObject Labour = null;
    public GameObject[] Targets;
    [SerializeField]
    private int Steps = 0;
    private void Awake() {
        MarketTrigs.MarketStart += Reg;
    }
    private void Reg(){
        try{
        GameObject controller = GameObject.Find("Serial_Controller");
        controller.SetActive(true);
        } catch (NullReferenceException){
            
        }
    }
    private void Start() {
        if(GameObject.Find("Serial_Controller") != null)
        GameObject.Find("Serial_Controller").SetActive(false);
        if(VarManager.Singleton.StartCanvas != null)
        VarManager.Singleton.StartCanvas.enabled = true;
        SttcMoveTo(Labour, Targets[Steps], WalkSpeed);
        VarManager.Singleton.possessedDemon.SetBool("Walking", true);
        GameObject.Find("Serial_Controller").SetActive(false);
    }
    internal override IEnumerator SttcMove(GameObject A, Vector3 B, float Magnitude)
    {
        while(Vector3.Distance(A.transform.position, B) >= 0.001f){
            A.transform.position = Vector3.MoveTowards(A.transform.position, B, Magnitude);
            yield return new WaitForSeconds(0.01f);
        }
        print(Steps + " Complete");
        if(Steps == Targets.Length - 1){
            StartCoroutine(Rotate(Labour, VarManager.Singleton.Robot, 1, true));
        }
        try{
        if(Targets[Steps + 1] != null){
        StartCoroutine(Rotate(Labour, Targets[Steps + 1], 1));
        }
        } catch (IndexOutOfRangeException){

        }
        yield return null;
    }
    protected override void MoveToRet()
    {
        if(Steps < Targets.Length - 1){
            Steps++;
            SttcMoveTo(Labour, Targets[Steps], WalkSpeed);
        }
    }

}
