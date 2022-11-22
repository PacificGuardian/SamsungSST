using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BscM{
    public static bool Idling = false;
    [SerializeField]
    GameObject target;
    [SerializeField]
    GameObject slave;
    Vector3 distance;
    Vector3 tempRot;
    public static bool slaveReady = false;
    public bool intIdling = false;
    private void Start() {
        slave = VarManager.Singleton.possessedDemon.gameObject;
        SlaveBase.idleEnable += idleStart;
        SlaveBase.idleDisable += idleKill;
    }
    private void idleStart(){
        if(slaveReady){
        Idling = true;
        StartCoroutine(Stroller());
        intIdling = true;
        }
    }
    private void idleKill(){
        Idling = false;
    }
    IEnumerator Stroller(){
        while(Idling){
            distance = target.transform.position - slave.transform.position;
            tempRot = new(0, 0, 0);
            if(distance.sqrMagnitude > 0.01f)
            {
            slave.GetComponent<Animator>().SetBool("Walking", true);
            slave.transform.position = Vector3.MoveTowards(slave.transform.position, target.transform.position, 0.05f);
            tempRot.y = Quaternion.LookRotation(distance.normalized).eulerAngles.y;
            slave.transform.eulerAngles = tempRot;
            }
            else{
            slave.GetComponent<Animator>().SetBool("Walking", false);
            tempRot.y = Quaternion.LookRotation((VarManager.Singleton.Robot.transform.position - slave.transform.position).normalized).eulerAngles.y;
            slave.transform.eulerAngles = tempRot;
            }
            yield return new WaitForSeconds(0.01f);
        }
        intIdling = false;
        yield return null;
    }
}