using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WVR_Log;

public class button_pressed : MonoBehaviour
{
    /*static public int up=0;
    static public int down=0;
    static public int left=0;
    static public int right=0;*/
    static public string key;

    public void Update()
    {
        //up = down = left = right = 0;

        if (WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(WVR_InputId.WVR_InputId_Alias1_DPad_Up))
        {
            //up = 1;
            key = "w";
        }
        if(WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(WVR_InputId.WVR_InputId_Alias1_DPad_Down))
        {
            //down = 1;
            key = "s";
        }
        if(WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(WVR_InputId.WVR_InputId_Alias1_DPad_Left))
        {
            //left = 1;
            key = "a";
        }
        if(WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(WVR_InputId.WVR_InputId_Alias1_DPad_Right))
        {
            //right = 1;
            key = "d";
        }

    }

}
