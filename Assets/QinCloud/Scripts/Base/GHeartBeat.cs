using UnityEngine;
using System.Collections;
using WebSocket4Net;
using SimpleJson;
using UnityEngine.Internal;
using System;


public class GHeartBeat : MonoBehaviour {

    private float interval;
    bool isConnect;
    public ClientNet net;
    public float timeout = 0;
    public int frequency = 5;
    public float starttime;
    public Action heartBeatAction;

    public bool IsConnected
    {
        set { isConnect = value; }
        get { return isConnect; }
    }

    public float Interval
    {
        set { interval = value; }
        get { return interval; }
    }

    public GHeartBeat(int interval , ClientNet net)
    {
        this.interval = interval;
        IsConnected = true;
        this.net = net;
    }

    public void SendHeartBeat()
    {
        timeout += Time.realtimeSinceStartup - starttime;
        starttime = timeout;
        if (timeout > interval * frequency)
        {
            IsConnected = false;
            Stop();
            return;
        }
        net.request("connector.entryhandler.HeartBeat",(req) => {
            ResetTimeOut();
            if (heartBeatAction != null)
                heartBeatAction();
        });
    }

    public void ResetTimeOut()
    {
        starttime = Time.realtimeSinceStartup;
        timeout = 0;
    }
    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Interval);
            SendHeartBeat();
        }
    }
    public void Start()
    {
        if (interval < 1)
            return;
        ResetTimeOut();
        StopCoroutine("Timer");
        StartCoroutine("Timer");
    }

    public void Stop()
    {
        StopCoroutine("Timer");
        IsConnected = false;
        GameObject.Destroy(this.gameObject);
    }

}
