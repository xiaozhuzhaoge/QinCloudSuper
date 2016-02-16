using UnityEngine;
using System.Collections;

public class ScaleAdaptive : MonoBehaviour
{
    public enum FloatType
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public FloatType horizontalFloat = FloatType.NONE;
    public FloatType verticalFloat = FloatType.NONE;

	int preFrame = -1;
    Vector2 lastVector;
    Vector3 startVector;
 
	public void Update()
	{
        Adapter();
	}

    void Adapter()
    {
        Vector2 kWH = UIAdaptive.GetKeepRatioWH();
        if (lastVector == kWH)
            return;

        lastVector = kWH;

        transform.localScale = startVector * UIAdaptive.GetKeepRatioScale();
        //        print("localScale==" + transform.localScale + "===kWH====" + kWH);
        Vector3 offset = Vector3.zero;
        if (horizontalFloat != FloatType.NONE)
        {
            int dir = horizontalFloat == FloatType.LEFT ? -1 : 1;
            offset += dir * Vector3.right * (GUIManager.instance.mPanel.width - kWH.x) / 2;
            //            print(gameObject.name + "=floating=" + offset);

        }

        if (verticalFloat != FloatType.NONE)
        {
            int dir = verticalFloat == FloatType.DOWN ? -1 : 1;

            offset += dir * Vector3.up * (GUIManager.instance.mPanel.height - kWH.y) / 2;
            //            print(gameObject.name + "=floating=" + offset);
            ;
        }
        transform.localPosition = offset;

    }
    public void Start()
    {
        startVector = this.transform.localScale;
        Adapter();
    }

}
