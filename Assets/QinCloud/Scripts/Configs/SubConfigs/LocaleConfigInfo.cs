using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class ConfigInfo : MonoBehaviour{

    Dictionary<int, LocaleConfig> _locales = new Dictionary<int, LocaleConfig>();

    static void LocaleConfig(List<LocaleConfig> list)
    {
        foreach (var item in list)
        {
            try
            {
                ConfigInfo.instance._locales.Add(item.id, item);
            }
            catch (Exception e)
            {
                Debug.Log(item.id + ";;;;;;;;;;");
                Debug.LogException(e);
            }
        }
    }
    
    public Dictionary<int, LocaleConfig> locales
    {
        get
        {
            PreLoadConfig("uiMessage", _locales.Count == 0);
            return _locales;
        }
    }

   
}
