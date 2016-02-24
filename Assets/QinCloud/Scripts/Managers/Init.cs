using UnityEngine;
using System.Collections;
using SimpleJson;
using System;

public class Init : MonoBehaviour {

    public SceneManager manager;
    public Utility tool;
    public Connector connector;
    public UICamera uicamera;
    public bool playInBackGround = true;
    public bool enableDebuger = true;

    public static Init instance;


    void Awake()
    {
        instance = this;
        InitFuc();
    }
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {
       
    }
    /// <summary>
    /// Init all roots and configs
    /// </summary>
    void InitFuc()
    {
        
        Application.runInBackground = playInBackGround;
        Debuger.EnableLog = enableDebuger;
        manager = this.gameObject.AddComponent<SceneManager>();
        manager.cacheRoot = transform.FindChild("CacheRoot");
        manager.uiRoot = transform.FindChild("UI Root");
        manager.senceRoot = transform.FindChild("SceneRoot");
        manager.playerRoot = transform.FindChild("PlayerRoot");
        manager.ghostRoot = transform.FindChild("GhostRoot");
        uicamera = manager.uiRoot.FindChild("Camera").GetComponent<UICamera>();
        tool = this.gameObject.AddComponent<Utility>();
        connector = this.gameObject.AddComponent<Connector>();
        this.gameObject.AddComponent<CacheFactory>();
        ConfigFactory.InitResourceConfig();
        ConfigFactory.ReadSystemConfig();
        
        this.gameObject.AddComponent<ConfigInfo>();

        ConfigInfo.ReadUIRules();
        GUIManager.instance.Loading.gameObject.SetActive(true);
        GUIManager.instance.FloatMessagePanel.gameObject.SetActive(true);
        GUIManager.instance.FloatTip.gameObject.SetActive(true);

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

    [ContextMenu("TestLoad")]
    public void LoadTest()
    {
       CacheFactory.instance.StartLoadPicture("-2020523228");
    }

    public void GetTime()
    {
        DateTime t1 = new DateTime(2010, 2, 2);
        GUIManager.FloatMessage(TimeManager.GetUTCTime().ToString());
        GUIManager.FloatMessage(TimeManager.GetUTCTime().ToString());
    }

    public void TestConnect()
    {
        connector.ConnectWebSocket();
    }

    public void TestSendMessage()
    {
        connector.SendAMessageWS();
    }

    public void TestDisConnect()
    {
        if(Connector.pc != null)
        Connector.pc.disconnect();
    }

}
