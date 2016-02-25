using UnityEngine;
using System.Collections;

public class SpeedProcess : MonoBehaviour {

    float smooth = 3;
    public float targetValue = 1;
    public UIProgressBar bar;

	// Use this for initialization
	void Start () {
	  
	}
	
	// Update is called once per frame
	void Update () {
        bar.value = Mathf.Lerp(bar.value, targetValue, Time.fixedDeltaTime * smooth);
        if (Mathf.Abs(targetValue - bar.value) <= 0.02f)
        {
            bar.value = targetValue;
        }
	}

    public void Reset()
    {
        bar.value = 0f;
    }
}
