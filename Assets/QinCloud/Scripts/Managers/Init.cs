using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour {

    SceneManager manager;
    Utility tool;
    Connector connector;
    UICamera uicamera;
    public bool playInBackGround = true;

    void Awake()
    {
        InitFuc();
    }
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// Init all roots and configs
    /// </summary>
    void InitFuc()
    {
        Application.runInBackground = playInBackGround;

        manager = this.gameObject.AddComponent<SceneManager>();
        manager.cacheRoot = transform.FindChild("CacheRoot");
        manager.uiRoot = transform.FindChild("UI Root");
        manager.senceRoot = transform.FindChild("SceneRoot");
        manager.playerRoot = transform.FindChild("PlayerRoot");
        manager.ghostRoot = transform.FindChild("GhostRoot");
        uicamera = manager.uiRoot.FindChild("Camera").GetComponent<UICamera>();
        tool = this.gameObject.AddComponent<Utility>();
        connector = this.gameObject.AddComponent<Connector>();
        
        ConfigFactory.InitResourceConfig();
        ConfigFactory.ReadSystemConfig();
        
        this.gameObject.AddComponent<ConfigInfo>();

        ConfigInfo.ReadUIRules();
        GUIManager.instance.Loading.gameObject.SetActive(true);
        GUIManager.instance.FloatMessagePanel.gameObject.SetActive(true);


        GUIManager.LoadingPercent(10);
        ConfigInfo.Init();
        ConfigInfo.InitPreGameConfigs();
        GUIManager.FinishLoading();
      
        
        StartGame();
        
    }
    [ContextMenu("TestLogin")]
    void LoadLogin()
    {
        manager.LoadSceneByConfig("Login","");
    }

    [ContextMenu("TestMain")]
    void LoadMain()
    {
        manager.LoadSceneByConfig("Main", "");
    }


    void StartGame()
    {
        LoadLogin();
    }



}
