using UnityEngine;
using System;
using System.Collections;

public class ManagedLabel : MonoBehaviour
{
	public UILabel target;
	public int id;
	string content = null;

	void Start ()
	{
        if (target == null)
            target = this.GetComponent<UILabel>();
		content = GetVal ();
		if (target != null) {
			target.text = content;
		}
	}

	public string SetParams (params object[] param)
	{
		content = LocaleConfig.Get (id, param);
		if (target != null) {
			target.text = content;
		}
		return content;
	}

	public string GetVal ()
	{
		if (content != null) {
			return content;
		}
		return LocaleConfig.Get (id);
	}   
}
