using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Light))]
public class LightAudioResponse : GenericAudioResponse {


	public float colorMultiplier = 1;
	public float sizeMultiplier = 1;
	public float intensityMultiplier = 1;
	
	public Gradient gradient;

	private new Light light;
	private float wat;
	private float initialIntensity;

	// Use this for initialization
	void Awake () {

		light = GetComponent<Light>();
		wat = light.range;
		initialIntensity = light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		GetResponseValue();
		light.color = gradient.Evaluate(currentResponseValue * colorMultiplier);
		light.range = wat * currentResponseValue * sizeMultiplier;
		light.intensity = initialIntensity * currentResponseValue * intensityMultiplier;
	}
}
