using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/**
 * 本类用于继承
 * 包含了生成标签页以及滚轴视窗的一系列元素
 * 可以生成指定单元的滚轴视窗
 * 多标签的指定单元滚轴视窗
 * 主要函数
 *  CreateGrid 包含一个重载用于生成多标签滚轴视窗
 *  CreateTable
 * */
public class UIComplex : MonoBehaviour {

    public UIGrid grid;//用于不含有标签页单元格
    public UIScrollView scrollView;//用于不含有标签页的滚轴窗口
    
    public UIScrollView tabScrollView;//用于生成标签页的滚轴窗口

    public GameObject itemPre;//itemPre 用于复制单元元素
    public GameObject tablePre;//tablePre 用于复制标签元素点击之前
    public GameObject tableCPre;//tableCPre 用于复制标签元素点击之后
    public UITable table;//生成标签页点击之前的状态的Table
    public UITable tableC;//生成标签页点击之后的状态的Table
    
    public bool dragScrollView;//滚轴视窗拖动开关
    public bool dragTabScrollView;//标签拖动开关

    public float CreateTime;//创建的延迟时间
    public float RefreshTime;//刷新的延迟时间

    [HideInInspector]
    public List<GameObject> list;
    [HideInInspector]
    public List<GameObject> tabs;
    [HideInInspector]
    public List<GameObject> tabsC;

    public GameObject clonePoint;//生成一系列控件的地方
    [HideInInspector]
    public List<List<GameObject>> allitemList;
    [HideInInspector]
    public List<UIScrollView> scrollViews;
    [HideInInspector]
    public List<UIGrid> grids;
    [HideInInspector]
    public List<int> numbers;

    public bool ClickViewSelect;
    public bool defaultSelect;
    public int currentClick;


    void Awake()
    {
        gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenView()
    {
        gameObject.SetActive(true);
     
    }
    
    protected void CloneElements(UIGrid grid , UIScrollView scrollView,int time)
    {
        for (int i = 0; i < time; i++)
        {
            GameObject cloneScrollView = (GameObject)GameObject.Instantiate(scrollView.gameObject);
            cloneScrollView.transform.parent = clonePoint.transform;
            cloneScrollView.transform.localPosition = scrollView.transform.localPosition;
            cloneScrollView.transform.localScale = scrollView.transform.localScale;
            scrollViews.Add(cloneScrollView.GetComponent<UIScrollView>());
			cloneScrollView.GetComponent<UIScrollView>().ResetPosition();
            cloneScrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
            cloneScrollView.transform.localPosition = new Vector2(cloneScrollView.transform.localPosition.x, 0);
            grids.Add(cloneScrollView.transform.FindChild("Grid").GetComponent<UIGrid>());
        }
    }
    
    public void CloseView()
    {
        gameObject.SetActive(false);
    }

    public void CreateGrid(int number)
    {
        list = new List<GameObject>();
        Utility.IntilizationBlocks<UIGrid>(grid, number, itemPre, list);
        if(this.GetComponent<PagingGrid>())
        {
            this.GetComponent<PagingGrid>().SetItems(list);
        }
        if (ClickViewSelect)
        {
            list.ForEach(btr => btr.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "UpdateSelect")));
        }
        if (scrollView != null) {
            Utility.instance.WaitForSecond(RefreshTime, () =>
            {
                scrollView.ResetPosition();
            });
        }
        if (dragScrollView)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].AddComponent<UIDragScrollView>().scrollView = scrollView;
            }
        }
        if (defaultSelect)
        DefaultSelect();
    }

    public void CreateGrid (List<int> numbers, List<UIGrid> grids , List<UIScrollView> scrollViews)
    {
        allitemList = new List<List<GameObject>>();
        for (int i = 0; i < numbers.Count; i++)
        {
            allitemList.Add(new List<GameObject>());
        }
        for (int i = 0; i < numbers.Count; i++)
        {
            Utility.IntilizationBlocks<UIGrid>(grids[i], numbers[i], itemPre, allitemList[i]);
        }
        for (int i = 0; i < scrollViews.Count; i++)
        {
            scrollViews[i].ResetPosition();
        }
        if (dragScrollView)
        {
            for (int i = 0; i < allitemList.Count; i++)
                for (int j = 0; j < allitemList[i].Count; j++ )
                    allitemList[i][j].AddComponent<UIDragScrollView>().scrollView = scrollViews[i];
        }
    }

    public void MuteAllScrollViews()
    {
        for (int i = 0; i < scrollViews.Count; i++)
        {
            scrollViews[i].ResetPosition();
            scrollViews[i].gameObject.SetActive(false);
        } 
    }

    public void SelectScrollView(int index)
    {
        if (index <= scrollViews.Count-1)
        {
			MuteAllScrollViews();
            scrollViews[index].ResetPosition();
            scrollViews[index].GetComponent<UIPanel>().clipOffset = Vector2.zero;
            scrollViews[index].transform.localPosition = new Vector2(scrollViews[index].transform.localPosition.x, 0);
			scrollViews[index].gameObject.SetActive(true);
        }
    }
    public void AdapteData()
    { //SetElements 
    }

    public void ClearList()
    {
        for (int i = 0; i < list.Count; i++)           
            Destroy(list[i].gameObject);
        list.Clear();
    }
   
    public void Refresh()
    {
        //RefreshView
    }

    public void CreateTable(int tableNumber)
    {
        UISystem.IntilizationBlocks<UITable>(table, tableNumber,tablePre, tabs);
        UISystem.IntilizationBlocks<UITable>(tableC, tableNumber, tableCPre, tabsC);
        if (dragScrollView)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].GetComponent<UIDragScrollView>() == null)
                    list[i].AddComponent<UIDragScrollView>().scrollView = scrollView;
            }
        }
        if (dragTabScrollView)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].GetComponent<UIDragScrollView>() == null)
                    tabs[i].AddComponent<UIDragScrollView>().scrollView = tabScrollView;
            }
        }
    }
     public void UpdateTableSelect()
    {
        int index = Convert.ToInt32(UICamera.lastHit.collider.name);
        UISystem.IntilizationTabs(tabs, tabsC, index);
    }
    
     public void UpdateSelect(Action me = null)
     {
         int index = Convert.ToInt32(UICamera.lastHit.collider.name);
         currentClick = index;
         list.ForEach(gameObject => gameObject.transform.FindChild("Select").gameObject.SetActive(false));
         list[index].gameObject.transform.FindChild("Select").gameObject.SetActive(true);
         if(me!=null)
            me();
     }
     public void UpdateSelectByIndex(int index)
     {
         //Debug.Log("Update");
         list.ForEach(gameObject => gameObject.transform.FindChild("Select").gameObject.SetActive(false));
         list[index].gameObject.transform.FindChild("Select").gameObject.SetActive(true);
         grid.Reposition();
     }

     public void DefaultSelect()
     {
         if(list.Count > 0)
             UpdateSelectByIndex(0);
     }
}
