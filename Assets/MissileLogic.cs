using UnityEngine;
using System.Collections;

public class MissileLogic : MonoBehaviour {
	public Vector3 velocity;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += velocity * Time.deltaTime;
	}
}
