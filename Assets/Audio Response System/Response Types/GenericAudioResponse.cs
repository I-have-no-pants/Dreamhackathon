using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Parent class. Does not affect objects on its own.
/// </summary>
public class GenericAudioResponse : MonoBehaviour {

	/// <summary>
	/// The audio source the object listens to.
	/// </summary>
	public AudioSource audioSource;

	/// <summary>
	/// How many neighboring samples in either direction are used in getting the response value.
	/// </summary>
	public int range = 0;

	/// <summary>
	/// How much the frequency affects this object.
	/// </summary>
	public float multiplier = 20;

	/// <summary>
	/// How fast the rawValue can change. Higher -> faster.
	/// </summary>
	public float damping = 20;

	/// <summary>
	/// If the frequency index at this position goes above this number, call trigger.
	/// </summary>
	public float triggerVolume = 30;
	
	/// <summary>
	/// The central frequency index the object listens for.
	/// </summary>
	public int frequencyIndex = 50;

	/// <summary>
	/// Is true if object was triggered this frame.
	/// </summary>
	[HideInInspector]
	public bool isTriggered = false;

	/// <summary>
	/// The current response value. Use GetResponseValue() instead of this variable.
	/// </summary>
	protected float currentResponseValue = 0;

	/// <summary>
	/// Index of the data extraction type. 0 -> max, 1 -> min, 2 -> average.
	/// </summary>
	[HideInInspector]
	public int rangeType = 0;

	void Start () {
		// Error checks
		if(audioSource == null)
		{
			if(AudioResponseManager.instance.defaultAudioSource == null)
			{
				Debug.LogError("Default Audio Source not set.");
				return;
			}
			audioSource = AudioResponseManager.instance.defaultAudioSource;
		}

		RegisterSource();

		// The frequency must be in the range of (0) ~ (frequency resolution - 1)
		if(frequencyIndex < 0 || frequencyIndex >= AudioResponseManager.instance.frequencyResolution)
		{
			Debug.LogError("Frequency for " + this.GetType() + " out of bounds on " + gameObject.name + ". \n" +
				"Please change the frequency to be less than the frequency resolution.");
		}
	}

	/// <summary>
	/// Returns the raw amplitude of the target frequency range.
	/// </summary>
	public float GetResponseValue()
	{
		float rawValue = AudioResponseSystem.GetData(audioSource, frequencyIndex, range, AudioResponseSystem.rangeType[rangeType]);
		currentResponseValue = Mathf.Lerp(currentResponseValue, 
		                          rawValue * multiplier, 
		                          Time.deltaTime * damping);

		if(triggerVolume < rawValue * 6000) 
		{ 
			isTriggered = true;
			OnTriggered();
		} else {
			isTriggered = false;
		}

		return currentResponseValue;
	}

	/// <summary>
	/// Returns the full array of the spectrum data.
	/// </summary>
	public float[] GetSpecData()
	{
		return AudioResponseManager.instance.GetSpectrumData(audioSource);
	}

	/// <summary>
	/// Registers the audio source in the manager.
	/// </summary>
	public void RegisterSource()
	{
		AudioResponseManager.instance.RegisterSource(audioSource);
	}

	/// <summary>
	/// Gets called when trigger is activated. Override this in child classes.
	/// </summary>
	public virtual void OnTriggered()
	{
		
	}
}
