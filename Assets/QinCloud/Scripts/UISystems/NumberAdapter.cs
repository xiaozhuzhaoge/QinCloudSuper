using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NumberAdapter : MonoBehaviour {

    public UITable table;
    public List<UISprite> numbers;
    public Char [] currentNumberChar;
    public int getNumber;

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    Char [] getStringByInt(long numbers)
    {
        string value = Convert.ToString(numbers);
        Char[] chars = value.ToCharArray();
        return chars;
    }

    public void SetValues(long number)
    {
        Char[] cs = getStringByInt(number);
        for (int i = 0; i < numbers.Count; i++)
        {
            numbers[i].gameObject.SetActive(false);
        }
        if (number == -1)
            return;
        for (int i = 0; i < cs.Length; i++)
            numbers[i].gameObject.SetActive(true);
        for (int i = 0; i < cs.Length; i++)
            numbers[i].spriteName = cs[i].ToString();
        table.Reposition();
    }
}
