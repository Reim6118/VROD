using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WVR_Log;

public class button_pressed : MonoBehaviour
{
  
    void Update()
    {
        bool up = WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(WVR_InputId.WVR_InputId_Alias1_DPad_Up);
    }
}
