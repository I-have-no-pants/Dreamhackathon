using UnityEngine;
using System.Collections;

public class PostToTwitterScript : MonoBehaviour {
	public string url = "humanshieldflow.mybluemix.net/highscore";
	public string postString;
	// Use this for initialization
	void Start () {
		StartCoroutine (Post ());
	}

	IEnumerator Post() {
		/*
		if (!Input.GetKeyDown (KeyCode.T)) {
			yield break;
		}*/
		Debug.Log ("Sending tweet!");
		// Start a download of the given URL
		//string s = "{\"body\": " + postString + "}";

		WWWForm form = new WWWForm ();
		//form.AddField ("\"body\"", "\"hello world form\"");
		form.AddField ("msg", "hello unity");
		//byte[] byteString = System.Text.Encoding.ASCII.GetBytes (s);
		//WWW www = new WWW (url, byteString);
		WWW www = new WWW (url, form);
		
		// Wait for download to complete
		yield return www;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
