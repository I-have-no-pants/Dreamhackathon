using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ImageSaver : MonoBehaviour
{
	public string imageType;
	public string saveFile;
	public int w = 320;
	public int h = 240;
		
	public void SaveTexture (string imageName, string saveAs)
	{
		Texture2D tex = GetComponent<Renderer>().material.mainTexture as Texture2D;
		byte[] byteArray;

		byteArray = tex.EncodeToPNG ();
			
		string temp = Convert.ToBase64String (byteArray);
		Debug.Log (temp);

		//File.ReadAllText
		File.WriteAllText(saveFile+imageType, temp);

		w = tex.width;
		h = tex.height;
	}
		
	public Texture2D RetriveTexture (string savedImageName)
	{
		string temp = File.ReadAllText (saveFile+imageType);
			
		int width = w; //PlayerPrefs.GetInt (savedImageName + "_w");
		int height = h; //PlayerPrefs.GetInt (savedImageName + "_h");
			
		byte[] byteArray = Convert.FromBase64String (temp);
			
		Texture2D tex = new Texture2D (width, height);
			
		tex.LoadImage (byteArray);
		Debug.Log (tex);
		return tex;
			
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.F11)){
			SaveTexture ("bar", "bar");
		}

		if(Input.GetKeyDown(KeyCode.F10)){
			GetComponent<Renderer>().material.mainTexture = RetriveTexture ("");
			Debug.Log ("Load");
		}
	}
}
