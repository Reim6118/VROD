using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class get_set_text : MonoBehaviour
{
    public  GameObject Inputfield;
    public static string ip;
    public void setget()
    {
        ip = Inputfield.GetComponent<Text>().text;
    }
    void OnDisable()
    {
        PlayerPrefs.SetString("ip", ip);    
    }
}
