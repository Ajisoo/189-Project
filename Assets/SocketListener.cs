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
    public static ManualResetEvent allDone = new ManualResetEvent(false);

    public float readValue1;
    public float readValue2;
    public class StringHolder
    {
        public string data;
    }
    public static StringHolder str = new StringHolder();
    byte[] bytes = new byte[1024];
    Socket handler = null;
    // Start is called before the first frame update
    void Start()
    {
        str.data = "";
        //new Thread(new ThreadStart(doThing)).Start();
        //doThing();
    }

    void doThing()
    {
        Debug.Log("Starting thread");
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 27016);

        Socket listener = new Socket(AddressFamily.InterNetwork,
           SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            // An incoming connection needs to be processed.  
            Debug.Log("Waiting for a connection...");
            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

            allDone.WaitOne();
            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
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
        lock (str)
        {
            //if (str.data != null) Debug.Log("Text received : " + str.data);

        }
    }

    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public static void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.  
        allDone.Set();

        // Get the socket that handles the client request.  
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        // Create the state object.  
        StateObject state = new StateObject();
        state.workSocket = handler;
        Debug.Log("Accepted");
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public static void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket.   
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0)
        {
            // There  might be more data, so store the data received so far.  
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            Debug.Log(state.sb.ToString());
            // Check for end-of-file tag. If it is not there, read   
            // more data.  
            content = state.sb.ToString();
            if (content.IndexOf("<EOF>") > -1)
            {
                // All the data has been read from the   
                // client. Display it on the console.  
                lock (str)
                {
                    str.data = content;
                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            else
            {
                // Not all data received. Get more.  
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            }
        }
    }
}
