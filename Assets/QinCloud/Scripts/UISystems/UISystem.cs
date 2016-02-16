using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UISystem : MonoBehaviour
{
    /// <summary>
    /// 创建单一对象
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="itemPre"></param>
    /// <param name="switchFlag"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject createObj(Transform parent, GameObject itemPre , bool switchFlag,string name)
    {
        GameObject gameO = (GameObject)GameObject.Instantiate(itemPre);
        gameO.transform.parent = parent;
        gameO.transform.localScale = Vector3.one;
        gameO.transform.localPosition = Vector3.one;
        gameO.transform.localRotation = Quaternion.identity;

        gameO.SetActive(switchFlag);
        gameO.name = name;
        return gameO;
    }
    /// <summary>
    /// 带回调的创建对象
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="scale"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="itemPre"></param>
    /// <param name="switchFlag"></param>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public static void createObjCallBack(Transform parent,Vector3 scale, Vector3 position , Quaternion rotation, GameObject itemPre, bool switchFlag, string name , Action<GameObject> callback = null)
    {
        GameObject gameO = (GameObject)GameObject.Instantiate(itemPre);
        gameO.transform.parent = parent;
        gameO.transform.localScale = scale;
        gameO.transform.localPosition = position;
        gameO.transform.localRotation = rotation;

        gameO.SetActive(switchFlag);
        gameO.name = name;
        if(callback != null)
            callback(gameO);
    }

   
    /// <summary>
    /// 生成格子
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="grid"></param>
    /// <param name="number"></param>
    /// <param name="itemPre"></param>
    /// <param name="list"></param>
    /// <param name="switchFlag"></param>
    /// <param name="callback"></param>
    /// 
    public static void IntilizationBlocks<T> (T grid, int number, GameObject itemPre, List<GameObject> list , bool switchFlag = true, Action<T> callback = null) where T : MonoBehaviour
    {
        for (int i = 0; i < number; i++)
        {
            list.Add(createObj(grid.transform, itemPre, switchFlag, i.ToString()));
        }
        if(callback != null)
        {
            callback(grid);
        }
    }

    public static void ActiveAllObjects(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
            list[i].gameObject.SetActive(true);
    }

    public static void ResetAllGrids(UIGrid[] grids)
    {
        for (int i = 0; i < grids.Length; i++)
            grids[i].Reposition();
    }

    public static void ResetAllScrollView(UIScrollView[] views)
    {
        for (int i = 0; i < views.Length; i++)
            views[i].ResetPosition();
    }

    public static void UpdateTabs(List<GameObject> tabs, List<GameObject> cTabs)
    {
        int cuu = 0;
        try
        {
            cuu = Convert.ToInt32(UICamera.lastHit.collider.name);
        }
        catch (Exception exp)
        {
            cuu = 1;
            Debug.Log(exp);
        }
        for (int i = 0; i < cTabs.Count; i++)
        {
            if (i == cuu)
                cTabs[i].gameObject.SetActive(true);
            else
                cTabs[i].gameObject.SetActive(false);
        }
    }

    public static void IntilizationTabs(List<GameObject> tabs, List<GameObject> cTabs, int current)
    {
        for (int i = 0; i < cTabs.Count; i++)
        {
            if (i == current)
                cTabs[i].gameObject.SetActive(true);
            else
                cTabs[i].gameObject.SetActive(false);
        }

    }

    public static void RefreshScrollViews(UIScrollView[] sws, int currentIndex)
    {
        for (int i = 0; i < sws.Length; i++)
            sws[i].gameObject.SetActive(false);
        sws[currentIndex].gameObject.SetActive(true);
    }
    public static void SetOriginalStateForScrollViews(UIScrollView[] sws)
    {
        for (int i = 0; i < sws.Length; i++)
            sws[i].ResetPosition();
    }


    public static void ResetGameItemColor(UISprite back)
    {
        back.color = Color.white;
    }

    public static void rightShiftScrollView(UIScrollView scrollView, UIGrid grid)
    {
        float weight = grid.cellWidth;
        Vector3 currentPosition = scrollView.transform.localPosition;
        scrollView.transform.localPosition = new Vector3(currentPosition.x + weight, currentPosition.y, currentPosition.z);
        scrollView.GetComponent<UIPanel>().clipOffset = new Vector3(currentPosition.x + weight, currentPosition.y, currentPosition.z);
    }
    public static void leftShiftScrollView(UIScrollView scrollView, UIGrid grid)
    {
        float weight = grid.cellWidth;
        Vector3 currentPosition = scrollView.transform.localPosition;
        scrollView.transform.localPosition = new Vector3(currentPosition.x - weight, currentPosition.y, currentPosition.z);
        scrollView.GetComponent<UIPanel>().clipOffset = new Vector3(currentPosition.x - weight, currentPosition.y, currentPosition.z);
    }

    public static GameObject RaysEvent(UIWidget rayOrignal, Camera camera, string rayCollider)
    {
        Ray center = camera.ScreenPointToRay(camera.WorldToScreenPoint(rayOrignal.transform.position));
        RaycastHit[] centerContant;
        centerContant = Physics.RaycastAll(center, 20, 1 << 8);
        foreach (var o in centerContant)
            if (o.collider.name == rayCollider)
                return o.collider.gameObject;
        return null;
    }

    public static void SetAllSpriteForButton(UIButton button, string spriteName)
    {
        button.normalSprite = spriteName;
        button.pressedSprite = spriteName;
        button.hoverSprite = spriteName;
        button.disabledSprite = spriteName;
    }

}
