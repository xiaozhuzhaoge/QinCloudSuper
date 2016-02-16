using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public enum GameOOType
{
   UI = 1,
   Scene = 2,
   Players = 3,
   Effect = 4,
   Music = 5
}


public class SceneManager : MonoBehaviour {

    public static GameObject couroutineObj = null;
    public static SceneManager instance;
    public BetterList<GameObject> clearOnChangeScene = new BetterList<GameObject>();
    public Dictionary<GameOOType, BetterList<GameObject>> groupBuffer = new Dictionary<GameOOType, BetterList<GameObject>>();

    public event Action OnLoadedEnd;
    public string loadLevel;
    public string targetSundries;
    public SceneContainer curScene;

    public Transform senceRoot;
    public Transform cacheRoot;
    public Transform uiRoot;
    public Transform playerRoot;
    public Transform ghostRoot;


    void Awake()
    {
        instance = this;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator InitScene()
    {
        yield return new WaitForSeconds(0.01f);

    }

    /// <summary>
    /// Stop InitCoroutine
    /// </summary>
    public void StopInitScene()
    {
        StopCoroutine("InitScene");
    }


    /// <summary>
    ///  Clear All Cached Clone Objects in the Scene
    /// </summary>
    public void ClearAllCache()
    {
        groupBuffer.Clear();
        foreach (GameObject go in clearOnChangeScene)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }

        clearOnChangeScene.Clear();
    }


    [ContextMenu("Test1")]
    public void LoadTest()
    {
        LoadSceneByConfig("Login","");
    }
    /// <summary>
    /// 读取场景配置
    /// </summary>
    /// <param name="UI">场景UI标识</param>
    /// <param name="level"></param>
    public void LoadSceneByConfig(string UI,string level)
    {
        Utility.StartSceneCoroutine<string, string>(LoadProcess, UI, level);
    }

    IEnumerator LoadProcess(string ui, string level)
    {
        ClearAllCache();
        yield return new WaitForSeconds(0.01f);
        yield return Resources.UnloadUnusedAssets();
        GC.Collect();

        yield return new WaitForSeconds(0.01f);

        if(!String.IsNullOrEmpty(level))
        {
            StartCoroutine(LoadLevelAsyc());
            targetSundries = ui;
        }
        else
        {
            LoadSundries(ui);
        }

    }
    /// <summary>
    /// Asyc loading
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadLevelAsyc()
    {
        AsyncOperation asyncOp = Application.LoadLevelAsync(loadLevel);
        yield return asyncOp;
    }

    void LoadSundries(string target)
    {
        if(!String.IsNullOrEmpty(target))
        {
            LoadEffect(target);
            LoadGUI(target);
            LoadSound(target);
        }

        if (OnLoadedEnd != null)
        {
            OnLoadedEnd();
        }
    }
    /// <summary>
    /// LoadGUI perfab
    /// </summary>
    /// <param name="target"></param>
    void LoadGUI(string target)
    {
        print("start LoadGUI=\t" + Time.realtimeSinceStartup);
        foreach(string gui in ConfigFactory.sundries["ui"][target])
        {
            string curGUI = gui;

            LoadSceneGUI(curGUI);
        }
    }

    /// <summary>
    /// LoadEffect perfab
    /// </summary>
    /// <param name="target"></param>
    void LoadEffect(string target)
    {

    }


    /// <summary>
    /// Load Sound
    /// </summary>
    /// <param name="target"></param>
    void LoadSound(string target)
    {

    }

    List<string> specialUi = new List<string>();

    void SetUiObject(GameObject sceneGui, Action callback)
    {
        sceneGui.transform.parent = uiRoot;
        sceneGui.transform.rotation = Quaternion.identity;
        sceneGui.transform.localPosition = Vector3.zero;
        sceneGui.transform.localScale = Vector3.one;
        SceneManager.instance.clearOnChangeScene.Add(sceneGui);
        if (callback != null)
        {
            callback();
        }
    }

    public void LoadSceneGUI(string guiName, Action callback = null)
    {
        Utility.instance.LoadGameObject(guiName, (scene) =>
        {
            SetUiObject(scene, callback);
        });
    }

    public void LoadSceneGUIAsync(string guiName, Action callback = null)
    {
        Utility.instance.LoadGameObjectAsync(guiName, (scene) =>
        {
            SetUiObject(scene, callback);
        });
    }


 }
