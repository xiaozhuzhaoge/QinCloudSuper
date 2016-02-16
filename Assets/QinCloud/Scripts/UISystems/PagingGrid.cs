using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PagingGrid : MonoBehaviour {

    public UIGrid grid;
    public UIScrollView scrollView;
    public GameObject item;
    public List<GameObject> items;
    Vector3 topWorldCenter;
    Vector3[] worldCorners;
    float height;
    public float factor;

	// Use this for initialization
	void Start () {
        worldCorners = scrollView.panel.worldCorners;
        topWorldCenter = (worldCorners[0] + worldCorners[2]) * 0.5f;
        height = (worldCorners[1].y - worldCorners[0].y) * 0.5f;
        factor = 2f;
        
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(Mathf.Abs(items[i].transform.position.y - topWorldCenter.y) <= height * factor);
        }
	}
    
    [ContextMenu("Execute")]
    void CreateItems()
    {
        UISystem.IntilizationBlocks(grid, 10, item, items);
    }

    public void SetItems(List<GameObject> varb)
    {
        Clear();
        items = varb;
    }

    //void OnEnable()
    //{
    //    items = new List<GameObject>();
    //    foreach(var data in this.grid.GetChildList())
    //    {
    //        items.Add(data.gameObject);
    //    }
    //}

    //void OnDisable()
    //{
    //    items = null;
    //}
    public void Clear()
    {
        items = null;
    }
}
