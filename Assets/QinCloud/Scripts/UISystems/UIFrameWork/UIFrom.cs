using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CellIndex
{
    public int x;
    public int y;
    public CellIndex(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class UIFrom : MonoBehaviour {

	// Use this for initialization
    public UIScrollView scrollView;
    public UIScrollBar bar;
    public UIGrid rowG;
    public UITable columeT;
    public GameObject item;
    public Dictionary<UIGrid, List<UICell>> cells = new Dictionary<UIGrid, List<UICell>>();

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateForm(int row , int colume)
    {
        for (int j = 0; j < row; j++)
        {
            Utility.createObjCallBack(columeT.transform
                ,rowG.gameObject
                ,true
                ,j.ToString()
                , (obj) => { cells.Add(obj.GetComponent<UIGrid>(), new List<UICell>()); 
                
                  for(int i = 0 ; i < colume ; i++)
                    {
                        Utility.createObjCallBack(obj.transform, item, true, i.ToString(), (cell) => { 
                            cells[obj.GetComponent<UIGrid>()].Add(cell.GetComponent<UICell>()); });
                    }
                });
        }
        foreach(var data in cells)
        {
            data.Key.repositionNow = true;
            data.Key.Reposition();
        }

        columeT.Reposition();
        columeT.repositionNow = true;
    }

    [ContextMenu("TestForm")]
    public void Test()
    {
        CreateForm(20, 6);
    }
}
