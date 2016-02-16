using UnityEngine;
using System.Collections;

public class LoadingPanel : MonoBehaviour {

    UIPanel mpanel;
    public static LoadingPanel instance;
    public SpeedProcess progress;
    public GameObject loading;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   
	}

    public void ShowLoading()
    {
        loading.SetActive(true);
    }

    public void CloseLoading()
    {
        loading.SetActive(false);
    }
    
    /// <summary>
    /// 设置当前进度条
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="direct"></param>
    public void SetLoadingPercent(int percent, bool direct = false)
    {
        float value = percent / 100f;
        if (value < progress.bar.value || direct)
        {
            progress.bar.value = value;
            progress.targetValue = value;
        }
        else
        {
            progress.targetValue = value;
        }
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        SetLoadingPercent(0, true);
    }


    [ContextMenu("Test")]
    public void Test()
    {
        SetLoadingPercent(60, false);
    }
}
