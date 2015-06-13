using UnityEngine;
using System.Collections;

public class ColorAudioResponse : GenericAudioResponse {

	public Gradient gradient;

	private MeshRenderer meshRenderer;

	void Awake () {
		meshRenderer = GetComponent<MeshRenderer>();
		//multiplier /= 5; // Color stuff is a lot more sensitive so we dampen it down here for consistency.
	}

	// Update is called once per frame
	void Update () {
		meshRenderer.material.color = gradient.Evaluate(GetResponseValue());
	}
}
