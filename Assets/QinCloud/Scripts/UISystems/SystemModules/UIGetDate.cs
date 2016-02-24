using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

public class UIGetDate : MonoBehaviour {

	// Use this for initialization
    ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();
   
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
       print(calendar.GetMonth(DateTime.Now));
    }
}
