using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageCfg : MonoBehaviour{
    #region Setup
    Image imgGaze;
    float time;
    int distanceOfRay;
    bool vrStatus;
    float vrTimer;
    private RaycastHit _ooo;
    private void Start() {
        imgGaze = sceneCfg.ImgGaze;
        time = sceneCfg.Time;
        distanceOfRay = sceneCfg.rayDistance;
    }
    #endregion
    public void vrOn(){
        vrStatus = true;
        Debug.Log("Ok");
    }
    public void vrOff(){
        vrStatus = false;
        vrTimer = 0;
        imgGaze.fillAmount = 0;
    }
    private void Update() {
        if(vrStatus){
            vrTimer += Time.deltaTime;
            imgGaze.fillAmount = vrTimer / time;
        }
        if(vrTimer > time)
        Activate();
    }
    private void Activate(){
        Debug.Log("Active");
    }
}