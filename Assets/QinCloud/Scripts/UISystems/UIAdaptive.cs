using UnityEngine;
using System.Collections;

public class UIAdaptive
{
    public static float originWidth = 1280f;
    public static float originHeight = 720f;
    static Vector2 nowWH = new Vector2();

    public static float lastWidth ;
    public static float lastHeight ;

    public static Vector2 GetKeepRatioWH()
    {
        if (Screen.width == lastWidth && Screen.height == lastHeight)
        {
            return nowWH;
        }

        float originRatio = originWidth / originHeight;

        int width = (int)Screen.width;
        int height = (int)(Screen.width / originRatio);

        if (height > Screen.height)
        {
            width = (int)(Screen.height * originRatio);
            height = Screen.height;
        }  

        while (width > Screen.width)
        {
            int overFlow = (int)(width - Screen.width);
            height -= (int)overFlow;
            width = (int)(height / originRatio);
        }
        nowWH = new Vector2(width, height);
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        return nowWH;
    }

    public static float GetKeepRatioScale()
    {
        Vector2 wh = GetKeepRatioWH();
        float yScale = wh.y / originHeight;
        float xScale = wh.x / originWidth;
        return yScale > xScale ? xScale : yScale;
    }


}
