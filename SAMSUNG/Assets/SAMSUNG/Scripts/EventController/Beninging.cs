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
    private void Start() {
        if(VarManager.Singleton.StartCanvas != null)
        VarManager.Singleton.StartCanvas.enabled = true;
        SttcMoveTo(Labour, Targets[Steps], WalkSpeed);
    }
    internal override IEnumerator SttcMove(GameObject A, Vector3 B, float Magnitude)
    {
        while(Vector3.Distance(A.transform.position, B) >= 0.001f){
            A.transform.position = Vector3.MoveTowards(A.transform.position, B, Magnitude);
            yield return new WaitForSeconds(0.01f);
        }
        print(Steps + " Complete");
        Quaternion targeting;
        try{
        if(Targets[Steps + 1] != null){
        targeting = Quaternion.LookRotation(Targets[Steps + 1].transform.position, Vector3.up);
        Debug.Log(targeting);
        StartCoroutine(Rotate(Labour, targeting, 1));
        }
        } catch (IndexOutOfRangeException){

        }
        yield return null;
    }
    
    internal override IEnumerator Rotate(GameObject A, Quaternion Goal, float Duration){
        Quaternion temp = A.transform.rotation;
        float rotAmount = 0;
        while(Quaternion.Angle(A.transform.rotation, Goal) > 0.001f){
            A.transform.rotation = Quaternion.Slerp(temp, Goal, rotAmount);
            rotAmount += Duration/100;
            yield return new WaitForSeconds(0.01f);
        }
        MoveToRet();
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
