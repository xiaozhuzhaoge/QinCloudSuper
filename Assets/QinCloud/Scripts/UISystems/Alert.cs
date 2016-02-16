using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Alert : MonoBehaviour
{
	public UILabel label;
	public UILabel okLabel;
	public UILabel cancelLabel;
	public UILabel title;
	Action OK_Callback = null;
	Action Cancel_Callback = null;

    public Alert ShowAlertS(string content)
    {
        Alert alert = CloneSelf();
        alert.okLabel.text = "确定";
        alert.Show(content, null, null, true);
        return alert;
    }
    /// <summary>
    /// 调出确定取消小窗口
    /// </summary>
   
	
	public Alert ShowAlert (string okContent, string cancelContent, string titleContent, string content, Action callback=null, Action cancelCallback = null, bool clearOnSceneChanged = false)
	{
		Alert alert = CloneSelf ();

		alert.okLabel.text = okContent;
		if (alert.cancelLabel != null) {
			alert.cancelLabel.text = cancelContent;
		}
		alert.title.text = titleContent;
        
		alert.Show (content, callback, cancelCallback, clearOnSceneChanged);

		return alert;
	}

	static List<Alert> alerts = new List<Alert> ();

	Alert CloneSelf ()
	{
		GameObject go = (GameObject)Instantiate (gameObject);
		go.transform.parent = transform.parent;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
		Alert alert = go.GetComponent<Alert> ();
		alerts.Add (alert);
		return alert;
	}

	void Show (string content, Action callback=null, Action cancelCallback =null, bool clearOnSceneChanged = false, float timeout = -1, bool timeoutCancel = true)
	{
		OK_Callback = callback;
		Cancel_Callback = cancelCallback;
		gameObject.SetActive (true);
		label.text = content;
		if (clearOnSceneChanged) {
			SceneManager.instance.clearOnChangeScene.Add (gameObject);
		}

		if (timeout > 0) {
			StartCoroutine (Timeout (timeout, timeoutCancel));
		}
	}

	IEnumerator Timeout (float timeout, bool timeoutCancel)
	{
		yield return new WaitForSeconds (timeout);
		if (timeoutCancel) {
			OnAlertCancel ();
		} else {
			OnAlertOK ();
		}
	}

	public void OnAlertOK ()
	{
		Destroy (gameObject);

		if (OK_Callback != null) {
			OK_Callback ();
		}
	}

	void OnDestroy ()
	{
		alerts.Remove (this);
	}

	public void OnAlertCancel ()
	{
		Destroy (gameObject);

		if (Cancel_Callback != null) {
			Cancel_Callback ();
		}
	}

	public static void ClearAll ()
	{
		foreach (var a in alerts) {
			Destroy (a.gameObject);
		}
	}

    public void Destory()
    {
        GameObject.Destroy(this.gameObject);
    }
}
