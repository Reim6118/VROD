using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class local_IP : MonoBehaviour
{
    public GameObject displayText;
    private void Start()
    
    {
        string IPv4 = IPManager.GetIP(ADDRESSFAM.IPv4);
        string IPv6 = IPManager.GetIP(ADDRESSFAM.IPv6);
        displayText.GetComponent<Text>().text = "Local_IPv4:" + IPv4 + "\n IPv6:" + IPv6;

    }
}
