using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class SoundManager
{
	public static void GetSound (string soundName, Action<GameObject> callback=null)
	{
		Utility.instance.LoadGameObject (soundName, callback);
	}

	public static void Spawn (int id)
	{
		if (id == 0) {
			return;
		}
		Spawn (SceneManager.instance.cacheRoot, id, Vector3.zero);
	}
	
	public static void Spawn (Transform target, int id, Vector3 pos)
	{
        if (target == null)
			return;
        Spawn(target, id, null, pos);
	}

    public static void Spawn(Transform target, int id, Transform parent, Vector3 pos, Action<GameObject> callback = null)
	{

		if (id == 0) {
			return;
		}

		var config = ConfigInfo.instance.sounds [id];

		if (config.limit > 0) {
			if (SoundPlayer.playing.ContainsKey (config.id)) {
				if (SoundPlayer.playing [config.id].Count >= config.limit) {
					return;
				}
			}
		}

		SoundManager.GetSound (config.resId, (go) => {

			if (parent == null) {
				go.transform.parent = SceneManager.instance.cacheRoot;
			} else {
				go.transform.parent = parent;
			}
			go.transform.position = pos;
			
			AudioSource source = go.GetComponent<AudioSource> ();
			SoundPlayer player = go.GetComponent<SoundPlayer> ();
			if (config.type == 6) {//music
				source.priority = 0;			
				player.RegisterSysVolume (GetMusicVolume);
			} else {
				source.priority = 256 - config.priority; 		
				player.RegisterSysVolume (GetEffectVolume);
			}

			player.Set (config);
            
			if (config.time > 0) {
				go.AddComponent<DestroySelf> ().delay = config.time;
			} else if (!config.ifCircle) {
				go.AddComponent<DestroySelf> ().delay = source.clip.length;           
			} else {
                SceneManager.instance.clearOnChangeScene.Add(go);
			}	

			if (callback != null) {
				callback (go);
			}
		});
	}

	static float GetEffectVolume ()
	{
        Debug.Log ("GetEffectVolume=" + GameManagerSys.SEValue);
        
		return GameManagerSys.SEValue;
	}

	static float GetMusicVolume ()
	{
		return GameManagerSys.BGMValue;
	}

	public static List<int> loaded = new List<int> ();

	public static void PreLoad (int id)
	{
		if (loaded.Contains (id)) {
			return;
		}
		loaded.Add (id);
		Utility.instance.LoadGameObject (ConfigInfo.instance.sounds [id].resId, (go) => {
			go.transform.parent = SceneManager.instance.cacheRoot;
			go.SetActive (false);
		});
	}
}
