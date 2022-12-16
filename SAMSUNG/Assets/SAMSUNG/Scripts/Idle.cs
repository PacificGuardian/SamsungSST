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
    Vector3 pos;
    public static bool slaveReady = false;
    public bool intIdling = false;
    [SerializeField]
    float walkSpeed;
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
            if(distance.sqrMagnitude > 0.5f)
            {
            slave.GetComponent<Animator>().SetBool("Walking", true);
            pos = new Vector3(target.transform.position.x,slave.transform.position.y,target.transform.position.z);
            slave.transform.position = Vector3.MoveTowards(slave.transform.position, target.transform.position, 0.05f * walkSpeed);
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