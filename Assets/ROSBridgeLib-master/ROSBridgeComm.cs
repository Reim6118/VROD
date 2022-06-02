using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class ROSBridgeComm : MonoBehaviour
        {

            private ROSBridgeWebSocketConnection ros = null;
            private Vector3Msg rot;
            private float previousRealtime;
            private Vector3 a;
            private Vector3 b;
            private Vector3 threesixty = Vector3.zero;
            private Vector3 c = Vector3.zero;
            private double x = 0.0000000001;
            private Vector3Msg rotation;
            // Vector3Msg oldA = new Vector3Msg(0,0,0);
            //Vector3Msg oldb = new Vector3Msg(0, 0, 0);
            private Vector3 oldA = Vector3.zero;
            private Vector3 oldB = Vector3.zero;
            //private Vector3Msg angularvelocity;

            private Vector3Msg linear;
            void Start()
            {
                threesixty.y = 360;
                ros = new ROSBridgeWebSocketConnection("ws://192.168.43.106", 9090);
                ros.AddPublisher(typeof(ROSBridgePublisher));
                ros.Connect();

            }
            void OnApplicationQuit()
            {
                if (ros != null)
                {
                    Debug.LogWarning("disconnect");
                    ros.Disconnect();
                }
            }   

            // Update is called once per frame
            void FixedUpdate()
            {
                a = Vector3.zero;
                b = Vector3.zero;
                float deltaTime = Time.realtimeSinceStartup - previousRealtime;
                a.x = WaveVR_DevicePoseTracker.r.eulerAngles.x ;
                a.y = WaveVR_DevicePoseTracker.r.eulerAngles.y;
                Debug.LogWarning("A=" + WaveVR_DevicePoseTracker.r.eulerAngles.y);
                a.z = WaveVR_DevicePoseTracker.r.eulerAngles.z;
                b.x = WaveVR_DevicePoseTracker.rigid_pose.pos.x;
                b.y = WaveVR_DevicePoseTracker.rigid_pose.pos.y;
                b.z = WaveVR_DevicePoseTracker.rigid_pose.pos.z;
                Debug.LogWarning("In update");
                Vector3 angular = (a - oldA) / (deltaTime);
                Debug.LogWarning("Angular =" + angular + "deltatime="+deltaTime);
                //Vector3 position = (b - oldB) / deltaTime;
                Vector3 position = a;
                if (angular.y >=200 )
                {
                    angular = (a - (oldA + threesixty))/(15*deltaTime);
                    rotation = new Vector3Msg(x, x, angular.y);
                }
                else if(angular.y <= -200)
                {
                    angular = (a - (oldA - threesixty))/(15*deltaTime);
                    rotation = new Vector3Msg(x, x, angular.y);    
                }
                /*else if (angular.y <= 10 && angular.y >=-10)
                {
                    rotation = new Vector3Msg(x,x,x);
                    Debug.LogWarning("ROT:" + rotation);
                }*/
                else
                {
                    angular = angular / 15;
                    rotation = new Vector3Msg(x,x,angular.y);
                }
                Debug.LogWarning("rot=" + rotation);

                //Positions
                if (position==Vector3.zero)
                {
                    linear = new Vector3Msg(x,x,x);
                }
                else if(position.x>20)
                {
                    //linear = new Vector3Msg(b.z, b.y, b.z);
                    linear = new Vector3Msg(20,x, x);

                }
                else if(position.x<=20 && position.x>=10)
                {
                    linear = new Vector3Msg(x, x, x);
                }
                else
                {
                    linear = new Vector3Msg(-20, x, x);
                }

                TwistMsg msg = new TwistMsg(linear,rotation);
                Debug.LogWarning("Msggggggg="+msg);

                previousRealtime = Time.realtimeSinceStartup;
                oldA = a;
                oldB = b;
                ros.Publish(PubSliderValue.GetMessageTopic(), msg);

                ros.Render();
            }

        }
    }
}
