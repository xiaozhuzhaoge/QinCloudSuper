using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageBundle : ScriptableObject {
    
    public Dictionary<string, Texture> images = new Dictionary<string, Texture> ();
    #if UNITY_EDITOR
    public List<string> textsNames = new List<string>();
    [ContextMenu("生成图片集")]
    void GenImageBundle ()
    {
        images.Clear ();
        textsNames.Clear ();
        foreach(var obj in UnityEditor.Selection.GetFiltered(typeof(Texture),UnityEditor.SelectionMode.DeepAssets)){
            Texture tex = (Texture)obj;
            images.Add (tex.name, tex);
            textsNames.Add (tex.name);
        }
    }
    #endif
    public Texture Get(string name){
        Texture output;
        images.TryGetValue (name, out output);
        return output;
    }
}
