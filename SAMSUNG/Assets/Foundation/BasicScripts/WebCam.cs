using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    public int CamNumber;
    public GameObject cube;
    public bool EnableOnStartup = true;
    private void Start(){
        if(EnableOnStartup)
        StartCamera();
    }
    public void StartCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log("Number of web cams connected: " + devices.Length);

        for (int i = 0; i < devices.Length; i++)
            Debug.Log(i + " " + devices[i].name);

        WebCamTexture mycam = new WebCamTexture();
        string camName = "NULL";
        if (CamNumber < devices.Length)
            camName = devices[CamNumber].name;
        if(camName == "NULL")
            Debug.Log("<color=red>Error: </color>CamNumber is not set in range of number of devices.");
        else{
            Debug.Log("The webcam name is " + camName);
        mycam.deviceName = camName;
        //mycam.requestedFPS = 30;
        //img.texture = mycam;
        cube.GetComponent<Renderer>().material.mainTexture = mycam;

        mycam.Play();
        }
    }
}
