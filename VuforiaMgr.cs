using UnityEngine;


[AddComponentMenu("")]
public class VuforiaMgr : MonoBehaviour {

	public VuforiaHandler vuMark = null;

	private void Start() {
		
		GameObject obj = GameObject.Instantiate(vuMark.gameObject) as GameObject;
		obj.transform.SetParent(transform, false);
		#if ! UNITY_EDITOR
//		GameObject obj = GameObject.Instantiate(vuMark.gameObject) as GameObject;
//		obj.transform.SetParent(transform, false);
		#endif
	}
}
