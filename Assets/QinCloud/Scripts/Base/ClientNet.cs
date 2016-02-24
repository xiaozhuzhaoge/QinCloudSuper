using UnityEngine;
using System.Collections;
using System;
using WebSocket4Net;
using System.Net;
using SuperSocket.ClientEngine;
using SimpleJson;
using System.Collections.Generic;

public class Message
{
    public WBMessageType type;
    public string handler;
    public uint id;
    public string msg;

    public Message(WBMessageType type, uint id, string handler, string data)
    {
        this.type = type;
        this.id = id;
        this.handler = handler;
        this.msg = data;
    }

    public Message(string message)
    {
        string realMesage = ClientNet.getReceivedMessage(message);
        JsonObject obj = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(realMesage);
        if(obj.ContainsKey("reqId"))
        {
            type = WBMessageType.MSG_REQUEST;
            id = Convert.ToUInt32(obj["reqId"]);
        }
        else
        {
            type = WBMessageType.MSG_RESPONSE;
        }
        if (obj.ContainsKey("handler"))
        {
            handler = Convert.ToString(obj["handler"]);
        }
        if(obj.ContainsKey("msg"))
        {
            msg = Convert.ToString(obj["msg"]);
        }
    }
}

public enum WBMessageType
{
    MSG_REQUEST = 0,
    MSG_NOTIFY = 1,
    MSG_RESPONSE = 2,
    MSG_PUSH = 3
}

public class ClientNet : IDisposable {

    public EventManager eventManager;
    const int HEART_BEAT_INTERVAL = 7;
    public GHeartBeat heartBeat = null;
    public WebSocket myClient;
    public bool disposed;
    public string uri;
    uint reqId = 1;
    string EVENT_DISCONNECT = "disconnect";

    public bool IsConnect
    {
        get { return !disposed; }
    }



    public Queue<string> messageQueue = new Queue<string>();


    public void EnqueueMessage(string message)
    {
        lock(messageQueue)
        messageQueue.Enqueue(message);
    }


    public ClientNet(string uri)
    {
        this.eventManager = new EventManager();
        this.uri = uri;

    }

    private bool EnsureWebSocketOpen()
    {
        if (!myClient.Handshaked)
        {
            Debuger.LogError("NoOpenSending");
            return false;
        }

        return true;
    }

    private void StatUnityClient(string url)
    {
        Debuger.Log("Start Socket " );
        myClient = new WebSocket(uri);
        myClient.Opened += new EventHandler(OnWebSocketConnected);
        myClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(OnWebSocketError);
        myClient.Closed += new EventHandler(OnWebSocketClosed);
        myClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(OnWebSocketMessageReceived);
        myClient.DataReceived += new EventHandler<DataReceivedEventArgs>(OnWebSocketDataReceived);
        myClient.Open();
    }

    private void OnWebSocketConnected(object sender, EventArgs e)
    {
        Debuger.Log("Socket Open " + e);
        disposed = false;
    }

    private void OnWebSocketClosed(object sender, EventArgs e)
    {
        Debuger.Log("Socket Close " + e);
        disposed = true;
        Dispose();
    }

    private void OnWebSocketError(object sender, ErrorEventArgs e)
    {
        Debuger.Log("Socket Error " + e.Exception);
        disposed = true;
        Dispose();
    }

    private void OnWebSocketMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debuger.Log("Received Message " + e.Message);
        EnqueueMessage(e.Message);
    }

    private void OnWebSocketDataReceived(object sender, DataReceivedEventArgs e)
    {
        Debuger.Log("Received Data " + e.Data);
    }

    //检查心跳撒
    public bool Connected
    {
        get
        {
            if (heartBeat != null)
                return heartBeat.IsConnected;
            else
                return false;
        }
    }

    public void connect()
    {
        StatUnityClient(uri);
    }
    /// <summary>
    /// 请求函数，需要注册回调
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="msg"></param>
    /// <param name="action"></param>
    public void request(string handler, JsonObject msg , Action<JsonObject> action)
    {
        this.eventManager.AddCallBack(reqId, action);
        sendMessage(handler, reqId, msg);
        reqId++;
    }

    public void request(string handler , Action<JsonObject> action)
    {
        eventManager.AddCallBack(reqId, action);
        JsonObject empty = new JsonObject();
        sendMessage(handler, reqId, empty);
        reqId++;
    }

    /// <summary>
    /// 通知函数不需要注册回调
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="msg"></param>
    public void notify(string handler , JsonObject msg)
    {
        sendMessage(handler, msg);
    }

    private void sendMessage(string handler , JsonObject msg)
    {
        JsonObject obj = new JsonObject();
        obj.Add("handler", handler);
        obj.Add("msg", msg);
        myClient.Send(obj.ToString());
    }

    private void sendMessage(string handler ,uint reqId,JsonObject msg)
    {
        JsonObject obj = new JsonObject();
        obj.Add("handler",handler);
        obj.Add("reqId",reqId);
        obj.Add("msg",msg);
        myClient.Send(obj.ToString());
    }
    /// <summary>
    /// 接受服务器推送
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    private void on(string eventName , Action<JsonObject> action)
    {
        eventManager.AddOnEvent(eventName, action);
    }

    public void disconnect()
    {
        Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
            return;
        if (disposing)
        {
            myClient.Close();
            this.disposed = true;
            if (heartBeat != null)
            {
                heartBeat.Stop();
                heartBeat = null;
            }
            eventManager.InvokeOnEvent(EVENT_DISCONNECT, null);
        }
    }

    public static string getReceivedMessage(string input)
    {
        string output = input.Remove(0, 10);
        return output;
    }

    public void StartHeartBeat()
    {
        if (IsConnect == false)
            return;

        heartBeat = NGUITools.AddChild<GHeartBeat>(SceneManager.instance.cacheRoot.gameObject);
        heartBeat.Interval = 6f;
        heartBeat.frequency = 5;
        heartBeat.net = this;
        heartBeat.Start();
    }

}
