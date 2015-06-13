using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class AudioResponseManager : MonoBehaviour {


	private Dictionary<AudioSource, float[]> sourceSpectrumData = new Dictionary<AudioSource, float[]>();
	private Dictionary<AudioSource, float[]> sourceOutputData = new Dictionary<AudioSource, float[]>();
	public AudioSource defaultAudioSource;

	[HideInInspector]
	public string[] frequencyChoices = new [] { "64", "128", "256", "512", "1024", "2048", "4096", "8192" };
	[HideInInspector]
	public int[] frequencyChoiceNumbers = new [] { 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
	[HideInInspector]
	public int frequencyChoiceIndex = 4;

	[HideInInspector]
	public int _frequencyResolution = 1024; // Needs to be public or it will reset on play
	public int frequencyResolution{
		get{
			return _frequencyResolution;
		}
		set{
			if(_frequencyResolution != value)
			{
				AudioResponseSystem.UpdateFrequencyValues(_frequencyResolution, value);
				_frequencyResolution = value;
				
				int index = 0;
				for(int i = 0; i < AudioResponseManager.instance.frequencyChoices.Length; i++) 
				{
					if(AudioResponseManager.instance.frequencyChoiceNumbers[i] == frequencyResolution) 
					{
						index = i; // Get index of resolution
						break;
					}
				}
				frequencyChoiceIndex = index;
			}
		}
	}

	void Update()
	{
		// Cycle through all the registered audio sources in Update. 
		// This way each source will only be processed once per frame.
		foreach(KeyValuePair<AudioSource, float[]> pair in sourceSpectrumData)
		{
			pair.Key.GetSpectrumData(pair.Value, 0, FFTWindow.BlackmanHarris);
		}
		foreach(KeyValuePair<AudioSource, float[]> pair in sourceOutputData)
		{
			pair.Key.GetOutputData(pair.Value, 0);
		}
	}

	public float[] GetOutputData(AudioSource source)
	{
		if(source == null)
		{
			if(defaultAudioSource == null) 
			{
				Debug.LogError ("Default audio source not set");
				return new float[1];
			}
			source = defaultAudioSource;
		}
		if(sourceOutputData.ContainsKey(source))
			return sourceOutputData[source];
		else
		{
			float[] data = new float[AudioResponseManager.instance.frequencyResolution];
			source.GetOutputData(data, 0);
			return data;
		}
	}

	public float[] GetSpectrumData(AudioSource source)
	{
		if(source == null)
		{
			if(defaultAudioSource == null) 
			{
				Debug.LogError ("Default audio source not set");
				return new float[1];
			}
			source = defaultAudioSource;
		}
		if(sourceSpectrumData.ContainsKey(source))
			return sourceSpectrumData[source];
		else
		{
			float[] data = new float[AudioResponseManager.instance.frequencyResolution];
			source.GetSpectrumData(data, 0, FFTWindow.Blackman);
			return data;
		}
	}

	public void RegisterSource(AudioSource audioSource)
	{
		if(!sourceSpectrumData.ContainsKey(audioSource))
		{
			sourceSpectrumData.Add (audioSource, new float[AudioResponseManager.instance.frequencyResolution]);
			sourceOutputData.Add (audioSource, new float[AudioResponseManager.instance.frequencyResolution]);
		}
	}

	public void ResetSources()
	{
		sourceSpectrumData.Clear();
		sourceOutputData.Clear ();
	}
		

	// Singleton
	private static AudioResponseManager _instance;
	public static AudioResponseManager instance
	{
		get{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<AudioResponseManager>();
				if(_instance == null)
				{
					Debug.LogError("Audio Response Manager does not exist.");
					return null;
				}
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	void Awake () {
		if(_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		} else {
			if(this != instance) Destroy (this.gameObject);
		}
	}
}
