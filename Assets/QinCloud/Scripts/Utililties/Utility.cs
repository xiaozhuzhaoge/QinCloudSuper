using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SimpleJson;
using System.Text;
using System.Security.Cryptography;


public delegate bool BoolAction<T> (T obj);

public delegate bool BoolAction ();

public delegate float FloatAction ();

public delegate T TAction<T> ();

public delegate IEnumerator CoroutineAction ();

public delegate IEnumerator CoroutineAction_Params (params object[] args);

public delegate IEnumerator CoroutineAction<T> (T obj);

public delegate IEnumerator CoroutineAction<T1,T2> (T1 obj1,T2 obj2);

public class Utility : MonoBehaviour
{
	public static Utility instance;

	void Awake ()
	{
		instance = this;
	}

	public static void EditorDebug (params object[] args)
	{
#if UNITY_EDITOR
		var output = "";
		foreach(var arg in args){
			output += "===";
			output += arg.ToString();
		}
		Debug.Log(output);
#endif
	}

    /// <summary>
    /// Sync Load perfabs
    /// </summary>
    /// <param name="path"></param>
    /// <param name="Callback"></param>
	public void LoadGameObject (string path, Action<UnityEngine.GameObject> Callback)
	{
		Load (path, (asset) => {
			if (asset == null) {
				throw new UnityException ("can not load game object:" + path);
			}
			GameObject go = (GameObject)Instantiate (asset);
			Callback (go);
		});
	}

    /// <summary>
    /// Async Load perfabs
    /// </summary>
    /// <param name="path"></param>
    /// <param name="Callback"></param>
    public void LoadGameObjectAsync(string path, Action<UnityEngine.GameObject> Callback)
    {
        if (SceneManager.couroutineObj == null)
        {
            Debug.Log(" 场景切换中，终止场景内的异步加载执行！");
            return;
        }
        CoroutineObj obj = SceneManager.couroutineObj.GetComponent<CoroutineObj>();
        obj.StartCoroutine(LoadAsync(path, (asset) =>
        {
            if (asset == null)
            {
                throw new UnityException("can not load game object:" + path);
            }
            GameObject go = (GameObject)Instantiate(asset);
            Callback(go);
        }));
    }

	IEnumerator LoadAsync(string name, Action<UnityEngine.Object> Callback) {
		ResourceRequest request = Resources.LoadAsync(name);
		yield return request;
		yield return new WaitForSeconds (0.03f);
		Callback(request.asset);
	}

	public void LoadObject<T> (string path, Action<T> Callback = null) where T : UnityEngine.Object
	{
		Load (path, (asset) => {
			if (asset == null) {
				throw new UnityException ("can not load object:" + path);
			}
			if (Callback != null) {
				Callback (asset as T);
			}
		});
	}

	void Load (string name, Action<UnityEngine.Object> Callback)
	{
		Callback (Resources.Load (name));            
	}

	public static void SetActiveChildren (Transform parent, bool active)
	{
		for (int i=0; i<parent.childCount; i++) {
			GameObject go = parent.GetChild (i).gameObject;            
			go.SetActive (active);
		}
	}
 
	public void SequenceInterval (float interval, params Action[] actions)
	{
		StartCoroutine (IntervalExecutes (interval, actions));
	}


	IEnumerator IntervalExecutes (float sec, Action[] actions)
	{
		int index=0;
		foreach (var action in actions) {
			yield return new WaitForSeconds (sec);
			if(action!=null)
			{
				action ();
			}
			index++;
		}
	}

    public void WaitForSecond(float sec, Action cb)
    {
        StartCoroutine(Wait(sec, cb));
    }
        
	public IEnumerator Wait (float sec, Action cb)
	{
		yield return new WaitForSeconds (sec);
		cb ();
    }

    public void WaitForFrame(int frameNum, Action cb)
    {
        StartCoroutine(Wait(frameNum, cb));
    }

	public IEnumerator Wait (int frameNum, Action cb)
	{
		do {
			yield return new WaitForEndOfFrame ();
			frameNum--;
		} while (frameNum>0);
		cb ();
	}

