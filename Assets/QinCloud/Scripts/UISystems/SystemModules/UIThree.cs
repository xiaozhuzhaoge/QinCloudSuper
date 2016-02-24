using UnityEngine;
using System.Collections;

public class UIThree : UImode
{

    public static UImode Instance;

    public override void Awake()
    {
        base.Default();
        Instance = this;
        SetMyTarget(this.gameObject);
        type = typeof(UIThree);
        currentMode = this;
        onDestory += DestoryEvent;
    }

    public override void OpenWindow(System.Action<GameObject> callback = null)
    {
        base.OpenWindow(callback);
    }

    public override void CloseWindow(System.Action<GameObject> callback = null)
    {
        base.CloseWindow(callback);
    }

}