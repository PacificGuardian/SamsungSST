using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTest : BscM{
    public static bool Idling = false;
    [SerializeField]
    GameObject target;
    [SerializeField]
    GameObject slave = null;
    Vector3 distance;
    Vector3 tempRot;
    private void Awake() {
        slave = slave == null? VarManager.Singleton.possessedDemon.gameObject : slave;
        //SlaveBase.idleEnable += idleStart;
        //SlaveBase.idleDisable += idleKill;
    }
    private void Start() {
        Idling = true;
        StartCoroutine(Stroller());
    }
    private void idleStart(){
        Idling = true;
        StartCoroutine(Stroller());
    }
    private void idleKill(){
        Idling = false;
    }
    IEnumerator Stroller(){
        slave.GetComponent<Animator>().SetBool("Walking", true);
        while(Idling){
            distance = target.transform.position - slave.transform.position;
            tempRot = new(0, 0, 0);
            if(distance.sqrMagnitude > 0.01f)
            slave.transform.position = Vector3.MoveTowards(slave.transform.position, target.transform.position, 0.05f);
            tempRot.y = Quaternion.LookRotation(distance.normalized).eulerAngles.y;
            slave.transform.eulerAngles = tempRot;
            yield return new WaitForSeconds(0.01f);
        }
        slave.GetComponent<Animator>().SetBool("Walking", false);
        yield return null;
    }
}