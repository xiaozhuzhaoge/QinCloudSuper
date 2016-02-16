
using UnityEngine;


public class UIPlayASound : MonoBehaviour	
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		Custom,
		OnEnable,
		OnDisable,
		OnStart
	}
	
	public int soundId;
	public Trigger trigger = Trigger.OnClick;
	
	bool mIsOver = false;
	
	bool canPlay {
		get {
			if (!enabled)
				return false;
			UIButton btn = GetComponent<UIButton> ();
			return (btn == null || btn.isEnabled);
		}
	}

	void Start(){
		if (trigger == Trigger.OnStart)
            Spawn();
	}
	
	void OnEnable ()
	{
		if (trigger == Trigger.OnEnable)
            Spawn();
	}
	
	void OnDisable ()
	{
		if (trigger == Trigger.OnDisable)
            Spawn();
	}
	
	void OnHover (bool isOver)
	{
		if (trigger == Trigger.OnMouseOver) {
			if (mIsOver == isOver)
				return;
			mIsOver = isOver;
		}
		
		if (canPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
            Spawn();
	}
	
	void OnPress (bool isPressed)
	{
		if (trigger == Trigger.OnPress) {
			if (mIsOver == isPressed)
				return;
			mIsOver = isPressed;
		}
		
		if (canPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
            Spawn();
	}
	
	void OnClick ()
	{
		if (canPlay && trigger == Trigger.OnClick)
            Spawn();
	}
	
	void OnSelect (bool isSelected)
	{
		if (canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
			OnHover (isSelected);
	}
	
	public void Play ()
	{
        Spawn();
	}

    void Spawn() 
    {
		SoundManager.Spawn(SceneManager.instance.cacheRoot,soundId,null,Vector3.zero);
    }
}
