
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(GenericAudioResponse), true)]
[CanEditMultipleObjects]
public class GenericAudioResponseEditor : Editor
{

	GenericAudioResponse targetObject;

	void Awake() {
		Initialize();
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();																// Draw the default inspector

		// Hertz field. Converts the frequency index to Hz and vice versa.
		if(targetObject.audioSource != null)
		{
			targetObject.frequencyIndex = Mathf.RoundToInt(
				EditorGUILayout.FloatField("Hertz",
			                           targetObject.frequencyIndex * targetObject.audioSource.clip.frequency / AudioResponseManager.instance.frequencyResolution) 
				/ targetObject.audioSource.clip.frequency * AudioResponseManager.instance.frequencyResolution);
		} else {
			targetObject.frequencyIndex = Mathf.RoundToInt(
				EditorGUILayout.FloatField("Hertz",
			                           targetObject.frequencyIndex * AudioResponseManager.instance.defaultAudioSource.clip.frequency / AudioResponseManager.instance.frequencyResolution) 
				/ AudioResponseManager.instance.defaultAudioSource.clip.frequency * AudioResponseManager.instance.frequencyResolution);
		}

		// Frequency resolution field

		int currentResolution = AudioResponseManager.instance.frequencyResolution;
		int index = 0;
		for(int i = 0; i < AudioResponseManager.instance.frequencyChoices.Length; i++) 
		{
			if(AudioResponseManager.instance.frequencyChoiceNumbers[i] == currentResolution) 
			{
				index = i; // Get index of resolution
				break;
			}
		}



		if(!Application.isPlaying)
			AudioResponseManager.instance.frequencyChoiceIndex = EditorGUILayout.Popup("Frequency resolution", index, AudioResponseManager.instance.frequencyChoices); 		// Select the index of the clicked button

		// Open editor window button
		AudioResponseManager.instance.frequencyResolution = AudioResponseManager.instance.frequencyChoiceNumbers[AudioResponseManager.instance.frequencyChoiceIndex];				// Update the selected index in the underlying object
		if(GUILayout.Button("Open Editor Window")) 												// Open editor window
		{
			AudioResponseSpecWindow window = (AudioResponseSpecWindow)EditorWindow.GetWindow (typeof (AudioResponseSpecWindow));
			window.targetObject = targetObject;
			window.Show();
		}

		

		if(GUILayout.Button("Sync Response Objects")) 		
		{
			foreach(GenericAudioResponse g in targetObject.gameObject.GetComponents<GenericAudioResponse>())
			{
				if(g != targetObject)
				{
					Undo.RecordObject(g, "Synchronized Response Objects");
					g.audioSource = targetObject.audioSource;
					g.range = targetObject.range;
					g.multiplier = targetObject.multiplier;
					g.damping = targetObject.damping;
					g.triggerVolume = targetObject.triggerVolume;
					g.frequencyIndex = targetObject.frequencyIndex;
					EditorUtility.SetDirty(g);
				}
			}
		}
		
		if(targetObject.frequencyIndex < 0) targetObject.frequencyIndex = 0;
		if(targetObject.range < 0) targetObject.range = 0;
		if(targetObject.damping < 0) targetObject.damping = 0;
		if(targetObject.triggerVolume < 0) targetObject.triggerVolume = 0;
		
		EditorUtility.SetDirty(target);			// Save the changes back to the object
	}

	void Initialize()
	{
		targetObject = target as GenericAudioResponse;
	}
}

[CustomEditor(typeof(ARSSequenceTool), true)]
public class ARSSequenceToolEditor : Editor
{
	
	ARSSequenceTool targetObject;
	List<GameObject> childObjects = new List<GameObject>();

	bool changeColor = false;
	bool changeIndex = false;
	bool changeRange = false;
	bool changeMultiplier = false;
	
	
	void Awake() {
		Initialize();
	}
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();																// Draw the default inspector

		changeColor = GUILayout.Toggle (changeColor, "Change color");
		changeIndex = GUILayout.Toggle (changeIndex, "Change index");
		changeRange = GUILayout.Toggle (changeRange, "Change range");
		changeMultiplier = GUILayout.Toggle (changeMultiplier, "Change multiplier");

		if(GUILayout.Button("Apply changes") && (changeColor || changeRange || changeMultiplier || changeIndex)) 		
		{
			List<Object> undoList = new List<Object>();
			foreach(GameObject g in childObjects)
			{
				undoList.Add ( g.GetComponent<GenericAudioResponse>() );
			}
			

			Undo.RecordObjects(undoList.ToArray(), "Using ARS Sequence Tool");

			if(changeColor)
			{
				List<ColorAudioResponse> list = new List<ColorAudioResponse>();
				for(int i = 0; i < childObjects.Count; i++)
				{
					if(childObjects[i].GetComponent<ColorAudioResponse>())
					{
						list.Add (childObjects[i].GetComponent<ColorAudioResponse>());
					}
				}
				for(int i = 0; i < list.Count; i++)
				{
					GradientColorKey[] colorKey = new GradientColorKey[] { 
						new GradientColorKey(targetObject.gradientRangeStart.Evaluate( (float)i / list.Count), 0),
						new GradientColorKey(targetObject.gradientRangeEnd.Evaluate( (float)i / list.Count), 1)
						
					};
					GradientAlphaKey[] alphaKey = new GradientAlphaKey[] { 
						
						new GradientAlphaKey(targetObject.gradientRangeStart.Evaluate( (float)i / list.Count).a, 0), 
						new GradientAlphaKey(targetObject.gradientRangeEnd.Evaluate( (float)i / list.Count).a, 1)

					};
					list[i].gradient.SetKeys( colorKey, alphaKey );
				}

			}

			if(changeIndex)
			{
				for(int i = 0; i < childObjects.Count; i++)
				{
					
					foreach(GenericAudioResponse g in childObjects[i].GetComponents<GenericAudioResponse>())
					{
						g.frequencyIndex = (int)((targetObject.indexRangeEnd - targetObject.indexRangeStart) * 
						                ((float)i / (float)childObjects.Count)) + targetObject.indexRangeStart;
					}
				}

			}

			if(changeRange)
			{
				for(int i = 0; i < childObjects.Count; i++)
				{

					foreach(GenericAudioResponse g in childObjects[i].GetComponents<GenericAudioResponse>())
					{
						g.range = (int)((targetObject.rangeRangeEnd - targetObject.rangeRangeStart) * 
						                ((float)i / (float)childObjects.Count)) + targetObject.rangeRangeStart;
					}
				}
				
			}

			if(changeMultiplier)
			{
				for(int i = 0; i < childObjects.Count; i++)
				{
					
					foreach(GenericAudioResponse g in childObjects[i].GetComponents<GenericAudioResponse>())
					{
						g.multiplier = (int)((targetObject.multiplierRangeEnd - targetObject.multiplierRangeStart) * 
						                     ((float)i / (float)childObjects.Count)) + targetObject.multiplierRangeStart;
					}
				}
			}
		}

		EditorUtility.SetDirty(target);			// Save the changes back to the object
	}
	
	void Initialize()
	{
		targetObject = target as ARSSequenceTool;
		foreach(Transform g in targetObject.GetComponentInChildren<Transform>())
		{
			childObjects.Add (g.gameObject);
		}
	}
}


#endif