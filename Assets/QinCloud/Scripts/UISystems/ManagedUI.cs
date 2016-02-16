using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ManagedUI : MonoBehaviour
{   
    public WindowID uiName;
    public UIPanel panel;
    static Dictionary<int,List<ManagedUI>> uis = new Dictionary<int, List<ManagedUI>>();
    int subLevel = 0;
    UIRuleConfig config;

    void OnEnable()
    {
        StartCoroutine(Show());
    }

    void OnDestroy()
    {
        if (config != null)
        {
            uis [config.uiLevel].Remove(this);
        }
    }

    IEnumerator Show()
    {
        try{
            config = ConfigInfo.instance.uiRule [(int)uiName];
        }catch(Exception e){
            Debug.Log (uiName+";;;;;");
            Debug.LogException (e);
            yield break;
        }
        if (!uis.ContainsKey(config.uiLevel))
        {
            uis.Add(config.uiLevel, new List<ManagedUI>());
        } 
        if (!uis [config.uiLevel].Contains(this))
        {
            uis [config.uiLevel].Add(this);
        }
        subLevel = uis [config.uiLevel].Count;

        uis [config.uiLevel].Sort((x , y) => {
            return x.subLevel - y.subLevel;
        });
        Reposition(this, config.uiLevel);
    }

    /// <summary>
    /// 同一组UI，后选择的在最上面;
    /// 并且一个UI内的不同panel会根据之前摆好的层级排列
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="level">Level.</param>
    static void Reposition(ManagedUI target, int level)
    {
        for (int i=0; i<uis [level].Count; i++)
        {
            ManagedUI ui = uis [level] [i];
            if (ui != target)
            {
                ui.subLevel = i;
            }
            ui.panel.depth = level * 4000 + ui.subLevel * 80;
            List<UIPanel> panels = new List<UIPanel>();
            panels.AddRange(ui.GetComponentsInChildren<UIPanel>(true));
            panels.Remove(ui.panel);
            if (panels.Count >= 80)
            {
				Debug.Log(panels.Count + ";;;;;;" + level + ";;;" + target.uiName, target);
                throw new UnityException("too many panels!");
            }
            panels.Sort((x,y) => {
                return x.depth - y.depth;
            });
            int temp = ui.panel.depth;
            foreach (var p in panels)
            {
                if (ui.panel == p)
                {
                    continue;
                }
                p.depth = ++temp;
            }
        }
    }
}
