using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class UIRuleData : ManagedUI {

    public bool isBaseWindow = false;
    public UIWindowType _windowType = UIWindowType.Normal;
    public UIWindowShowMode _showMode = UIWindowShowMode.DoNothing;
    public UIWindowColliderMode _colliderMode = UIWindowColliderMode.None;

    public EventDelegate onChangeType;
    public EventDelegate onChangeShowMode;
    public EventDelegate onChangeColliderMode;

    void Start()
    {

    }

    void Awake()
    {

    }

    public UIWindowType WindowType
    {
        set { _windowType = value; }
        get { return _windowType; }
    }

    public UIWindowShowMode ShowMode
    {
        set { _showMode = value; }
        get { return _showMode; }
    }

    public UIWindowColliderMode ColliderMode
    {
        set
        {
            _colliderMode = value;
            if (onChangeColliderMode != null)
                onChangeColliderMode.Execute();
        }
        get { return _colliderMode; }
    }


}
