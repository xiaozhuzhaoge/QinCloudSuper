using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class ConfigInfo : MonoBehaviour {

    Dictionary<int, SoundConfig> _sounds = new Dictionary<int, SoundConfig>();

    static void SoundConfig(List<SoundConfig> list)
    {
        foreach (var item in list)
        {
            ConfigInfo.instance._sounds.Add(item.id, item);
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
