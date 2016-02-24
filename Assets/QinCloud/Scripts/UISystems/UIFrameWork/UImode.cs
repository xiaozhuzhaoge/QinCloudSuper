using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class UImode : MonoBehaviour, UImodeInterface
{

    public bool defaultVisible = false;
    [HideInInspector]
    public UImode instance;
    [HideInInspector]
    public GameObject m_target;
    [HideInInspector]
    public bool isShowed = false;

    public UIPanel m_panel;

    public Action onInit;
    public Action onOpen;
    public Action onClose;
    public Action onDestory;


    public UImode currentMode;
    public UImode previousMode;
    public System.Type type;

    private bool DontAddBackSequence;
    public UIRuleData windowData;


    public virtual void Awake()
    {
        instance = this;
        
    }
  

    public void SetShowed(bool isShowed)
    {
        this.isShowed = isShowed;
    }

    public void SetMyTarget(GameObject game)
    {
        m_target = game;
    }


    public void DestoryEvent()
    {
        GUIManager.MainThreadStack.Remove(this);
        Debug.Log(GUIManager.MainThreadStack.Count);
    }

    virtual public void Default()
    {
        gameObject.SetActive(defaultVisible);
        SetShowed(gameObject.activeSelf);
        if (gameObject.GetComponent<UIRuleData>() != null)
            windowData = gameObject.GetComponent<UIRuleData>();
        if (!GUIManager.MainThreadStack.Contains(this))
            GUIManager.MainThreadStack.Add(this);

        if (gameObject.GetComponent<UIPanel>() != null)
            m_panel = gameObject.GetComponent<UIPanel>();
    }

    public virtual void InitWindow()
    {
        if (onInit != null)
            onInit();
    }

    public virtual void OpenWindow(Action<GameObject> callback = null)
    {
        AddToBackSequence();
        DirectlyOpenWindow();

        DepthsReposition(GUIManager.BackSequenceStack.IndexOf(this));

        if (onOpen != null)
            onOpen();
        if (callback != null)
            callback(m_target);
    }

    public virtual void DirectlyOpenWindow()
    {
        gameObject.SetActive(true);
        SetShowed(gameObject.activeSelf);
    }

    public virtual void CloseWindow(Action<GameObject> callback = null)
    {
        DirectlyCloseWindow();
        if (onClose != null)
            onClose();
        if (callback != null)
            callback(m_target);
    }

    public virtual void DirectlyCloseWindow()
    {
        gameObject.SetActive(false);
        SetShowed(gameObject.activeSelf);
    }

    public virtual void PreDestoryWindow()
    {
        onDestory();
    }

    public virtual void DestoryWindow()
    {
        PreDestoryWindow();
        GameObject.Destroy(this.gameObject);
    }

    public virtual void AddToBackSequence()
    {
        if (!DontAddBackSequence)
        {
            if(GUIManager.BackSequenceStack.Contains(this))
            {
                GUIManager.BackSequenceStack.Remove(this);
            }
            GUIManager.BackSequenceStack.Add(this);
        }
    }

    void OnDestory()
    {
        SetShowed(gameObject.activeSelf);
    }

    public virtual void OpenPreviousWindow()
    {
        if (previousMode != null)
            previousMode.OpenWindow();
    }

    public void DepthsReposition(int depth)
    {
        ManagedUI.Repostion(m_panel, depth);
    }
}
