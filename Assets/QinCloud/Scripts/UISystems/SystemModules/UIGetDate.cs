using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

public class UIGetDate : MonoBehaviour {

	// Use this for initialization
  
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
       
    }

    [ContextMenu("GetDate")]
    void GetDate()
    {
        LuaDay day = new LuaDay();
        Debug.Log(day.GetLunarCalendar(DateTime.Now)[LuaDay.DayType.Day]);
        string s = "";
        string a = "";
        LuaDay.CalcConstellation(DateTime.Now,out s,out a);
        Debug.Log(s + " " + a);
    }
}
