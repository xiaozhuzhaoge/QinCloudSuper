using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIOne : UImode ,IWinAnimation
{
    public static UImode Instance;
    public GameObject itemPre;
    public UIGrid tran;
    public TweenTransform tt;
    public AsyncTexture texture;

    public override void Awake()
    {
        base.Default();
        Instance = this;
        SetMyTarget(this.gameObject);
        type = typeof(UIOne);
        currentMode = this;
        onDestory += DestoryEvent;
    }

    
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void InitWindow()
    {
        base.InitWindow();
        ResetAnimation();
    }

    public void EnterAnimation(EventDelegate.Callback onComplete)
    {
        if (tt == null)
            return;
        tt.PlayForward();
        if (onComplete == null)
            return;

        EventDelegate.Set(tt.onFinished, onComplete);
    }

    public void QuitAnimation(EventDelegate.Callback onComplete)
    {
        if (tt == null)
            return;
        tt.PlayReverse();
        if (onComplete == null)
            return;

        EventDelegate.Set(tt.onFinished, onComplete);
    }

    public void ResetAnimation()
    {
        tt.ResetToBeginning();
    }

    public override void OpenWindow(Action<GameObject> preback = null)
    {
        base.OpenWindow(gameObject => { EnterAnimation(delegate { texture.Test(); }); });
    }

    public override void CloseWindow(Action<GameObject> callback = null)
    {
        QuitAnimation(delegate { base.CloseWindow(callback);  });
        
    }

    public override void DestoryWindow()
    {
        base.DestoryWindow();
    }

    public void Test()
    {
        List<GameObject> games = new List<GameObject>();
        Utility.IntilizationBlocks<UIGrid>(tran, 10, itemPre, games, false);
        IEnumerable<UIGrid> grids = games.Where(go => go.GetComponent<UIGrid>() != null).Select(go => go.GetComponent<UIGrid>());
        Utility.ResetAllGrids<List<UIGrid>>(grids.ToList());
        Utility.ActiveAllObjects<List<GameObject>>(games, true);
        tran.Reposition();
        tran.repositionNow = true;
    }

    public void ShowTwo()
    {
        //UITwo.Instance.OpenWindow((go) => { UITwo.Instance.previousMode = this.currentMode; });
        GUIManager.ShowGUI<UITwo>(UITwo.Instance,"UITwo");
    }


    public void ShowThree()
    {
        GUIManager.ShowGUI<UIThree>(UIThree.Instance,"UIThree");
    }

}
