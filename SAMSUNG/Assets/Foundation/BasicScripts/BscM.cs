using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BscM : MonoBehaviour
{
    protected virtual void MoveToRet(){
        Debug.Log("Stopped");
    }
    #region Expo
    public virtual void ExpoMoveTo(GameObject A, GameObject B, float Magnitude){
        StartCoroutine(ExpoMove(A, B.transform.position, Magnitude));
    }    
    public virtual void ExpoMoveTo(GameObject A, Vector3 B, float Magnitude){
        StartCoroutine(ExpoMove(A, B, Magnitude));
    }
    internal virtual IEnumerator ExpoMove(GameObject A, Vector3 B, float Magnitude){
        while(Vector3.Distance(A.transform.position, B) >= 0.001f){
            A.transform.position = Vector3.MoveTowards(A.transform.position, B, Magnitude);
            Magnitude *= 1.01f;
            yield return new WaitForSeconds(0.01f);
        }
        MoveToRet();
        yield return null;
    }
    #endregion
    #region StaticMove
    public virtual void SttcMoveTo(GameObject A, GameObject B, float Magnitude){
        StartCoroutine(SttcMove(A, B.transform.position, Magnitude));
    }
    public virtual void SttcMoveTo(GameObject A, Vector3 B, float Magnitude){
        StartCoroutine(SttcMove(A, B, Magnitude));
    }
    internal virtual IEnumerator SttcMove(GameObject A, Vector3 B, float Magnitude){
        while(Vector3.Distance(A.transform.position, B) >= 0.001f){
            A.transform.position = Vector3.MoveTowards(A.transform.position, B, Magnitude);
            yield return new WaitForSeconds(0.01f);
        }
        MoveToRet();
        yield return null;
    }
    #endregion
    internal virtual IEnumerator Rotate(GameObject A, Quaternion Goal, float Duration){
        Quaternion temp = A.transform.localRotation;
        float rotAmount = 0;
        while(Quaternion.Angle(A.transform.localRotation, Goal) > 0.001f){
            A.transform.localRotation = Quaternion.Slerp(temp, Goal, rotAmount);
            rotAmount += Duration/100/Duration;
            yield return new WaitForSeconds(0.01f * Duration);
        }
        MoveToRet();
        yield return null;
    }
    internal virtual IEnumerator Rotate(GameObject A, GameObject B, float Duration){
        Quaternion temp = A.transform.rotation;
        Quaternion Goal = Quaternion.LookRotation((B.transform.position - A.transform.position).normalized);
        float rotAmount = 0;
        while(Quaternion.Angle(A.transform.rotation, Goal) > 0.001f){
            A.transform.rotation = Quaternion.Slerp(temp, Goal, rotAmount);
            rotAmount += 0.01f;
            yield return new WaitForSeconds(0.01f * Duration);
        }
        MoveToRet();
        yield return null;
    }
    internal virtual IEnumerator Rotate(GameObject A, GameObject B, float Duration, bool End){
        Quaternion temp = A.transform.rotation;
        Quaternion Goal = Quaternion.LookRotation((B.transform.position - A.transform.position).normalized);
        Goal = Quaternion.Euler(0, Goal.eulerAngles.y, 0);
        float rotAmount = 0;
        while(Quaternion.Angle(A.transform.rotation, Goal) > 0.001f){
            A.transform.rotation = Quaternion.Slerp(temp, Goal, rotAmount);
            rotAmount += 0.01f;
            yield return new WaitForSeconds(0.01f * Duration);
        }
        if(End){
            VarManager.Singleton.possessedDemon.SetBool("Walking", false);
            Gazer.startReady = true;
        }
        MoveToRet();
        yield return null;
    }
}
