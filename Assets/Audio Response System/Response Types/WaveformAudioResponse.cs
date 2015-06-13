using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(LineRenderer))]
public class WaveformAudioResponse : MonoBehaviour {

	public Vector2 size = new Vector2(100, 10);
	public AudioSource audioSource;
	public Gradient gradient;

	public bool around = false;
	public float degreesRotation = 360;
	public float damping = 30;

	float[] data;
	Vector3[] oldPos;
	LineRenderer lineRenderer;

	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(AudioResponseManager.instance.frequencyResolution);
		oldPos = new Vector3[AudioResponseManager.instance.frequencyResolution];
	}

	void Update () {
		data = AudioResponseManager.instance.GetOutputData(audioSource);
		if(around)
		{
			for(int i = 0; i < AudioResponseManager.instance.frequencyResolution; i++)
			{
				Vector3 position = new Vector3(size.x, data[i] * size.y, 0);
				position = Quaternion.Euler (0, ((float)i / AudioResponseManager.instance.frequencyResolution) * (degreesRotation + 1), 0) * position;
				position = Vector3.Lerp (oldPos[i], position, damping * Time.deltaTime);
				lineRenderer.SetPosition(i, Vector3.Scale(transform.rotation * position + transform.position, transform.localScale));
				oldPos[i] = position;
			}
		} else {
			for(int i = 0; i < AudioResponseManager.instance.frequencyResolution; i++)
			{
				Vector3 position = new Vector3(size.x * ((float)i / AudioResponseManager.instance.frequencyResolution) - size.x/2, data[i] * size.y, 0);
				position = Vector3.Lerp (oldPos[i], position, damping * Time.deltaTime);
				lineRenderer.SetPosition(i, Vector3.Scale(transform.rotation * position + transform.position, transform.localScale));
				oldPos[i] = position;
			}
		}
		lineRenderer.SetColors (gradient.Evaluate(0),gradient.Evaluate(1));

	}
}