	public static string FormatTime (int sec, int length=3)
	{
		string hour = "00";
		string mins = "00";
		string secs = "00";
		TimeString (sec / 3600, ref hour);
		TimeString (sec % 3600 / 60, ref mins);
		TimeString (sec % 60, ref secs);

		if (length == 2)
			return String.Format ("{0}:{1}", mins, secs);
		if (length == 1)
			return String.Format ("{0}", secs);
		return String.Format ("{0}:{1}:{2}", hour, mins, secs);
	}

	static void TimeString (int time, ref string output)
	{
		output = time.ToString ();
		if (output.Length == 1) {
			output = "0" + output;
		}
	}

	public static long ClampLong (long value, long min, long max)
	{
		value = Math.Min (value, max);
		value = Math.Max (value, 0);
		return value;
	}

	public static void DestroyAllChildren (GameObject parent)
	{
		for (int i=0; i<parent.transform.childCount; i++) {
			Destroy (parent.transform.GetChild (i).gameObject);
		}
		parent.transform.DetachChildren ();
	}

	public static Vector3 GetUIPosByWorldPos (Camera uicamera , Vector3 worldPos)
	{
		Vector3 mainCameraPos = Camera.main.WorldToScreenPoint (worldPos);
		Vector3 pos = uicamera.ScreenToWorldPoint (mainCameraPos);
		return pos;        
	}


	public static void ClearArray<T> (T[] arr)
	{
		for (int i=0; i<arr.Length; i++) {
			arr [i] = default(T);
		}
	}

	public static Vector3 GetCenterFromPoints (IList<Vector3> vecs)
	{        
		if (vecs.Count == 0) {
			return Vector3.zero;
		}
		if (vecs.Count == 1) {
			return vecs [0];
		}
		Bounds bounds = new Bounds (vecs [0], Vector3.zero);
		for (var i = 1; i < vecs.Count; i++) {
			bounds.Encapsulate (vecs [i]); 
		}
		return bounds.center;
	}

	/// <summary>
	/// Spawns the mesh from bottom points.
	/// </summary>
	/// <returns>center of bottom</returns>
	/// <param name="posList">Position list.</param>
	public static void SpawnMeshFromBottomPoints (List<Vector3> posList, GameObject target)
	{
		Vector3 center = Utility.GetCenterFromPoints (posList);
		target.transform.localPosition = center;
		List<Vector3> vecs = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		for (int i = 0; i < posList.Count; i++) {
			Vector3 vec2_t, vec1_t, vec2, vec1 = posList [i] - center;
			if (i + 1 == posList.Count) {
				vec2 = posList [0] - center;
			} else {
				vec2 = posList [i + 1] - center;
			}
			vec1_t = vec1 + new Vector3 (0, 50, 0);
			vec2_t = vec2 + new Vector3 (0, 50, 0);
			vecs.Add (vec1);
			vecs.Add (vec1_t);
			vecs.Add (vec2);
			vecs.Add (vec2_t);
			triangles.Add (i * 4 + 0);
			triangles.Add (i * 4 + 1);
			triangles.Add (i * 4 + 2);
			triangles.Add (i * 4 + 1);
			triangles.Add (i * 4 + 3);
			triangles.Add (i * 4 + 2);
		}
		MeshFilter filter = target.AddComponent<MeshFilter> ();
		filter.mesh = new Mesh ();
		filter.mesh.vertices = vecs.ToArray ();
		filter.mesh.triangles = triangles.ToArray ();
		filter.mesh.RecalculateBounds ();
		filter.mesh.RecalculateNormals ();
		MeshCollider col = target.AddComponent<MeshCollider> ();
		col.convex = true;

	}

	/// <summary>
	/// 会随着场景切换而清理的coroutine
	/// </summary>
	/// <param name="co">Co.</param>
	public static CoroutineObj StartSceneCoroutine (CoroutineAction co)
	{
		GameObject go = new GameObject ();
		CoroutineObj obj = go.AddComponent<CoroutineObj> ();
		obj.StartCoroutine (co ());
		go.name = "_co";

		return obj;
	}

	public static CoroutineObj StartSceneCoroutine (CoroutineAction_Params co, params object[] args)
	{
		GameObject go = new GameObject ();
		CoroutineObj obj = go.AddComponent<CoroutineObj> ();
		obj.StartCoroutine (co (args));
		go.name = "_co";
		
		return obj;
	}

