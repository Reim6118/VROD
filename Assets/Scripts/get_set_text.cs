using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class get_set_text : MonoBehaviour
{
    public  GameObject Button;
    public static string ip;
    public void setget()
    {
        ip = Button.GetComponent<Text>().text;
    }
    void OnDisable()
    {
        PlayerPrefs.SetString("ip", ip);    
    }
}
