using UnityEngine;
using System.Collections;

interface IWinAnimation {
    /// <summary>
    /// 显示播放动画
    /// </summary>
    void EnterAnimation(EventDelegate.Callback onComplete);
    /// <summary>
    /// 退出播放动画
    /// </summary>
    void QuitAnimation(EventDelegate.Callback onComplete);
    /// <summary>
    /// 重置动画
    /// </summary>
    void ResetAnimation();
}