	public static CoroutineObj StartSceneCoroutine<T1> (CoroutineAction<T1> co, T1 t1)
	{
		GameObject go = new GameObject ();
		CoroutineObj obj = go.AddComponent<CoroutineObj> ();
		obj.StartCoroutine (co (t1));
		go.name = "_co";
		return obj;
	}


	/// <summary>
	/// 会随着场景切换而清理的coroutine
	/// </summary>
	/// <param name="co">Co.</param>
	public static CoroutineObj StartSceneCoroutine<T1,T2> (CoroutineAction<T1,T2> co, T1 t1, T2 t2)
	{
		GameObject go = new GameObject ();
		CoroutineObj obj = go.AddComponent<CoroutineObj> ();
		obj.StartCoroutine (co (t1, t2));
		go.name = "_co";
		return obj;
	}

	public static bool Roll (float rate)
	{
		return  UnityEngine.Random.value <= rate;
	}
  
	public static void ResetTransform (Transform tran)
	{
		tran.localPosition = Vector3.zero;
		tran.localRotation = Quaternion.identity;
		tran.localScale = Vector3.one;
	}

	Dictionary<string,GameObject> timeoutHandlers = new Dictionary<string, GameObject> ();

	/// <summary>
	/// 手动清理的coroutine ; destroy gameObject 以停止
	/// </summary>
	/// <param name="co">Co.</param>

	public static float GetTimeByFixedFrame (float frames)
	{
		return frames * Time.fixedDeltaTime;
	}


	public static void SortDictionary<T1,T2> (ref Dictionary<T1, T2> members, Comparison<KeyValuePair<T1,T2>> comparer)
	{
		List<KeyValuePair<T1, T2>> list = new List<KeyValuePair<T1, T2>> ();
		foreach (var kv in members) {
			list.Add (kv);
		}
		members.Clear ();
		
		list.Sort (comparer);

		foreach (var kv in list) {
			members.Add (kv.Key, kv.Value);
		}
	}


	public static Quaternion RotateToDir (Transform transform, Vector3 dir)
	{   		
		return Quaternion.LookRotation (Vector3.RotateTowards (transform.forward, dir, 2, 0));
	}
	
	public static Quaternion RotateToTarget_onlyY (Transform transform, Vector3 pos)
	{
		var dir = new Vector3 (pos.x, transform.position.y, pos.z) - transform.position;
		return RotateToDir (transform, dir);
	}


	public static void SetLayerWithChildren (GameObject go, int layer)
	{
		foreach (var t in go.GetComponentsInChildren<Transform>()) {
			t.gameObject.layer = layer;
		}		
	}


	public static void GetActiveChildren(List<GameObject> array,ref List<GameObject> activeArray)
	{
		activeArray.Clear ();
		foreach(GameObject item  in array)
		{
			if(item.activeSelf)
				activeArray.Add(item);
		}
	}

	static float GetDisToTrans(Vector3 curPos,Transform trans,bool world)
	{
		float distance=Vector3.Distance(curPos,trans.localPosition);
		if(world)
			distance=Vector3.Distance(curPos,trans.position);
		return distance;
	}



	public static int FindItemCount<T> (List<T> lists, T value)//
	{
		int count = 0;
		foreach (var item in lists) {
			if (value.Equals (item))
				count++;
		}
		return count;
	}

	public static List<int> GetListByString (string _content, string _symbol)
	{
		List<int> _list = new List<int> ();
		string[] _temp = _content.Split (_symbol.ToCharArray ());
		foreach (string _item in _temp) {
			if (_item == "")
				continue;
			_list.Add (Convert.ToInt32 (_item));
		}
		
		return _list;
	}
	
	public static bool GetKeyValueByIndex<T1,T2> (Dictionary<T1,T2> _dic, ref T2 _value, int _index)
	{
		int i = 1;
		foreach (var _item in _dic) {
			if (i == _index) {
				_value = _item.Value;
				return true;
			}
			i++;
		}
		return false;
	}

	public static void DicCopy<T1,T2> (Dictionary<T1,T2> _original, ref Dictionary<T1,T2> _target)
	{
		_target.Clear ();
		foreach (var _item in _original) {
			_target.Add (_item.Key, _item.Value);
		}
	}

