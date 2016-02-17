using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;

/// <summary>
/// 界面UI枚举
/// </summary>
public enum WindowID
{
    Invaild = 0,
    FloatMessage = 1,
    Alert = 2,
    Loading = 3,
    UILogin = 4,
    UIMain = 5
}

/// <summary>
/// 窗口Data
/// 1.显示方式
/// 2.窗口类型
/// </summary>
public class WindowData
{

    // 是否是导航起始窗口(到该界面需要重置导航信息)
    public bool isStartWindow = false;
    public UIWindowType windowType = UIWindowType.Normal;
    public UIWindowShowMode showMode = UIWindowShowMode.DoNothing;
    public UIWindowColliderMode colliderMode = UIWindowColliderMode.None;
}

public class BackWindowSequenceData
{
    //public UIBaseWindow hideTargetWindow;
    public List<WindowID> backShowTargets;
}

public class ShowWindowData
{
    // Reset窗口
    public bool forceResetWindow = false;
    // Clear导航信息
    public bool forceClearBackSeqData = false;
    // Object 数据
    public object data;
}

public delegate bool BoolDelegate();

public enum UIWindowType
{
    Normal,    // 可推出界面(UIMainMenu,UIRank等)
    Fixed,     // 固定窗口(UITopBar等)
    PopUp,     // 模式窗口
}

public enum UIWindowShowMode
{
    DoNothing,
    HideOther,     // 闭其他界面
    NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
    NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
}

public enum UIWindowColliderMode
{
    None,      // 显示该界面不包含碰撞背景
    Normal,    // 碰撞透明背景
    WithBg,    // 碰撞非透明背景
}

public class GUIManager : MonoBehaviour {

    public static GUIManager instance;
    public UIPanel mPanel;
    public Alert alert;
    public Alert OK_Cancel_alert;
    public Queue<GameObject> messagesQueue = new Queue<GameObject>();
    public UITable messageQueueTable;
    public UILabel floatMessage;
    public LoadingPanel Loading;
    public GameObject FloatMessagePanel;

    void Awake()
    {
        GUIManager.instance = this;
    }
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   
    IEnumerator WaitShow()
    {
        while (true)
        {
            if (messagesQueue.Count > 0)
                messagesQueue.Dequeue().SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }

    public void RunFloatMessage()
    {
        StartCoroutine("WaitShow");
    }

    [ContextMenu("TestAlert")]
    public void TestAlert()
    {
        alert.ShowAlert("确定", "取消", "仅供测试", "请点击确定已确认", () => { }, null, true);
    }

    [ContextMenu("TestFloatMessage")]
    public void TestFloatMessage()
    {
        FloatMessage("呵呵呵",true);
    }

    

    public static void LoadingPercent(int percent, bool direct = false)
    {
        GUIManager.instance.Loading.SetLoadingPercent(percent, direct);
        GUIManager.instance.Loading.ShowLoading();
    }

    public static void FloatMessage(string message, bool overColor = false)
    {
        if (overColor)
        {
            _FloatMessage(message);
        }
        else
        {
            _FloatMessage(LocaleConfig.Get(104033, message));
        }
    }

    static void _FloatMessage(string message)
    {
        Utility.createObjCallBack(
            GUIManager.instance.messageQueueTable.transform,
            Vector3.one,
            GUIManager.instance.floatMessage.transform.position,
            Quaternion.identity,
            GUIManager.instance.floatMessage.gameObject,
            false,
            GUIManager.instance.messagesQueue.Count.ToString(),
            (obj) =>
            {
                obj.GetComponent<UILabel>().text = message;
                GUIManager.instance.messageQueueTable.repositionNow = true;
                GUIManager.instance.messageQueueTable.Reposition();
                GUIManager.instance.messagesQueue.Enqueue(obj);
            });
        GUIManager.instance.messagesQueue.Dequeue().gameObject.SetActive(true);
        GUIManager.instance.messageQueueTable.Reposition();
    }

    public static void FinishLoading()
    {
        GUIManager.LoadingPercent(100);
        GUIManager.instance.Loading.CloseLoading();
    }

}
