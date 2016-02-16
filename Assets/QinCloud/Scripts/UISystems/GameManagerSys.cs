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

    public float sev;

    public float bgmv;
   
    void Update()
    {
        SEValue = sev = Mathf.Clamp01(sev);
        BGMValue = bgmv =  Mathf.Clamp01(bgmv);
    }


	
}
