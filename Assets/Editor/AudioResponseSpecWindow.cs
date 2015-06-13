using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class AudioResponseSpecWindow : EditorWindow {

	public GenericAudioResponse targetObject;

	// TODO: eq size multiplier
	// Trigger height
	// Enums for types of value
	// Pause button

	Vector2 eqSize = new Vector2(1024, 250);
	int eqWidth = 1024;
	int eqHeight = 250;
	Vector2 eqOffset = new Vector2(20, 170);

	Color32[] rawTextureData;

	float[] data;
	float[,] oldData;
	float[] averageData;
	float[] maxData;
	float[] waveForm;
	int oldDataStartIndex = 0;

	int averageFrameNumber = 30;

	float frequencyScale = 10;

	bool showAverage = true;
	bool showCurrent = true;
	bool showMax = true;
	bool showTrigger = true;
	bool showWaveform = false;
	bool onlyRange = false;

	bool wasTriggered = false;

	string[] options = new string[]{ "Max volume", "Min volume", "Average volume" };

	int triggerYPos = 0;

	Texture2D eqTexture;

	Color32 red = new Color32(255, 0, 0, 255);
	Color32 cyan = new Color32(0, 255, 255, 255);
	Color32 green = new Color32(0, 255, 0, 255);
	Color32 grey = new Color32(128, 128, 128, 255);
	Color32 blue = new Color32(0, 0, 255, 255);
	Color32 magenta = new Color32(255, 0, 255, 255);
	Color32 white = new Color32(255, 255, 255 ,255);
	Color32 yellow = new Color32(255, 255, 0, 255);
	Color32 darkGrey = new Color32(64, 64, 64, 255);
	
	
	/// <summary>
	/// Updates frequency resolution and prevents errors in case of a code reload.
	/// </summary>
	public void Initialize()
	{
		rawTextureData = new Color32[eqWidth * eqHeight];
		eqTexture = new Texture2D(eqWidth, eqHeight, TextureFormat.RGBA32, false);
		int res = 1024;
		if(AudioResponseManager.instance.frequencyResolution < 1024)
			res = AudioResponseManager.instance.frequencyResolution;
		data = new float[res];
		averageData = new float[res];
		oldData = new float[res,averageFrameNumber];
		maxData = new float[res];
		waveForm = new float[res];
	
		minSize = eqOffset + eqSize + Vector2.one * 20;
		maxSize = minSize;

		triggerYPos = (int)(eqOffset.y + eqHeight - Mathf.Clamp (targetObject.triggerVolume * frequencyScale / 10, 0, eqHeight));

	}

	void OnGUI () {

		// Draws the labels and buttons
		DrawGUI();
		// Fills the frequency + range grey band
		DrawBackground();
		// Fills the blue average intensity
		DrawAverageEq();
		// Fills the red realtime intensity
		DrawRealtimeEq();
		// Fills the white dots of the last max value
		DrawLastMax();
		// Draws the waveform
		DrawWaveform();
		// Fills the grey trigger line
		DrawTrigger();
		// Draws the texture in the window
		DrawEq ();
		// Ensures an update each frame
		Repaint ();
	}

	void DrawGUI()
	{
		if(!targetObject)
		{
			Close();
			return;
		}

		if(targetObject.audioSource == null) targetObject.audioSource = AudioResponseManager.instance.defaultAudioSource;

		EditorGUILayout.SelectableLabel("Target: " + targetObject.name, EditorStyles.boldLabel);
		
		EditorGUILayout.BeginHorizontal();
		int newRange = (int)( EditorGUILayout.Slider("Range", 
		                                             targetObject.range, 
		                                             0,
		                                             Mathf.Min (targetObject.frequencyIndex, Mathf.Abs (targetObject.frequencyIndex - AudioResponseManager.instance.frequencyResolution)), 
		                                             GUILayout.MaxWidth(500)));
		if(!(newRange + targetObject.frequencyIndex > AudioResponseManager.instance.frequencyResolution || targetObject.frequencyIndex - newRange < 1))
		{
			targetObject.range = newRange;
		}
		
		EditorGUILayout.Space ();
		
		frequencyScale = (float)Mathf.Pow ( EditorGUILayout.Slider("Spectrum Scale", (float)Mathf.Sqrt(frequencyScale), 1, 25, GUILayout.MaxWidth(500)), 2);
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		
		int newFrequency = (int)( EditorGUILayout.Slider("Frequency", targetObject.frequencyIndex, 0, AudioResponseManager.instance.frequencyResolution - 1, GUILayout.MaxWidth(500)));
		if(!(targetObject.range + newFrequency > AudioResponseManager.instance.frequencyResolution - 1 || newFrequency - targetObject.range < 0))
		{
			targetObject.frequencyIndex = newFrequency;
		} else {
			targetObject.frequencyIndex = newFrequency;
			targetObject.range = Mathf.Min (Mathf.Abs (targetObject.frequencyIndex - AudioResponseManager.instance.frequencyResolution) - 1, targetObject.frequencyIndex);
		}
		
		EditorGUILayout.Space();
		
		targetObject.rangeType = EditorGUILayout.Popup("Data Extraction Type", targetObject.rangeType, options,EditorStyles.popup, GUILayout.MaxWidth(500));
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		
		targetObject.triggerVolume = EditorGUILayout.Slider ("Trigger Volume", targetObject.triggerVolume, 0, 500, GUILayout.MaxWidth(500));
		
		GUILayout.Space(52);
		
		if(targetObject.range > 0)
		{
			EditorGUILayout.SelectableLabel(
				"Hertz range: " + 
				(targetObject.frequencyIndex - targetObject.range) * targetObject.audioSource.clip.frequency / AudioResponseManager.instance.frequencyResolution
				+ " ~ " +
				(targetObject.frequencyIndex + targetObject.range) * targetObject.audioSource.clip.frequency / AudioResponseManager.instance.frequencyResolution);
		} else {
			EditorGUILayout.SelectableLabel(
				"Hertz range: " + 
				targetObject.frequencyIndex * targetObject.audioSource.clip.frequency / AudioResponseManager.instance.frequencyResolution);
		}
		
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.BeginHorizontal();
		showCurrent = EditorGUILayout.Toggle("Show realtime", showCurrent, GUILayout.Width (200));
		
		showMax = EditorGUILayout.Toggle("Show max", showMax, GUILayout.Width (200));
		
		showWaveform = EditorGUILayout.Toggle("Show waveform", showWaveform, GUILayout.Width (200));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		showAverage = EditorGUILayout.Toggle("Show average", showAverage, GUILayout.Width (200));
		
		showTrigger = EditorGUILayout.Toggle("Show trigger", showTrigger, GUILayout.Width (200));

		GUI.enabled = false;
		onlyRange = EditorGUILayout.Toggle("View range only", onlyRange, GUILayout.Width (200));
		GUI.enabled = true;

		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		averageFrameNumber = EditorGUILayout.IntField("Frames stored", averageFrameNumber, GUILayout.Width(200));
		
		if(GUILayout.Button("Update", GUILayout.Width(60)) && averageFrameNumber > 0)
		{
			int res = 1024;
			if(AudioResponseManager.instance.frequencyResolution < 1024)
				res = AudioResponseManager.instance.frequencyResolution;
			oldData = new float[res, averageFrameNumber];
			oldDataStartIndex = 0;
		}
		EditorGUILayout.EndHorizontal();
	}

	void DrawBackground()
	{

		if(data == null || data.Length == 0)
			Initialize();

		int increments = (int)(1024 / data.Length);

		for(int i = 0; i < data.Length - 1; i++)
		{
			// Draws pixels up to the line
			for(int a = 0; a < eqHeight; a++)
			{
				if(i >= targetObject.frequencyIndex - targetObject.range && i <= targetObject.frequencyIndex + targetObject.range)
				{
					// Draws thicker lines when the resolution is less than 1024.
					for(int w = 0; w < increments; w++)
					{
						rawTextureData[(eqWidth * a) + i + w] = darkGrey;
					}
				}
			}
		}
	}

	void DrawWaveform()
	{
		if(showWaveform)
		{
			if(targetObject.audioSource == null) targetObject.audioSource = AudioResponseManager.instance.defaultAudioSource;
			targetObject.audioSource.GetOutputData(waveForm, 0);

			for(int i = 0; i < waveForm.Length - 1; i++)
			{
				int index = i + (eqWidth * (int)Mathf.Clamp (((waveForm[i] * (eqSize.y / 5)) + eqHeight/2), 0, eqHeight - 1));
				rawTextureData[index] = new Color32((byte)(rawTextureData[index].r / 4 + 192),(byte)( rawTextureData[index].g / 2 + 192),(byte)( rawTextureData[index].b / 2 + 192), 255);
			}
		}
	}
	
	void DrawTrigger()
	{
		triggerYPos = (int)(Mathf.Clamp (targetObject.triggerVolume / 10 * frequencyScale, 0, eqHeight));

		// Trigger line
		if(showTrigger && triggerYPos != eqHeight)
		{

			if(targetObject.isTriggered || wasTriggered)
			{
				for(int i = 0; i < eqWidth; i++)
				{
					rawTextureData[i + (eqWidth * triggerYPos)] = cyan;
				}
				wasTriggered = false;
			} else {
				for(int i = 0; i < eqWidth; i++)
				{
					rawTextureData[i + (eqWidth * triggerYPos)] = grey;
				}
			}
		}
	}

	void DrawLastMax()
	{
		if(showMax)
		{
			int increments = (int)(1024 / data.Length);

			for(int i = 0; i < oldData.GetLength(0); i++)
			{
				float max = 0;
				for(int a = 0; a < oldData.GetLength(1); a++)
				{
					if(oldData[i, a] > max) max = oldData[i, a];
				}
				maxData[i] = max;
			}

			for(int i = 0; i < maxData.Length - 1; i++)
			{
				for(int w = 0; w < increments; w++)
				{
					if(i >= targetObject.frequencyIndex - targetObject.range && i <= targetObject.frequencyIndex + targetObject.range)
					{
						rawTextureData[i + w + (eqWidth * (int)Mathf.Clamp (maxData[i] * 600 * frequencyScale, 0, eqHeight - 1))] = yellow;
					} else {
						rawTextureData[i + w + (eqWidth * (int)Mathf.Clamp (maxData[i] * 600 * frequencyScale, 0, eqHeight - 1))] = white;
					}
				}
			}
		}
	}

	void DrawAverageEq()
	{
		if(oldData == null)
			Initialize();

		if(!EditorApplication.isPaused)
		{
			// Set average eq size
			for(int i = 0; i < oldData.GetLength(0); i++)
			{
				for(int a = 0; a < oldData.GetLength(1); a++)
				{
					averageData[i] += oldData[i, a];
				}
				averageData[i] /= oldData.GetLength(1);
			}
		}

		if(showAverage)
		{
			int increments = (int)(1024 / data.Length);

			for(int i = 0; i < data.Length - 1; i++)
			{
				// Draws pixels up to the line
				for(int a = 0; a < (int)Mathf.Clamp (averageData[i] * 600 * frequencyScale, 0, eqHeight); a++)
				{
					if(i >= targetObject.frequencyIndex - targetObject.range && i <= targetObject.frequencyIndex + targetObject.range)
					{
						// Draws thicker lines when the resolution is less than 1024.
						for(int w = 0; w < increments; w++)
						{
							rawTextureData[(eqWidth * a) + i + w] = magenta;
						}
					} else {
						for(int w = 0; w < increments; w++)
						{
							rawTextureData[(eqWidth * a) + i + w] = blue;
						}
					}
				}
			}
		}
	}

	void DrawRealtimeEq()
	{
		if(oldData == null)
			Initialize();
		if(!EditorApplication.isPaused)
			data = targetObject.GetSpecData();

		int increments = (int)(1024 / data.Length);

		if(showCurrent)
		{
			for(int i = 0; i < data.Length - 1; i++)
			{
				// Draws pixels up to the line
				for(int a = 0; a < (int)Mathf.Clamp (data[i] * 600 * frequencyScale, 0, eqHeight); a++)
				{
					if(i >= targetObject.frequencyIndex - targetObject.range && i <= targetObject.frequencyIndex + targetObject.range)
					{
						// Draws thicker lines when the resolution is less than 1024.
						for(int w = 0; w < increments; w++)
						{
							rawTextureData[(eqWidth * a) + i + w] = green;
						}
						if(!Application.isPlaying && !wasTriggered && a >= triggerYPos)
							wasTriggered = true;

					} else {
						for(int w = 0; w < increments; w++)
						{
							rawTextureData[(eqWidth * a) + i + w] = red;
						}
					}
				}
			}
		}

		

		if(!EditorApplication.isPaused)
		{
			// Add to olddata
			for(int i = 0; i < data.Length; i++)
			{
				oldData[i, oldDataStartIndex] = data[i];
			}
			oldDataStartIndex++;
			if(oldDataStartIndex > oldData.GetLength(1) - 1)
			{
				oldDataStartIndex = 0;
			}
		}
	}

	void DrawEq()
	{
		if(eqTexture)
		{
			eqTexture.SetPixels32(rawTextureData);
			eqTexture.Apply ();
			rawTextureData = new Color32[eqWidth * eqHeight];

			EditorGUI.DrawPreviewTexture(new Rect(eqOffset.x, eqOffset.y, eqWidth, eqHeight), eqTexture);
		}
	}

}








