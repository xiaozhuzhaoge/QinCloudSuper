using UnityEngine;
using System.Collections;

public class UICell : MonoBehaviour {

    public BoxCollider mCollider;
    public UIEventListener listener;
    public BoxCollider cacheCollider;
    public UIInput input;
    public UILabel content;
    public bool CanEdit;


    CellIndex index;
    public CellIndex Index
    {
        set { index = value; }
        get { return index; }
    }


    void Awake()
    {
        content = transform.FindChild("content").GetComponent<UILabel>();
        Add2DCollider();
    }


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Add2DCollider()
    {
        mCollider = gameObject.AddComponent<BoxCollider>();
        gameObject.GetComponent<UISprite>().ResizeCollider();
        listener = gameObject.AddComponent<UIEventListener>();

        if (!CanEdit)
            return;

        listener.onHover = delegate
        {
            if(input == null)
            {
                input = content.gameObject.AddComponent<UIInput>();
                input.label = content;
            }
           
            if(cacheCollider == null)
            {
                cacheCollider = content.gameObject.AddComponent<BoxCollider>();
                input.GetComponent<UIWidget>().ResizeCollider();
                input.value = content.text;
            }

        };
    }

    
}
