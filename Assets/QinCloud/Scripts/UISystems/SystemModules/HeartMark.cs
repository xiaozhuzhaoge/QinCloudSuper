using UnityEngine;
using System.Collections;

public class HeartMark : MonoBehaviour {

    public TweenColor tc;
    public TweenScale sl;
    public bool isConnected
    {
        set {
            if (flag != value)
                SetState(value);
            flag = value;
        }
        get { return flag; }
    }

    bool flag = false;


	// Use this for initialization
	void Start () {
        tc.enabled = false;
        sl.enabled = false;
        SetState(false);
	}
	
	// Update is called once per frame
	void Update () {
        isConnected = (Connector.pc == null) ? false : Connector.pc.IsConnect;
	}

    void SetState(bool state)
    {
        if (state)
        {
            tc.enabled = true;
            tc.PlayReverse();
            sl.enabled = true;
        }
        else
        {
            tc.PlayForward();
            sl.PlayReverse();
            sl.enabled = false;
        }
    }




   
}
