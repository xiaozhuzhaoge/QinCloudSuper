using UnityEngine;
using System.Collections;

public class RenderQueueWidget : MonoBehaviour {
    public UIWidget target;
    public int offset = 1;

    int renderQueue = -100;

	public void SetDirty(){
		renderQueue = -100;
	}

    void Update()
    {
        if (target == null) {
            return;
        }

        if (target.drawCall != null && renderQueue != target.drawCall.finalRenderQueue) {        
            renderQueue = target.drawCall.finalRenderQueue;

            foreach (var ren in GetComponentsInChildren<Renderer>(true)) {
                ren.material.renderQueue = renderQueue + offset;
            }
        }
    }

    void OnDestroy()
    {
        foreach (var ren in GetComponentsInChildren<Renderer>(true))
        {
            DestroyImmediate(ren.material);
        }
    }
}
