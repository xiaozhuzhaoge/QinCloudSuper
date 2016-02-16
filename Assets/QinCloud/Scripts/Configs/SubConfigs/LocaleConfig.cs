using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocaleConfig : IConfig
{
    public int id;
    public string content;
    
    #region IConfig implementation
    public LocaleConfig()
    {

    }

    public LocaleConfig(SimpleJson.JsonObject o)
    {
        Init(o);
    }

    public void Init(SimpleJson.JsonObject o)
    {
        this.id = Convert.ToInt32(o ["id"]);
        this.content = Convert.ToString(o["content"]).Replace("\\n", "\n");
    }

    #endregion

    public static string Get(int id, params object[] objs)
    {
        if (id == 0)
        {
            return "";
        }
    
        try
        {
            return string.Format(ConfigInfo.instance.locales [id].content, objs);
        } catch (KeyNotFoundException e)
        {
            Debug.Log(id + ";;;;");
            Debug.LogException(e);
            return "";
        } catch (FormatException e)
        {
            Debug.Log(id + ";;;;");
            Debug.LogException(e);
            return "";
        }
    }

  
}
