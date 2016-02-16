using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{   
	public static Dictionary<int,List<SoundPlayer>> playing = new Dictionary<int, List<SoundPlayer>> ();
	AudioSource source;
	float interval;
	float volumeScale;
	SoundConfig config;

	public void Set (SoundConfig config)
	{   
		source = GetComponent<AudioSource> ();
		this.config = config;

		if (config.ifCircle) {
			interval = config.interval;
		
			source.loop = interval <= 0;
			StartCoroutine ("Circle");
			
		}

		volumeScale = config.volume;
		SetVolume ();
		if (config.limit > 0) {
			if (!playing.ContainsKey (config.id)) {
				playing.Add (config.id, new List<SoundPlayer> ());
			}

			playing [config.id].Add (this);
		}
	}

	void OnDestroy ()
	{
		if (config != null && config.limit > 0) {
			if (playing.ContainsKey (config.id)) {
				if (playing [config.id].Contains (this)) {
					playing [config.id].Remove (this);
				}
			}
		}
	}

	FloatAction GetSysVolume;

	public void RegisterSysVolume (FloatAction action)
	{
		GetSysVolume = action;
	}

	IEnumerator Circle ()
	{
        AudioSource audio = this.GetComponent<AudioSource>();
		while (true) {
			yield return new WaitForSeconds (audio.clip.length + this.interval);
			audio.Play ();
		}
	}

	void Update ()
	{
		SetVolume ();
	}

	void SetVolume ()
	{
		source.volume = volumeScale * GetSysVolume ();        
	}
}