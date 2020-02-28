using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;
using UnityEngine;

public class SocketListener : MonoBehaviour
{
    public int port;
    private string data = "";
    byte[] bytes = new byte[1024];
    Socket handler = null;
    // Start is called before the first frame update
    void Start()
    {
        data = "";
        new Thread(new ThreadStart(doThing)).Start();
    }

    void doThing()
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(address, 11001);

        Socket listener = new Socket(address.AddressFamily,
           SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            handler = listener.Accept();
            Debug.Log("Accepted input");
            // An incoming connection needs to be processed.  
            while (true)
            {
                int bytesRec = handler.Receive(bytes);
                StringBuilder hex = new StringBuilder(bytesRec * 2);
                foreach (byte b in bytes)
                {
                    hex.AppendFormat("{0:x2}", b);
                }
                Debug.Log(hex.ToString());
                lock (data)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    Debug.Log(bytes.ToString());
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            if (handler != null) handler.Shutdown(SocketShutdown.Both);
            if (handler != null) handler.Close();
            //if (listener != null) listener.Shutdown(SocketShutdown.Both);
            if (listener != null) listener.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        lock (data)
        {
            //if (data != null) Debug.Log("Text received : " + data);

        }
    }
}
