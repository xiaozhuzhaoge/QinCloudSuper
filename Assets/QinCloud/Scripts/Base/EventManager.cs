using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;

public class EventManager : IDisposable  {

    private Dictionary<uint, Action<JsonObject>> callBackMap;
    private Dictionary<string,List<Action<JsonObject>>> eventMap;

    public EventManager()
    {
        this.callBackMap = new Dictionary<uint, Action<JsonObject>>();
        this.eventMap = new Dictionary<string, List<Action<JsonObject>>>();
    }

    public void AddCallBack(uint id,Action<JsonObject> callback)
    {
        if(id > 0 && callback != null)
        {
            this.callBackMap.Add(id, callback);
        }
    }

    public void InvokeCallBack(uint id, JsonObject data)
    {
        if (!callBackMap.ContainsKey(id)) return;
        callBackMap[id].Invoke(data);
    }

    public void AddOnEvent(string eventName , Action<JsonObject> callback)
    {
        List<Action<JsonObject>> list = null;
        if (this.eventMap.TryGetValue(eventName, out list))
        {
            list.Add(callback);
        }
        else
        {
            list = new List<Action<JsonObject>>();
            list.Add(callback);
            this.eventMap.Add(eventName, list);
        }
    }

    public void InvokeOnEvent(string handler, JsonObject msg)
    {
        if (!this.eventMap.ContainsKey(handler)) return;

        List<Action<JsonObject>> list = eventMap[handler];
        foreach (Action<JsonObject> action in list) action.Invoke(msg);
    }

    // Dispose() calls Dispose(true)
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // The bulk of the clean-up code is implemented in Dispose(bool)
    protected void Dispose(bool disposing)
    {
        this.callBackMap.Clear();
        this.eventMap.Clear();
    }
}
