using UnityEngine;
using System.Collections;
using System;

public class UITwo : UImode ,IWinAnimation
{
    public static UImode Instance;
    public TweenAlpha ta;
    public TweenTransform tt;


    public override void Awake()
    {
        base.Awake();
        base.Default();
        Instance = this;
        SetMyTarget(this.gameObject);
        type = typeof(UITwo);
        currentMode = this;
        onDestory += DestoryEvent;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {

    }



    public override void InitWindow()
    {
        base.InitWindow();
        ResetAnimation();
    }


    public void EnterAnimation(EventDelegate.Callback onComplete = null)
    {
        if (ta == null || tt == null)
            return;
        ta.PlayForward();
        tt.PlayForward();

        ta.onFinished.Clear();
        tt.onFinished.Clear();

        if (onComplete == null)
            return;
        EventDelegate.Set(tt.onFinished, onComplete);
    }

    public void QuitAnimation(EventDelegate.Callback onComplete = null)
    {
        if (ta == null || tt == null)
            return;
        ta.PlayReverse();
        tt.PlayReverse();

        tt.onFinished.Clear();
        ta.onFinished.Clear();
        
        if (onComplete == null)
            return;

        EventDelegate.Set(tt.onFinished, onComplete);
    }

    public void ResetAnimation()
    {
        ta.ResetToBeginning();
        tt.ResetToBeginning();
    }

    public override void OpenWindow(Action<GameObject> callback = null)
    {
        base.OpenWindow(gameobject => {
            EnterAnimation();
        }); 
    }


    public override void CloseWindow(Action<GameObject> callback = null)
    {
        QuitAnimation(delegate { base.CloseWindow(callback); });
    }
    public override void DestoryWindow()
    {
        base.DestoryWindow();
    }

}
