using UnityEngine;
using System.Collections;

public class SizeAudioResponse : GenericAudioResponse {

	public Vector3 scaleAxes = new Vector3(1, 1, 1);
	// The initial size. Used to scale the resizing appropriately.
	Vector3 defaultScale;

	void Awake () {
		defaultScale = transform.localScale;
		audioSource = GameObject.Find ("Music").GetComponent<AudioSource> ();
	}

	void Update () {
		transform.localScale = defaultScale + (Vector3.Scale (defaultScale, scaleAxes) * GetResponseValue ());
	}
}
