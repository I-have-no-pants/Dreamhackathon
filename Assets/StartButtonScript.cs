﻿using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour {
	
	private CollisionManager cm;
	private GameManagerScript manager;

	public float ActivateTimer;

	public GameObject dot;
	public float startSize;
	public float endSize;

	public float timer;
	
	// Use this for initialization
	void Start () {
		cm = FindObjectOfType<CollisionManager>();
		manager = FindObjectOfType<GameManagerScript> ();
		timer = ActivateTimer;
	}

	void Update () {
		Vector2 pixel = cm.getTexturePosition(transform.position);
		if (pixel.x != 0 || pixel.y != 0) {
			if (cm.getCollision(pixel)) {
				timer -= Time.deltaTime;
				if (timer<=0) {
					manager.StartLoadingLevel();
					Destroy(gameObject);
				}
			} else if (timer<ActivateTimer) {
				timer += Time.deltaTime;
			}
		}
		float s = (ActivateTimer-timer)/ActivateTimer;
		float scale = (1-s) * startSize + (s) * endSize;
		dot.transform.localScale = new Vector3 (scale, scale, scale);
	}
}
