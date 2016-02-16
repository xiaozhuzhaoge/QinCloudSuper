using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using SimpleJson;

public class CacheFactory : MonoBehaviour {

    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    public static readonly string PathURL =
#if UNITY_ANDROID   //安卓
	"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE  //iPhone
	 Application.dataPath +"/Raw"
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
     Application.dataPath + "/StreamingAssets";
#else
     string.Empty;
#endif

    public enum PictureType{
        JPG,
        PNG
    }

    public enum TextFileType
    {
        Txt,
        Json,
        Excel
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 保存未图片
    /// </summary>
    /// <param name="icon"></param>
    /// <param name="prefix"></param>
    /// <param name="type">图片格式，PNG,JPG</param>
    public static void SaveToPicture(Texture2D icon , string prefix , PictureType type )
    {
        byte[] bytes = null;
        if (type == PictureType.PNG)
            bytes = icon.EncodeToPNG();
        else if (type == PictureType.JPG)
            bytes = icon.EncodeToJPG();
        if (bytes == null)
            return;

        CreateDir(PathURL);
        string filename = PathURL + prefix;
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("保存一张图片: {0}", filename));
    }


    public static void SaveToFile(string content, string prefix, TextFileType type)
    {
        string filename = PathURL + prefix;
        if (type == TextFileType.Json)
            File.WriteAllText(filename, getJson(content));
        else if (type == TextFileType.Txt)
            File.WriteAllText(filename, content);
     
    }

    public static string getJson(string content)
    {
        return (string)SimpleJson.SimpleJson.DeserializeObject(content);
    }


    //创建路径
    public static bool CreateDir(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return true;
        }
        return false;
    }

}
