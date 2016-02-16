using UnityEngine;
using System.Collections;

/// <summary>
/// 可扩展场景类型
/// </summary>
public enum UILevel
{
    MAIN = 1
}

public class LevelInfo {

    public int level;
    public UILevel uilevel;
    public int MusicId;

    public LevelInfo() { }
    public LevelInfo(int levelc,UILevel uiLevelc)
    {
        level = levelc;
        uilevel = uiLevelc;
    }

}
