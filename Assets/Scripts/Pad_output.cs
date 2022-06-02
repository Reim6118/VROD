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

public class Pad_output : MonoBehaviour
{
    private bool stop = false;
    private List<TcpClient> clients = new List<TcpClient>();
    private TcpListener listener;
    public int port = 33333;
    string holder;
    public int send_receive_count = 15;

    private void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(init());
    }

    IEnumerator init()
    {
        bool source = true;
        while (source == null)
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
            Debug.Log("<color=green>in !");

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
           // holder=button_pressed.up+"," + button_pressed.down+","+button_pressed.left+ button_pressed.right;



            byte[] rotbyte = ASCIIEncoding.ASCII.GetBytes(holder);
            
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
