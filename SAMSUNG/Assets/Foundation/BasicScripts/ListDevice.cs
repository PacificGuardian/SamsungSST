using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Text;

public class ListDevice : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < SteamVR.connected.Length; ++i)
        {
            ETrackedPropertyError error = new ETrackedPropertyError();
            StringBuilder sb = new StringBuilder();
            //OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
            //string SerialNumber = sb.ToString();

            OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_ModelNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
            var ModelNumber = sb.ToString();
            if(ModelNumber.Length > 0) {
                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                string SerialNumber = sb.ToString();
                if (SerialNumber.Length > 0 || ModelNumber.Length > 0)
                    Debug.Log("Device " + i.ToString() + " = " + SerialNumber + " | " + ModelNumber); 
                //Debug.Log("Device " + i.ToString() + " = " + ModelNumber);
            }
            /*if (SerialNumber.Length > 0 || ModelNumber.Length > 0)
                Debug.Log("Device " + i.ToString() + " = " + SerialNumber + " | " + ModelNumber);*/
        }
    }
}
