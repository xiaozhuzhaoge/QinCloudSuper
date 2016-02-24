using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;
using System.Reflection;



public class BackWindowSequenceData
{
    public List<WindowID> backShowTargets;
}

public delegate bool BoolDelegate();


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
    public UITooltip FloatTip;

    public static List<UImode> MainThreadStack = new List<UImode>();
    public static List<UImode> BackSequenceStack = new List<UImode>();

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

    public static void FloatMessage(string message, bool notOverColor = true)
    {
        if (notOverColor)
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


    public static void ShowGUI<T>(UImode ui, string name) where T : UImode
    {
        if(ui == null)
        {
            SceneManager.instance.LoadSceneGUI("GUI/"+name);
            Utility.StartSceneCoroutine<float, Action>(Utility.instance.Wait, 0.01f, () =>
            {
                ((UImode)typeof(T).GetField("Instance").GetValue(null)).OpenWindow();
            });
        }
        else
        {
          ui.OpenWindow();
        }
        ResetAllCacheDepth();
    }


    public static void CloseGUI<T>(UImode ui) where T : UImode
    {
        if(ui != null)
            ui.CloseWindow();
    }

    public static void ShowPreviousUI(UImode current , Action<GameObject> callback)
    {
        if (current == null)
            return;
          current.OpenPreviousWindow();
    }

    public static void ResetAllCacheDepth()
    {
        for(int i = 0 ;  i< BackSequenceStack.Count ; i ++)
        {
            ManagedUI.Repostion(BackSequenceStack[i].m_panel, i);
        }
    }

    public void BackSequence()
    {
        if (BackSequenceStack.Count == 0)
            return;
        BackSequenceStack[BackSequenceStack.Count - 1].CloseWindow(ui => { BackSequenceStack.Remove(ui.GetComponent<UImode>()); });

    }

    [ContextMenu("1")]
    public void TestShowGUI1()
    {
        GUIManager.ShowGUI<UIOne>(UIOne.Instance,"GUIOne");
    }
    [ContextMenu("2")]
    public void TestShowGUI2()
    {
        GUIManager.CloseGUI<UIOne>(UIOne.Instance);
    }

    public void TestToolTips()
    {
        UITooltip.Show("Click this button to open Page 1");  
    }
    public void HideToolTips()
    {
        UITooltip.Hide();
    }
}
