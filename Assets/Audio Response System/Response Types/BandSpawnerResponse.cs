using UnityEngine;
using System.Collections;

public class BandSpawnerResponse : MonoBehaviour {

	public AudioSource audioSource;

	public Material cubeMaterial;

	public Gradient colorRange;
	
	public int indexRangeStart;
	public int indexRangeEnd;
	
	public float multiplierRangeStart;
	public float multiplierRangeEnd;

	public float triggerVolume;
	
	public int rangeRangeStart;
	public int rangeRangeEnd;

	public int numberOfLines;
	public float lineWidth;
	public float lineLifetime = 5;

	public float lineSpeed = 60;

	public bool circular = false;
	public float degreesRotation = 360;

	GameObject[] linePositions;
	bool[] inProgress;

	void Start()
	{
		if(audioSource == null) audioSource = AudioResponseManager.instance.defaultAudioSource;
		linePositions = new GameObject[numberOfLines];
		inProgress = new bool[numberOfLines];
		if(circular)
		{
			for(int i = 0; i < numberOfLines; i++)
			{
				Vector3 position = new Vector3(lineWidth/2, 0, 0);
				position = Quaternion.Euler (0, ((float)i / numberOfLines) * (degreesRotation), 0) * position;
				linePositions[i] = new GameObject("Line " + i);
				linePositions[i].transform.parent = gameObject.transform;
				linePositions[i].transform.localRotation = Quaternion.identity;
				linePositions[i].transform.localPosition = position;
				inProgress[i] = false;
			}
		} else {
			for(int i = 0; i < numberOfLines; i++)
			{
				linePositions[i] = new GameObject("Line " + i);
				linePositions[i].transform.parent = gameObject.transform;
				linePositions[i].transform.localRotation = Quaternion.identity;
				linePositions[i].transform.localPosition = new Vector3((numberOfLines / 2 - i), 0, (Mathf.Pow ((numberOfLines / 2 - i), 2)) * 0.025f) * lineWidth / numberOfLines;
				inProgress[i] = false;
			}
		}
	}

	void FixedUpdate () {
		float indexIncrements = ((indexRangeEnd - indexRangeStart) / numberOfLines);
		float multiplierIncrements = ((multiplierRangeEnd - multiplierRangeStart) / numberOfLines);
		float rangeIncrements = ((rangeRangeStart - rangeRangeEnd) / numberOfLines);
		for(int i = 0; i < linePositions.Length; i++)
		{
			if(!inProgress[i])
			{
				int index = indexRangeStart + (int)(indexIncrements * i);
				int range = rangeRangeStart + (int)(rangeIncrements * i);
				float raw = AudioResponseSystem.GetData(audioSource, 
				                                        index,
				                                        range, 
				                                        AudioResponseSystem.RangeType.MAX);
				float multiplier = multiplierRangeStart + multiplierIncrements * i;
				if(raw * multiplier > triggerVolume)
				{
					StartCoroutine(StartLine (i, index, range, multiplier, lineLifetime, colorRange.Evaluate((float)i / numberOfLines)));
					inProgress[i] = true;
				}
			}
		}
	}
	
	IEnumerator StartLine(int lineNumber, int index, int range, float multiplier, float time, Color color)
	{
		float timer = 0;
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.parent = linePositions[lineNumber].transform;
		
		cube.transform.localPosition = Vector3.zero;
		cube.transform.localRotation = Quaternion.identity;
		Renderer cubeRenderer = cube.GetComponent<Renderer>();
		cubeRenderer.material = cubeMaterial;
		cubeRenderer.material.color = color;

		bool expand = true;
		while(true)
		{
			timer += Time.deltaTime;
			float raw = AudioResponseSystem.GetData(audioSource, 
			                                        index,
			                                        range, 
			                                        AudioResponseSystem.RangeType.MAX);
			if(expand)
			{
				if(raw * multiplier <= triggerVolume)
				{
					inProgress[lineNumber] = false;
					expand = false;
				}
				cube.transform.localScale = new Vector3(1, cube.transform.localScale.y + (Time.deltaTime * lineSpeed), 1);
				cube.transform.localPosition = new Vector3(0, cube.transform.localScale.y/2, 0);
			} else {
				cube.transform.localPosition = new Vector3(0, cube.transform.localPosition.y + (Time.deltaTime * lineSpeed), 0);
			}
			cubeRenderer.material.color = color * Mathf.Clamp ((raw * multiplier) / triggerVolume, 0.5f, 1);

			if(timer > time)
			{
				Destroy (cube);
				yield break;
			}
			yield return null;

		}
	}
}
