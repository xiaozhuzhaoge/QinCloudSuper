using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class ConfigInfo : MonoBehaviour {

    Dictionary<int , UIRuleConfig> _uiRule = new Dictionary<int, UIRuleConfig>();

    static void UIRuleConfig(List<UIRuleConfig> list)
    {
        foreach(var data in list)
        {
            ConfigInfo.instance._uiRule.Add(data.id, data);
        }
    }

    public Dictionary<int , UIRuleConfig> uiRule { 
    
        get {  
        PreLoadConfig("uiRule",_uiRule.Count == 0);
        return _uiRule;
        }
    }

}
