using UnityEngine;
using System.Collections;
using System;

public class ImageSaver : MonoBehaviour
{
	public string image;
	public int w;
	public int h;
	WWW www;
		
	void Start ()
	{
		//StartCoroutine (SaveTexture ("bar", "bar"));
	}
		
	public void SaveTexture (string imageName, string saveAs)
	{
		Texture2D tex = GetComponent<Renderer>().material.mainTexture as Texture2D;
		byte[] byteArray;
		//WWW www = new WWW ("file:" + Application.dataPath + "/Resources/" + imageName + ".png");
		//yield return www;
			
		//if (www.texture != null) {
		//	print ("success");
		//}
		//tex = www.texture;
		byteArray = tex.EncodeToPNG ();
			
		string temp = Convert.ToBase64String (byteArray);
		Debug.Log (temp);

		image = temp;
		w = tex.width;
		h = tex.height;
		/*
		PlayerPrefs.SetString (saveAs, temp);      /// save it to file if u want.
		PlayerPrefs.SetInt (saveAs + "_w", tex.width);
		PlayerPrefs.SetInt (saveAs + "_h", tex.height);*/
	}
		
	public Texture2D RetriveTexture (string savedImageName)
	{
		string temp = image; //PlayerPrefs.GetString (savedImageName);
			
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

		//test = RetriveTexture ("bar");
	}
}
