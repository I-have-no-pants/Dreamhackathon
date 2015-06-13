using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	private GameManagerScript manager;

	private Text text;
	
	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<GameManagerScript> ();
		text = FindObjectOfType<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "" + manager.Protects;
	}
}
