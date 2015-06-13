using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[System.Serializable]
public static class AudioResponseSystem {

	public enum RangeType{ MAX = 0x00, MIN = 0x01, AVERAGE = 0x02};
	public static RangeType[] rangeType = new RangeType[] { RangeType.MAX, RangeType.MIN, RangeType.AVERAGE };

	/// <summary>
	/// Updates the frequency and range of all response objects to match the new resolution.
	/// </summary>
	public static void UpdateFrequencyValues(int oldValue, int newValue)
	{
		GenericAudioResponse[] objects = Object.FindObjectsOfType<GenericAudioResponse>();
#if UNITY_EDITOR
		Undo.RecordObjects(objects, "Updating frequency values");
#endif

		foreach(GenericAudioResponse g in objects)
		{
			ResetAudioSources();
			g.frequencyIndex = (int)(g.frequencyIndex * ((float)newValue / (float)oldValue));
			g.range = (int)(g.range * ((float)newValue / (float)oldValue));
			g.RegisterSource();
#if UNITY_EDITOR
			EditorUtility.SetDirty(g);
#endif
		}

	}

	public static void ResetAudioSources()
	{
		AudioResponseManager.instance.ResetSources();
	}

	public static float GetData(AudioSource audioSource, int frequency, int range, RangeType rangeType)
	{
		float[] data = AudioResponseManager.instance.GetSpectrumData(audioSource);
		if(range == 0)
		{		
			return data[frequency];
		}

		// In case of errors
		if(frequency - range < 0 || frequency + range > data.Length - 1)
		{
			Debug.LogError ("Range exceeds frequency params");
			return 0;
		}

		float returnValue = 0;
		switch(rangeType)
		{
		case RangeType.MAX:
			for(int i = frequency - range; i < frequency + range; i++)
			{
				if(data[i] > returnValue) returnValue = data[i];
			}
			break;
		case RangeType.MIN:
			for(int i = frequency - range; i < frequency + range; i++)
			{
				if(data[i] < returnValue) returnValue = data[i];
			}
			break;
		case RangeType.AVERAGE:
			float total = 0;
			for(int i = frequency - range; i < frequency + range; i++)
			{
				total += data[i];
			}
			returnValue = total/(range*2);
			break;
		}

		return returnValue;
	}


}
