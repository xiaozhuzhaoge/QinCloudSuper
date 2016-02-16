using UnityEngine;
using System.Collections;

public class DelayActive : MonoBehaviour
{
	public GameObject target;
	public float delay;

	IEnumerator Start ()
	{
		if (delay == 0) {
			target.SetActive (true);
			yield return null;
		} else {
			yield return new WaitForSeconds (delay);
			target.SetActive (true);
		}
	}

}
