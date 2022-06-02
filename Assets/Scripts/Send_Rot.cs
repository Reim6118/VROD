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

public class Send_Rot : MonoBehaviour
{
    private bool stop = false;
    public static bool source = false;
    private List<TcpClient> clients = new List<TcpClient>();
    private TcpListener listener;
    public int port = 8888;
    string holder;
    public int send_receive_count = 60;

    public void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(init());
    }

    IEnumerator init()
    {
        while (source == false)
        {
            yield return null;
        }
        { 
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            StartCoroutine(SendControl());
        }
    }

    IEnumerator SendControl()
    {
        bool isConnected = false;

        TcpClient client = null;
        NetworkStream stream = null;

        // Wait for client to connect in another Thread 
        Loom.RunAsync(() =>
        {

            while (!stop)
            {
                // Wait for client connection
                client = listener.AcceptTcpClient();
                Debug.Log("<color=green>Connected!");
                // We are connected
                clients.Add(client);

                isConnected = true;
                stream = client.GetStream();
            }
        });

        while (!isConnected)
        {
            yield return null;
        }
        Debug.Log("Connected");
        bool readyToGetFrame = true;
        byte[] framesbybytes = new byte[send_receive_count];

        while (!stop)
        {
            //holder = WaveVR_DevicePoseTracker.data_rot + "," + button_pressed.key;

            holder = WaveVR_DevicePoseTracker.euler_rot + "," + button_pressed.key;
            Debug.LogWarning("Send_Rot_euler_rot:" + WaveVR_DevicePoseTracker.r.eulerAngles);
            //button_pressed.up + "," + button_pressed.down + "," + button_pressed.left + "," + button_pressed.right;
            //byte[] posbyte = ASCIIEncoding.ASCII.GetBytes(WaveVR_DevicePoseTracker.data_pos);
            byte[] rotbyte = ASCIIEncoding.ASCII.GetBytes(holder);
            //byte[] combine_byte = new byte[posbyte.Length + rotbyte.Length];
            //System.Buffer.BlockCopy(posbyte, 0, combine_byte, 0, posbyte.Length);
            //System.Buffer.BlockCopy(rotbyte, 0, combine_byte, posbyte.Length, rotbyte.Length);
            //Debug.Log("Combine_byte:" + combine_byte);
            readyToGetFrame = false;
            Loom.RunAsync(() =>
            {
                stream.Write(framesbybytes, 0, framesbybytes.Length);
                Debug.Log("Sent byte length:" + framesbybytes.Length);
                stream.Write(rotbyte, 0, rotbyte.Length);
                readyToGetFrame = true;


            });

            while (!readyToGetFrame)
            {
                yield return null;
            }
        }


    }
    private void OnApplicationQuit()
    {
        if (listener != null)
        {
            listener.Stop();
        }

        foreach (TcpClient c in clients)
            c.Close();
    }

    
}
