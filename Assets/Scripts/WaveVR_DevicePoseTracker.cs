// "WaveVR SDK 
// © 2017 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using wvr;
using WVR_Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class is mainly for Device Tracking.
/// Tracking object communicates with HMD device or controller device in order to:
/// update new position and rotation for rendering
/// </summary>
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WaveVR_DevicePoseTracker))]
public class WaveVR_DevicePoseTrackerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		WaveVR_DevicePoseTracker myScript = target as WaveVR_DevicePoseTracker;
        
		myScript.type = (WaveVR_Controller.EDeviceType)EditorGUILayout.EnumPopup ("Type", myScript.type);
		myScript.inversePosition = EditorGUILayout.Toggle ("Inverse Position", myScript.inversePosition);
		myScript.trackPosition = EditorGUILayout.Toggle ("Track Position", myScript.trackPosition);
		if (true == myScript.trackPosition)
		{
			if (myScript.type == WaveVR_Controller.EDeviceType.Head)
			{
				myScript.EnableNeckModel = (bool)EditorGUILayout.Toggle ("	Enable Neck Model", myScript.EnableNeckModel);
			}
		}

		myScript.inverseRotation = EditorGUILayout.Toggle ("Inverse Rotation", myScript.inverseRotation);
		myScript.trackRotation = EditorGUILayout.Toggle ("Track Rotation", myScript.trackRotation);
		myScript.timing = (WVR_TrackTiming)EditorGUILayout.EnumPopup ("Track Timing", myScript.timing);

		if (GUI.changed)
			EditorUtility.SetDirty ((WaveVR_DevicePoseTracker)target);
	}
}
#endif

public sealed class WaveVR_DevicePoseTracker : MonoBehaviour
{
	private static string LOG_TAG = "WaveVR_DevicePoseTracker";
	/// <summary>
	/// The type of this controller device, it should be unique.
	/// </summary>
	public WaveVR_Controller.EDeviceType type;
	public bool inversePosition = false;
	public bool trackPosition = true;
	[Tooltip("Effective only when Track Position is true.")]
	public bool EnableNeckModel = true;
	public bool inverseRotation = false;
 	public WVR_TrackTiming timing = WVR_TrackTiming.WhenNewPoses;
  	public static WaveVR_Utils.RigidTransform rigid_pose = WaveVR_Utils.RigidTransform.identity;

  	public static Vector3 z;
        public static Quaternion r;
	public static float euler_rot;
    	public static string data_rot;
    	public static string data_pos;
    	public bool trackRotation = true;
     	public int port = 8899;
   	
    
    private TcpListener listener;
    private TcpListener listner;
    private List<TcpClient> clients = new List<TcpClient>();
    private bool stop = false;
    private WVR_DevicePosePair_t wvr_pose = new WVR_DevicePosePair_t ();
    byte[] pos_bytes = null;
    byte[] rot_bytes = null;
    
    const int send_receive_count = 15;
	void Update()
	{
        // Debug.Log("<color=blue>Data_rot in update:</color>" + data_rot);
       // Debug.Log("<color=yellow>Byte length in update</color>" + rot_bytes.Length);
        if (timing == WVR_TrackTiming.WhenNewPoses)
			return;
		if (!WaveVR.Instance.Initialized)
			return;
		WaveVR.Device device = WaveVR.Instance.getDeviceByType (this.type);
		if (device.connected)
		{
			wvr_pose = device.pose;
			rigid_pose = device.rigidTransform;
		}

       Convertor(wvr_pose, rigid_pose);		// convert the headset pose into the format that we want
      //  updatePose(wvr_pose, rigid_pose);

    }

    private void OnNewPoses(params object[] args)
	{
		WVR_DevicePosePair_t[] _poses = (WVR_DevicePosePair_t[])args [0];
		WaveVR_Utils.RigidTransform[] _rtPoses = (WaveVR_Utils.RigidTransform[])args [1];

		WVR_DeviceType _type = WaveVR_Controller.Input (this.type).DeviceType;
		for (int i = 0; i < _poses.Length; i++)
		{
			if (_type == _poses [i].type)
			{
				wvr_pose = _poses [i];
				rigid_pose = _rtPoses [i];
			}
		}

		Convertor (wvr_pose, rigid_pose);
    //    updatePose(wvr_pose, rigid_pose);

    }
    //convert the headset position into ROS format
    private void Convertor(WVR_DevicePosePair_t pose, WaveVR_Utils.RigidTransform rtPos)
    {
        //Update holder
        if (trackPosition)
        {
            if (inversePosition)
            {
                Vector3 x = -rtPos.pos;
                data_pos = x.x + "," + x.y + "." + x.z;	
                Debug.Log("<color=blue>Rot bytes:</color>" + data_pos);

            }
            else
            {
                Vector3 x = rtPos.pos;
                data_pos= x.x + "," + x.y + "." + x.z;
                Debug.Log("<color=blue>Rot bytes:</color>" + data_pos);
            }

        }
        if (trackPosition)
        {
            if (inverseRotation)
            {
                r = Quaternion.Inverse(rtPos.rot);
                Vector3 z = r.eulerAngles;
                euler_rot = z.y;
                data_rot = r.x + "," + r.y + "," + r.z + "," + r.w;
                //    byte[] rot_bytes = Encoding.ASCII.GetBytes(data_rot);
                //Debug.Log("<color=red>Rot bytes:</color>" + euler_rot);
                Debug.Log("<color=red>Rot bytes:</color>" + data_rot);

            }
            else
            {
                r = (rtPos.rot);
                Vector3 z = r.eulerAngles;
                euler_rot= z.y;
                data_rot = r.x + "," + r.y + "," + r.z + "," + r.w;
                //   byte[] rot_bytes = Encoding.ASCII.GetBytes(data_rot);
                //Debug.Log("<color=red>Rot bytes:</color>" + euler_rot);
                Debug.Log("<color=red>Rot bytes:</color>" + data_rot);



            }
        }
    }
    
    //converts the data size to byte array and put result to the fullBytes array
    void byteLengthToFrameByteArray(int byteLength, byte[] fullBytes)
    {
        //Clear old data
        Array.Clear(fullBytes, 0, fullBytes.Length);
        //Convert int to bytes
        byte[] bytesToSendCount = BitConverter.GetBytes(byteLength);
        //Copy result to fullBytes
        bytesToSendCount.CopyTo(fullBytes, 0);
    }


    void OnEnable()
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        if (this.timing == WVR_TrackTiming.WhenNewPoses)
            WaveVR_Utils.Event.Listen(WaveVR_Utils.Event.NEW_POSES, OnNewPoses);

        if (this.type == WaveVR_Controller.EDeviceType.Head)
        {
            Log.i(LOG_TAG, "OnEnable() " + this.type + ", WVR_SetNeckModelEnabled to " + EnableNeckModel);
            WaveVR.Instance.SetNeckModelEnabled(EnableNeckModel);
        }

        Log.d(LOG_TAG, "OnEnable() " + this.type
            + ", trackPosition: " + this.trackPosition
            + ", trackRotation: " + this.trackRotation
            + ", timing: " + this.timing);
    }

    void OnDisable()
    {
        if (this.timing == WVR_TrackTiming.WhenNewPoses)
            WaveVR_Utils.Event.Remove(WaveVR_Utils.Event.NEW_POSES, OnNewPoses);
    }

    private void OnApplicationQuit()
    {
        if (listner != null)
        {
            listner.Stop();
        }

        foreach (TcpClient c in clients)
            c.Close();
    }

}

