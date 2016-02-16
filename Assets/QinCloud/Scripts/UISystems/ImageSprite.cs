using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageSprite : MonoBehaviour {

    public UITexture target;
//    public ImageBundle bundle;
    public Texture[] textures;

    public void SetImage(string name)
    {
        for(int i=0;i<textures.Length;i++){
            if(textures[i] != null && name == textures[i].name){
                target.mainTexture = textures[i];
                break; 
            }
        }
    }
}
