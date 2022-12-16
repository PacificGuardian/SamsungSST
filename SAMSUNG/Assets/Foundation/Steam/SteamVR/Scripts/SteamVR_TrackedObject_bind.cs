//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: For controlling in-game objects with tracked devices.
//
//=============================================================================

using System.Text;
using UnityEngine;
using Valve.VR;

namespace Valve.VR
{
    public class SteamVR_TrackedObject_bind : MonoBehaviour
    {
        [Range(0,1)]public float step;
        public enum EIndex
        {
            None = -1,
            Hmd = (int)OpenVR.k_unTrackedDeviceIndex_Hmd,
            Device1,
            Device2,
            Device3,
            Device4,
            Device5,
            Device6,
            Device7,
            Device8,
            Device9,
            Device10,
            Device11,
            Device12,
            Device13,
            Device14,
            Device15,
            Device16
        }

        [HideInInspector]
        public EIndex index;

        public string DesiredSerialNumber = "";

        [Tooltip("If not set, relative to parent")]
        public Transform origin;

        //Vector3 newpose;
        
        //public float mount = 0;

        int count = 0;
        public bool pos_change = false;

        public bool isValid { get; private set;}     
        public void OnNewPoses(TrackedDevicePose_t[] poses)
        {
            if (index == EIndex.None)
                return;

            var i = (int)index;

            isValid = false;
            if (poses.Length <= i)
                return;

            if (!poses[i].bDeviceIsConnected)
                return;

            if (!poses[i].bPoseIsValid)
                return;

            isValid = true;

            var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            if (origin != null)
            {
                //print("origin update");
                transform.position = origin.transform.TransformPoint(pose.pos);
                transform.rotation = origin.rotation * pose.rot;       
            }
            else 
            {
                pos_change = true;
                change_pos(poses);
            }
        }

        public SteamVR_Events.Action newPosesAction;

        SteamVR_TrackedObject_bind()
        {
            newPosesAction = SteamVR_Events.NewPosesAction(OnNewPoses);
        }

        private void Start()
        {
            ETrackedPropertyError error = new ETrackedPropertyError();
            StringBuilder sb = new StringBuilder();
            bool Assigned = false;
            for (int i = 0; i < SteamVR.connected.Length; ++i)
            {
                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_ModelNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                var ModelNumber = sb.ToString();
                if (ModelNumber.Length > 0)
                {
                    OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                    var SerialNumber = sb.ToString();
                    //print("SN");
                    //print("SN:" + SerialNumber);
                    if (SerialNumber == DesiredSerialNumber)
                    {
                        UnityEngine.Debug.Log("Assigning device " + i + " to " + gameObject.name + " (" + DesiredSerialNumber + ")");
                        SetDeviceIndex(i);
                        Assigned = true;
                        //obj.TrackedObject.gameObject.transform.Find("Model").GetComponent<SteamVR_RenderModel>().index = (SteamVR_TrackedObject.EIndex)i;
                    }
                }
            }
            if (!Assigned)
            {
                UnityEngine.Debug.Log("Couldn't find a device with Serial Number \"" + DesiredSerialNumber + "\"");
            }
        }

        private void Awake()
        {
            OnEnable();
        }

        void OnEnable()
        {
            var render = SteamVR_Render.instance;
            if (render == null)
            {
                enabled = false;
                return;
            }

            newPosesAction.enabled = true;
        }

        void OnDisable()
        {
            //newPosesAction.enabled = false;
            isValid = false;
        }

        public void SetDeviceIndex(int index)
        {
            if (System.Enum.IsDefined(typeof(EIndex), index))
                this.index = (EIndex)index;
        }

        public void change_pos(TrackedDevicePose_t[] poses){
            print("pose_change");
            var i = (int)index;
            var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);
            transform.localPosition = Vector3.Lerp(transform.localPosition,pose.pos,step);
            transform.localRotation = Quaternion.Lerp(transform.localRotation,pose.rot,step) ;
            pos_change = false;
        }
    }
}