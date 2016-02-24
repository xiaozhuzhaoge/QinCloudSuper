using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIMenu : MonoBehaviour {

    public List<UIButton> buttons;
    public List<UISprite> fornts;
    public int currentIndex;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenView()
    {
        gameObject.SetActive(true);
    }
    public void CloseView()
    {
        gameObject.SetActive(false);
    }
    public void SetTab(int i)
    {
        fornts.ForEach(fornt => fornt.gameObject.SetActive(false));
        fornts[i].gameObject.SetActive(true);
    }
    public void Reset()
    {
        SetTab(0);
    }
    public void SetAllButtonEvent()
    {
        buttons.ForEach(button => button.onClick.Add(new EventDelegate(this, "SetTabByButton")));
    }

    public void SetTabByButton()
    {
		string name=UICamera.lastHit.collider.name;
		name=(name.Split("(Clone)".ToCharArray()))[0];
		currentIndex = Convert.ToInt32(name) ;
        SetTab(currentIndex);
    }
}
