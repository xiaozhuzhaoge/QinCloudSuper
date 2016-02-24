using UnityEngine;
using System.Collections;

public class GameManagerSys : MonoBehaviour {

    public static GameManagerSys instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }
    /// <summary>
    /// 音效音量
    /// </summary>
    public static float SEValue;
    /// <summary>
    /// 音乐音量
    /// </summary>
    public static float BGMValue = 1f;

    void Update()
    {
       
    }


    public void SetSeValue(float value)
    {
        SEValue = Mathf.Clamp01(value);
    }

    public void SetBGMValue(float value)
    {
        BGMValue = Mathf.Clamp01(value);
    }

	
}
