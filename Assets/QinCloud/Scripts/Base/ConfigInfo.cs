using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;
using System.Linq;
using System.Reflection;
public partial class ConfigInfo : MonoBehaviour {

    public static ConfigInfo instance;
    void Awake()
    {
        instance = this;
    }

    public static void Init()
    {
        //Type type = typeof(ConfigInfo);
        //foreach(var data in ConfigReader.classDic)
        //{
        //    RegisterConfigHandler<SoundConfig>(data.Key, list =>
        //    {
        //        type.GetMethod(data.Value).Invoke(ConfigInfo.instance, new object[] { list });
        //    });
        //}
        RegisterConfigHandler<SoundConfig>("sound", SoundConfig);
        RegisterConfigHandler<LocaleConfig>("uiMessage", LocaleConfig);
    }
 
    public static void ReadUIRules()
    {
        RegisterConfigHandler<UIRuleConfig>("uiRule", UIRuleConfig);
    }
    /// <summary>
    /// 载入游戏时第一次配置文件预加载(量级大时再使用)
    /// </summary>

    public static void InitPreGameConfigs()
    {
        //preGameConfigs.Add("uiMessage");
        //preGameConfigs.Add("sound");
        //preGameConfigs.Add("uiRule");
    }
}
