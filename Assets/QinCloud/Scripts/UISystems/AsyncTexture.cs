using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class AsyncTexture : MonoBehaviour
{
    public string url;
    public static string UITextureName;
    public Texture placeholder;
    public UITexture view;

    private static string path = Application.persistentDataPath + "/ImageCache/";  
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public AsyncTexture CreateSingleton()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        AsyncTexture loader = this.gameObject.GetComponent<AsyncTexture>();  
        loader.placeholder = Resources.Load(UITextureName) as Texture;
        return loader;  
    }

    public void SetAsyncImage(string url,UITexture texture,Action callback = null)
    {
        texture.mainTexture = placeholder;
        if (!File.Exists(path + url.GetHashCode()))
        {
            StartCoroutine(DownloadImage(url, texture, callback));
        }
        else
        {
            StartCoroutine(LoadLocalImage(url, texture, callback));
        }  
    }
    /// <summary>
    /// 创建至本地文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="texture"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator DownloadImage(string url, UITexture texture,Action callback = null)
    {
        Debug.Log("downloading new image:" + path + url.GetHashCode());
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(www.error);
        Texture2D image = www.texture;
        byte[] pngData = image.EncodeToPNG();
        File.WriteAllBytes(path + url.GetHashCode(), pngData);
        texture.mainTexture = image;
        if (callback != null)
            callback();
    }
    //读取网络资源
    IEnumerator LoadLocalImage(string url, UITexture texture, Action callback = null)
    {
        string filePath = "file:///" + path + url.GetHashCode();
        Debug.Log("getting local image:" + filePath);
        WWW www = new WWW(filePath);
        yield return www; 
        texture.mainTexture = www.texture;
        if (callback != null)
            callback();
    }  

    [ContextMenu("TestDownload")]
    public void Test()
    {
        CreateSingleton();
        SetAsyncImage(url, view);
    }
}
