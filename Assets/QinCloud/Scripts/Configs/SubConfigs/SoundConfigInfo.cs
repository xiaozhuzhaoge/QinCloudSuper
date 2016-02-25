using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public partial class ConfigInfo : MonoBehaviour {

    Dictionary<int, SoundConfig> _sounds = new Dictionary<int, SoundConfig>();

    public static void SoundConfig(List<SoundConfig> list)
    {
        for(int i = 0 ; i < list.Count ; i++){
            ConfigInfo.instance._sounds.Add(list[i].id, list[i]);
        }
    }

    public Dictionary<int, SoundConfig> sounds
    {
        get
        {
            PreLoadConfig("sound", _sounds.Count == 0);
            return _sounds;
        }
    }
}
