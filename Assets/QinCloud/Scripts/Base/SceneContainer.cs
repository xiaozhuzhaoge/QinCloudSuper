using UnityEngine;
using System.Collections;

public class SceneContainer : MonoBehaviour {

    public Transform temp_GhostPoint;
    void Start()
    {
        if (SceneManager.instance == null)
            return;
        transform.parent = SceneManager.instance.senceRoot;
        SceneManager.instance.curScene = this;
        SceneManager.instance.clearOnChangeScene.Add(gameObject);
    }

	
}
