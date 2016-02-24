using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//用于Tables Tag托管
public class UITags : MonoBehaviour {

    public List<GameObject> fornts;
    public List<UIButton> backs;
    public bool defaultState;

    void Start()
    {
        if(defaultState)
            DefaultState();
        SetDelegatesToButton();
    }
	// Use this for initializatio
    public void SelectIndex(int index)
    {
        fornts.ForEach(tags => tags.gameObject.SetActive(false));
        fornts[index].gameObject.SetActive(true);
    }

    public void SelectByName()
    {
        int index = Convert.ToInt32(UICamera.lastHit.collider.name);
        SelectIndex(index);
    }
    public void DefaultState()
    {
        SelectIndex(0);
    }
    void SetDelegatesToButton()
    {
        backs.ForEach(btr => btr.onClick.Add(new EventDelegate(this, "SelectByName")));
    }

    public void SetButtonEvent(int btrIndex, EventDelegate function)
    {
        backs[btrIndex].GetComponent<UIButton>().onClick.Add(function);
    }

}
