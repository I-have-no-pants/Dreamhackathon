using UnityEngine;
using System.Collections;

/// <summary>
/// Changes size in the way a speaker cone would
/// move back and forth to create air vibrations.
/// </summary>
public class SpeakerAudioResponse : GenericAudioResponse {

	float[] outputData = new float[16];
	Vector3 defaultPos;
	
	void Awake () {
		defaultPos = transform.localPosition;
		// We want it to react as fast as possible as it outputs vibrations.
		damping = 60;
	}
	
	void Update () {
		audioSource.GetOutputData(outputData, 0);

		transform.localPosition = defaultPos + new Vector3(GetResponseValue () * outputData[0], 0, 0);
	}
}