	public static bool CheckUiCanInstance(MonoBehaviour ui,GameObject obj)
	{
		if(ui!=null)
		{
			Utility.DestroyObject(obj);
			return false;
		}
		return true;
	}

	public static void DicConn<T1, T2> (ref Dictionary<T1, T2> _original, Dictionary<T1, T2> _target)
	{
		foreach (var _item in _target) {
			_original.Add (_item.Key, _item.Value);
		}
	}

	public static void  ListCopy<T1> (List<T1> _original, ref List<T1> _target)
	{
		_target.Clear ();
		foreach (var _item in _original) {
			_target.Add (_item);
		}
	}

	public static long GetSecTimeByString (string _time, string _symbol)
	{
		long _timeSec = 0;
		if (_time == "0")
			return _timeSec;
		List<int> _list = Utility.GetListByString (_time, _symbol);
		
		_timeSec += _list [0] * 3600;
		_timeSec += _list [1] * 60;
		_timeSec += _list [2];
		return _timeSec;
	}

	public static string Serialize (string originalStr)
	{
		byte[] originalStrAsBytes = Encoding.UTF8.GetBytes (originalStr);
		byte[] keys = new byte[] {
			0xC8,
			0xAA,
			0xFD,
			0xC9,
			0xBB,
			0xFA,
			0xCA,
			0xCC,
			0xAF,
			0xBF,
			0xDD,
			0xC6,
			0xBC,
			0xBC
		};
		using (MemoryStream memStream = new MemoryStream(originalStrAsBytes.Length)) {
			for (int i=0; i < originalStrAsBytes.Length; i++) {
				byte x = originalStrAsBytes [i];
				x = (byte)(x ^ keys [i % keys.Length]);
				x = (byte)(~x);
				memStream.WriteByte (x);
			}

			originalStrAsBytes = memStream.ToArray ();
		}
		return  Convert.ToBase64String (originalStrAsBytes);
	}

	public static string Parse (string serializedStr)
	{
		byte[] serializedStrAsBytes = Convert.FromBase64String (serializedStr);
		byte[] keys = new byte[] {
			0xC8,
			0xAA,
			0xFD,
			0xC9,
			0xBB,
			0xFA,
			0xCA,
			0xCC,
			0xAF,
			0xBF,
			0xDD,
			0xC6,
			0xBC,
			0xBC
		};
		using (MemoryStream memStream = new MemoryStream(serializedStrAsBytes.Length)) {
			for (int i=0; i < serializedStrAsBytes.Length; i++) {
				byte x = serializedStrAsBytes [i];
				x = (byte)(~x);
				x = (byte)(x ^ keys [i % keys.Length]);
				memStream.WriteByte (x);
			}
			
			serializedStrAsBytes = memStream.ToArray ();
		}
		return  Encoding.UTF8.GetString (serializedStrAsBytes);
	}

	public static int GetCircleSectorIndex (int sectorCount, Vector3 originPos, Vector3 forward, Vector3 targetPos)
	{
		float sectorAngle = 360f / sectorCount;
		float halfSector = sectorAngle / 2;
		Vector3 origin = forward;
		origin.y = 0;
		
		Vector3 toTarget = targetPos - originPos;
		toTarget.y = 0;
		
		float angle = Vector3.Angle (origin, toTarget);
		if (angle < halfSector) {
			return 0;
		}
		
		var index = Mathf.RoundToInt (Mathf.CeilToInt (angle / halfSector) / 2f);
		
		Vector3 cross = Vector3.Cross (origin, toTarget);
		if (cross.y < 0) {
			index = sectorCount - index;
		}
		return index;
	}

	public static string CTString(object obj)
	{
		return Convert.ToString (obj);
	}

	public static float CTFloat(object obj)
	{
		return Convert.ToSingle (obj);
	}

	public static int CTInt(object obj)
	{
		return Convert.ToInt32 (obj);
	}

	public static double CTDouble(object obj)
	{
		return Convert.ToDouble(obj);
	}

	public static long CTLong(object obj){
		return Convert.ToInt64 (obj);
	}

}
