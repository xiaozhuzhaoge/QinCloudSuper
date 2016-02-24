using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/**
 * 本类用于继承
 * 包含了拥有分页显示的滚轴视窗
 * 主要函数
 *  CreatePages(int PagesNumber,int eachPageItemsNumber)
 *  参数为生成页数与每页的单元数
 *  开启createDot开关，生成分页按钮点击进行页面切换
 * */
public class UIPages : MonoBehaviour {
    
    [HideInInspector]
    public List<UIGrid> EachPreGridsForPage = new List<UIGrid>();
    
    public GameObject EachItemPreForPage;//页面的单元的预设

    //以下参数用于生成新的包含分页的拖拽ScrollView
    public UIScrollView newItemsScrollView;//滚轴窗口
    public UIGrid newItemsGrid;//滚轴对应的单元格框
    public GameObject PagePre;//页面的预设
    [HideInInspector]
    public List<GameObject> Pages = new List<GameObject>();
     [HideInInspector]
    public List<GameObject> PagesItems = new List<GameObject>();
     [HideInInspector]
    public List<GameObject> Dots = new List<GameObject>();
    public GameObject DotPre;//切换页面按钮的预设
    public UIGrid DotGrid;
    public bool createDot = false;

    public float SmoothMove = 30.0f;
    public float ActiveDotScale = 1.3f;
    public float NotActiveDotScale = 1.0f;

	// Use this for initialization
    void Awake()
    {
        gameObject.SetActive(true);
    }
	void Start () {
        CreatePagesByTotalAmount(51,6);
	}
	
	// Update is called once per frame
	void Update () {
        if (newItemsGrid.GetComponent<UICenterOnChild>().centeredObject != null)
        {
            ScaleButton();
            newItemsGrid.GetComponent<UICenterOnChild>().springStrength = SmoothMove;
        }
	}

    public void CreatePagesByTotalAmount(int totalAmount,int EachPageAmount)
    {
        int TestNum = totalAmount / EachPageAmount;
        if (totalAmount % EachPageAmount != 0)
            TestNum++;
        Debug.Log(TestNum);
        CreatePages(totalAmount/EachPageAmount, EachPageAmount);
        int add = EachPageAmount * TestNum - totalAmount;
        Debug.Log(add);
    }

    void CreatePages(int PagesNumber,int eachPageItemsNumber)
    {
        EachPreGridsForPage.Clear();
        CreateItemScrollView(PagesNumber);
       
        for (int i = 0; i < Pages.Count; i++)
        {
           EachPreGridsForPage.Add(Pages[i].transform.FindChild("Grid").GetComponent<UIGrid>());
           for(int j = 0; j<eachPageItemsNumber ; j++)
           {
               GameObject clone = (GameObject)GameObject.Instantiate(EachItemPreForPage);
               clone.transform.parent = EachPreGridsForPage[i].transform;
               clone.transform.localScale = Vector3.one;
               clone.name = (i*eachPageItemsNumber + j).ToString();
               PagesItems.Add(clone);
           }
        }
        PagesItems.ForEach(gameObject => gameObject.SetActive(true));

        if (createDot)
        {
            GreateDots();
            ScaleButton();
        }
          
    }

    public void CreateItemScrollView(int PageNumber)
    {
        UISystem.IntilizationBlocks<UIGrid>(newItemsGrid, PageNumber, PagePre, Pages);
    }

    void GreateDots()
    {
        Dots.Clear();
        int createNumber = Pages.Count;
        for (int i = 0; i < createNumber; i++)
        {
            GameObject dotClone = (GameObject)GameObject.Instantiate(DotPre.gameObject);
            dotClone.transform.parent = DotGrid.transform;
            dotClone.transform.localScale = Vector3.one;
            dotClone.name = i.ToString();
            Dots.Add(dotClone);
            DotGrid.AddChild(dotClone.transform);
        }
        DotGrid.Reposition();
        Dots.ForEach(gameObject => gameObject.SetActive(true));
    }

    public void SwitchPage()
    {
        int index = Convert.ToInt32(UICamera.lastHit.collider.name);
        newItemsGrid.GetComponent<UICenterOnChild>().CenterOn(Pages[index].transform);
        ScaleButton();
    }
    public void ScaleButton()
    {
        if(newItemsGrid.GetComponent<UICenterOnChild>().centeredObject != null)
        {
            int index = Convert.ToInt32(newItemsGrid.GetComponent<UICenterOnChild>().centeredObject.name);

            Dots.ForEach(gameObject =>
            {
                if (gameObject.name == index.ToString())
                    gameObject.transform.localScale = Vector3.Lerp(new Vector3(ActiveDotScale, ActiveDotScale, ActiveDotScale),
                        new Vector3(NotActiveDotScale, NotActiveDotScale, NotActiveDotScale), 0.01f * Time.deltaTime);
                else
                    gameObject.transform.localScale = Vector3.Lerp(new Vector3(NotActiveDotScale, NotActiveDotScale, NotActiveDotScale),
                        new Vector3(ActiveDotScale, ActiveDotScale, ActiveDotScale), 0.01f * Time.deltaTime);
            });
        }
    }

    public void OpenView()
    {
        gameObject.SetActive(true);
    }

    public void CloseView()
    {
        Debug.Log("UIBase Close");
        gameObject.SetActive(false);
    }
}
