using UnityEngine;
using System;
using System.Collections;

public class UIRuleConfig : IConfig {

    public int id;
    public int uiId;
    public string uiName;
    public int uiLevel;

    public UIRuleConfig(){}

    public UIRuleConfig(SimpleJson.JsonObject o){Init(o);}

    #region IConfig implementation

    public void Init(SimpleJson.JsonObject o)
    {
        this.id = Convert.ToInt32(o["id"]);
        this.uiId = Convert.ToInt32(o["uiId"]);
        this.uiName = Convert.ToString(o["uiName"]);
        this.uiLevel = Convert.ToInt32(o["uiLevel"]);
    }

    #endregion


	
}
