using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using SimpleJson;
using System;
using System.Text;

public class GeTools : EditorWindow {

	public static GeTools window;
	[MenuItem ("QC/Window/工具箱")]
	static void Init ()
	{
		window = (GeTools)EditorWindow.GetWindow <GeTools> ("常用文本功能");
	}

    Vector2 spacing = new Vector2(1, 1);
    Vector2 distance = new Vector2(1, 1);
    UIAtlas atlas;
    string temp;

	void OnGUI()
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("文件功能");
		if (GUILayout.Button ("加密文件")) {
			EncryptAllConfig();
		}
		if (GUILayout.Button ("解密文件")) {
			DecryptAllConfig();
		}
		GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal();
        serverUrl = EditorGUILayout.TextField("登录服务器地址", serverUrl);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("服务器地址改为:" + serverUrl))
        {
            ChangeServerUrl();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("将选择的音频文件打包"))
        {
            PackSoundToPerfab();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("字间距:");
        temp = GUILayout.TextField(spacing.x.ToString());
        spacing.x = Convert.ToSingle(temp);
        GUILayout.Label("×");
        temp = GUILayout.TextField(spacing.y.ToString());
        spacing.y = Convert.ToSingle(temp);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("设置选中以及所有子项 UILabel 字间距"))
        {
            SetSpacing();
        } 
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("描边厚度:");
        temp = GUILayout.TextField(distance.x.ToString());
        distance.x = Convert.ToSingle(temp);
        GUILayout.Label("×");
        temp = GUILayout.TextField(distance.y.ToString());
        distance.y = Convert.ToSingle(temp);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("设置选中以及所有子项 UILabel 描边"))
        {
            SetOutline();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        atlas = (UIAtlas)UnityEditor.EditorGUILayout.ObjectField("图集", atlas, typeof(UIAtlas), false);
        if (GUILayout.Button("设置选中的物体的所有子物体 UIAtlas"))
        {
            foreach (UISprite sprite in Selection.activeGameObject.GetComponentsInChildren<UISprite>(true))
            {
                sprite.atlas = this.atlas;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("设置选中文件夹下所有 ui atlas 的图集格式为 Sprite"))
        {
            SetAtlasTextureType();
        }
        GUILayout.EndHorizontal();
	}

	static void EncryptAllConfig()
	{
		
		foreach (var t in Selection.GetFiltered(typeof(TextAsset), SelectionMode.DeepAssets)) {
			if (!(t is TextAsset)) {
				continue;
			}
			TextAsset text = (TextAsset)t;
			var p = Application.dataPath + AssetDatabase.GetAssetPath (text).Substring (("Assets").Length);
			Debug.Log (p);
			File.WriteAllText (p, Utility.Serialize (text.text));
		}
		AssetDatabase.Refresh ();
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
	}

	static void DecryptAllConfig()
	{
		foreach (var t in Selection.GetFiltered(typeof(TextAsset), SelectionMode.DeepAssets)) {
			if (!(t is TextAsset)) {
				continue;
			}
			TextAsset text = (TextAsset)t;
			var p = Application.dataPath + AssetDatabase.GetAssetPath (text).Substring (("Assets").Length);
			Debug.Log (p);
			File.WriteAllText (p, Utility.Parse (text.text));
		}
		AssetDatabase.Refresh ();
	}


    public string serverUrl;

    void ChangeServerUrl()
    {
        TextAsset config = Resources.Load<TextAsset>("config");
        //JsonObject obj = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(Utility.Parse(config.text));
        //obj["host"] = "http://" + serverUrl + "/";
        //File.WriteAllText(Application.dataPath + AssetDatabase.GetAssetPath(config).Substring("Assets".Length), Utility.Serialize(obj.ToString()));
        //Debug.Log("服务器url修改为:" + obj["host"]);
        //AssetDatabase.Refresh();
        //AssetDatabase.SaveAssets();
    }

    static void PackSoundToPerfab()
    {
        foreach (var b in Selection.GetFiltered(typeof(AudioClip), SelectionMode.Assets))
        {
            AudioClip clip = (AudioClip)b;

            GameObject go = new GameObject();
            go.AddComponent<AudioSource>().clip = clip;
            go.name = clip.name;

            string path = "Assets/QinCloud/Resources/Sounds/" + clip.name + ".prefab";
            var prefab = PrefabUtility.CreateEmptyPrefab(path);
            PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);
            DestroyImmediate(go);

        }
    }


    void SetSpacing()
    {
        foreach (var label in Selection.activeGameObject.GetComponentsInChildren<UILabel>(true))
        {
            label.spacingX = (int)spacing.x;
            label.spacingY = (int)spacing.y;
        }
    }

    void SetOutline()
    {
        foreach (var label in Selection.activeGameObject.GetComponentsInChildren<UILabel>(true))
        {
            label.effectStyle = UILabel.Effect.Outline;
            label.effectDistance = distance;
        }
    }

    void SetSpriteButtons()
    {

        foreach (var ob in Selection.gameObjects)
        {
            foreach (UIButton button in ((GameObject)ob).transform.GetComponentsInChildren<UIButton>(true))
            {
                Debug.Log(button.name);
                button.hover = Color.white;
            }
        }
        Debug.Log("批处理完成");
        AssetDatabase.Refresh();
    }

    public static void SetAtlasTextureType()
    {
        Debug.Log("设置选中文...");
        foreach (UnityEngine.Object atl in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            if (atl.GetType() != typeof(GameObject))
            {
                continue;
            }

            var go = (GameObject)atl;
            UIAtlas a = go.GetComponent<UIAtlas>();
            if (a == null)
            {
                continue;
            }

            Debug.Log("UnityEngine.Object=" + atl);

            var importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(a.spriteMaterial.mainTexture));
            importer.textureType = TextureImporterType.Sprite;
        }
    }

    [MenuItem("QC/清掉缓存")]
    static void ClearGamePer()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("清除完毕");
    }

    [MenuItem("QC/将选中图片转换为PNG")]
    static void ChageToPng()
    {
        foreach (var t in Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets))
        {
            if (!(t is Texture2D))
            {
                continue;
            }

            var source = t as Texture2D;
            RenderTexture texture = new RenderTexture(source.width, source.height, 24);
            texture.Create();
            Graphics.Blit(source, texture);
            int width = texture.width;
            int height = texture.height;
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
            RenderTexture.active = texture;
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture2D.Apply();

            var rawPath = AssetDatabase.GetAssetPath(source);
            var dir = Path.GetDirectoryName(rawPath).Substring("Assets/".Length);
            dir = Path.Combine(Application.dataPath, dir);
            var name = Path.GetFileNameWithoutExtension(rawPath) + ".png";
            var path = Path.Combine(dir, name);

            var bytes = texture2D.EncodeToPNG();
            Debug.Log("save to:" + path);
            using (var file = File.Open(path, System.IO.FileMode.Create))
            {
                file.Write(bytes, 0, bytes.Length);
            }
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("QC/将文件转换成utf-8 BOM")]
    static void UTF_8_BOM()
    {
        Encoding encodingUTF8 = new UTF8Encoding(true);
        byte[] preamble = encodingUTF8.GetPreamble();

        foreach (var s in Selection.GetFiltered(typeof(TextAsset), SelectionMode.DeepAssets))
        {
            TextAsset text = (TextAsset)s;
            string path = Path.Combine(Application.dataPath, AssetDatabase.GetAssetPath(text).Substring(7));
            Debug.Log("reading " + path);
            byte[] bytes = File.ReadAllBytes(path);

            if (bytes[0] != preamble[0] || bytes[1] != preamble[1] || bytes[2] != preamble[2])
            {
                byte[] newBytes = new byte[bytes.LongLength + 3];
                Array.Copy(preamble, 0, newBytes, 0, preamble.Length);
                Array.Copy(bytes, 0, newBytes, 3, bytes.LongLength);
                File.WriteAllBytes(path, newBytes);

                Debug.Log("Convert done." + newBytes.Length);
            }
            else
            {
                Debug.Log("File already with BOM.");
            }
        }
    }
}
