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

public class _TextureReceiver : MonoBehaviour
{
    public int port = 5000;
    public string IP = "127.0.0.1";
    TcpClient client;

    [HideInInspector]
    public Texture2D texture;

    private bool stop = false;

    [Header("Must be the same in sender and receiver")]
    public int messageByteLength = 24;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;

        client = new TcpClient();

        //Connect to server from another Thread
        Loom.RunAsync(() => {
            // if on desktop
            // client.Connect(IPAddress.Loopback, port);
            client.Connect(IPAddress.Parse(IP), port);

            imageReceiver();
        });
    }
    void imageReceiver()
    {
        //thread wont block unity main thread
        Loom.RunAsync(() => {
            while (!stop)
            {
                //Read Image Count
             //   int imageSize = readVR(messageByteLength);

                //Read Image Bytes and Display it
          //     readFrameByteArray(imageSize);
            }
        });
    }

    //Converts the byte array to the data size and returns the result
    int frameByteArrayToByteLength(byte[] frameBytesLength)
    {
        int byteLength = BitConverter.ToInt32(frameBytesLength, 0);
        return byteLength;
    }

    private int readVR(int size)
    {
        bool disconnected = false;

        NetworkStream serverStream = client.GetStream();
        byte[] imageBytesCount = new byte[size];
        var total = 0;
        do
        {
            var read = serverStream.Read(imageBytesCount, total, size - total);
            if (read == 0)
            {
                disconnected = true;
                break;
            }
            total += read;
        } while (total != size);

        int byteLength;

        if (disconnected)
        {
            byteLength = -1;
        }
        else
        {
            byteLength = frameByteArrayToByteLength(imageBytesCount);
        }

        return byteLength;
    }

    private void readFrameByteArray(int size)
    {
        bool disconnected = false;

        NetworkStream serverStream = client.GetStream();
        byte[] imageBytes = new byte[size];
        var total = 0;
        do
        {
            var read = serverStream.Read(imageBytes, total, size - total);
            if (read == 0)
            {
                disconnected = true;
                break;
            }
            total += read;
        } while (total != size);

        bool readyToReadAgain = false;

        //Display Image
        if (!disconnected)
        {
            //Display Image on the main Thread
            Loom.QueueOnMainThread(() => {
               // loadReceivedImage(imageBytes);
               
                readyToReadAgain = true;
            });
        }

        //Wait until old Image is displayed
        while (!readyToReadAgain)
        {
            System.Threading.Thread.Sleep(1);
        }
    }


   /* void loadReceivedImage(byte[] receivedImageBytes)
    {
        if (texture) texture.LoadImage(receivedImageBytes);
    }*/

   /* public void SetTargetTexture(Texture2D t)
    {
        texture = t;
    }*/

    void OnApplicationQuit()
    {
        stop = true;

        if (client != null)
        {
            client.Close();
        }
    }
}
