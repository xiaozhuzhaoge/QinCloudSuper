using UnityEngine;
using System;
using System.Collections;

public class BufferPlay : MonoBehaviour
{
	[HideInInspector]
	public static int index=1;
	public static int changeAfterFrame=6;
	static float pastTime=0.0f;
	void Update ()
	{
		pastTime += Time.deltaTime;
		if(pastTime<(0.01f*changeAfterFrame))
		{
			return;
		}
		pastTime = 0.0f;

		if(index>12)
		{
			index=1;
		}
		gameObject.transform.GetComponent<UISprite> ().spriteName = Convert.ToString (index);
		index++;
	}
}
