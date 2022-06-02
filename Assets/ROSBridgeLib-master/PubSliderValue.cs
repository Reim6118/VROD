using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class PubSliderValue : ROSBridgeLib.ROSBridgePublisher
        {

            public new static string GetMessageTopic()
            {

                return "/cmd_vel";
            }

            public new static string GetMessageType()
            {
                return "geometry_msgs/Twist";
            }

            public static string ToYAMLString(ROSBridgeLib.std_msgs.StringMsg msg)
            {
                return msg.ToYAMLString();
            }

            public static ROSBridgeMsg ParseMessage(SimpleJSON.JSONNode msg)
            {
                return new TwistMsg(msg);
            }
        }
    }
}


