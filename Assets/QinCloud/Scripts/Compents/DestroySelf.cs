using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour
{
    public Mode mode;
    public float delay = 0;
   
    public enum Mode
    {
        normal,
        audioClipLength
    }

    // Use this for initialization
    void Start()
    {
		Destroy ();
    }
    
	void Destroy()
	{
		if (mode == Mode.audioClipLength)
		{
			Destroy(gameObject, audio.clip.length );
		} else
		{
			Destroy(gameObject, delay);
		}
	}
    // Update is called once per frame
    void Update()
    {
    
    }
}
