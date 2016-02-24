using UnityEngine;
using System.Collections;


public enum UIWindowType
{
    Normal,    // 系统窗口
    Fixed,     // 固定窗口
    PopUp,     // 弹出窗口
}

public enum UIWindowShowMode
{
    DoNothing,     // 打开界面不做任何事情
    HideOther,     // 打开界面关闭其他界面
    NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
    NoNeedBack,    // 关闭TopBar,关闭其他界面
}

public enum UIWindowColliderMode
{
    None,           // 显示该界面不包含碰撞背景
    Transparent,    // 碰撞透明背景
    WithBg,         // 碰撞非透明背景
}


/// <summary>
/// 界面ID枚举
/// </summary>
public enum WindowID
{
    Invaild = 0,
    FloatMessage = 1,
    Alert = 2,
    Loading = 3,
    UILogin = 4,
    UIMain = 5,
    UIToolTip = 6
}


