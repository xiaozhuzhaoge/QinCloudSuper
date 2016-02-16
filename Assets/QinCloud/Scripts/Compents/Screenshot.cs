using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour {

    public GameObject screenImage;
    public static Screenshot instance;
    public Camera camera;
    void Awake()
    {
        Screenshot.instance = this;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    [ContextMenu("Test")]
    public void ScreenCameraTrue()
    {
        screenImage.GetComponent<UITexture>().mainTexture = CaptureCamera(camera, new Rect(Screen.width * 0f, Screen.height * 0f, Screen.width * 1f, Screen.height * 1f));
        screenImage.SetActive(true);
    }

    /// <summary>
    /// 对相机截图。 
    /// </summary>
    /// <returns>The screenshot2.</returns>
    /// <param name="camera">Camera.要被截屏的相机</param>
    /// <param name="rect">Rect.截屏的区域</param>
    Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        // 创建一个RenderTexture对象
        Debug.Log(rect.width + " " + rect.height);
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
        camera.targetTexture = rt;
        camera.Render();
        //--- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
        //camera2.targetTexture = rt;
        //camera2.Render();
        //-------------------------------------------------------------------
        // 激活这个rt, 并从中中读取像素。
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        GameObject.Destroy(rt);
        CacheFactory.SaveToPicture(screenShot, "/ScreenShot.png",CacheFactory.PictureType.JPG);
        return screenShot;
    }
}
