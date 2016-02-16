using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Connector : MonoBehaviour {

    class HttpStatus
    {
        public bool disposed;
    }
    /// <summary>
    /// HTTP POST 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="form"> WWWform </param>
    /// <param name="callback"> ActionCallBack </param>
    /// <param name="timeout"> outTime </param>
    /// <param name="OnTimeout"> OnTimeOutCallBack </param>
    public void HttpPost(string url, WWWForm form, Action<WWW> callback, float timeout = 0, Action<WWW> OnTimeout = null)
    {
        HttpStatus status = new HttpStatus();
        StartCoroutine(Request(url, status, form, callback, (www) =>
        {
            if (timeout > 0)
            {
                StartCoroutine(Timing(www, status, timeout, OnTimeout));
            }
        }));
    }
    /// <summary>
    /// HTTP GET
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback"> ActionCallBack </param>
    /// <param name="timeout"> outTime </param>
    /// <param name="OnTimeout"> OnTimeOutCallBack </param>
    public void HttpGet(string url, Action<WWW> callback, float timeout = 0, Action<WWW> OnTimeout = null)
    {
        HttpStatus status = new HttpStatus();
        StartCoroutine(Request(url, status, callback, (www) =>
        {
            if (timeout > 0)
            {
                StartCoroutine(Timing(www, status, timeout, OnTimeout));
            }
        }));
    }

    IEnumerator Request(string url, HttpStatus status, WWWForm form, Action<WWW> callback, Action<WWW> OnRequestBegin = null)
    {
        using (WWW currentHttpwww = new WWW(url, form))
        {
            if (OnRequestBegin != null)
                OnRequestBegin(currentHttpwww);
            yield return currentHttpwww;
            if (!status.disposed)
            {
                if (callback != null)
                {
                    callback(currentHttpwww);
                }
                status.disposed = true;
            }
        }
    }
    /// <summary>
    /// 超时
    /// </summary>
    /// <param name="_www"></param>
    /// <param name="status"></param>
    /// <param name="timeout"></param>
    /// <param name="OnTimeout"></param>
    /// <returns></returns>
    IEnumerator Timing(WWW _www, HttpStatus status, float timeout, Action<WWW> OnTimeout)
    {
        yield return new WaitForSeconds(timeout);
        if (status.disposed || _www == null)
        {
            yield break;
        }
        if (!status.disposed || !_www.isDone)
        {
            if (OnTimeout != null)
            {
                OnTimeout(_www);
            }
        }
        status.disposed = true;
    }
    /// <summary>
    /// 请求封装
    /// </summary>
    /// <param name="url"></param>
    /// <param name="status"></param>
    /// <param name="callback"></param>
    /// <param name="OnRequestBegin"></param>
    /// <returns></returns>
    IEnumerator Request(string url, HttpStatus status, Action<WWW> callback, Action<WWW> OnRequestBegin = null)
    {
        using (WWW currentHttpwww = new WWW(url))
        {
            if (OnRequestBegin != null)
                OnRequestBegin(currentHttpwww);
            yield return currentHttpwww;
            if (!status.disposed)
            {
                status.disposed = true;
                if (callback != null)
                {
                    callback(currentHttpwww);
                }
            }
        }
    }
}
