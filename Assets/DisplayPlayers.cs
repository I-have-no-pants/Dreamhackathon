using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class DisplayPlayers : MonoBehaviour {
	
	public DepthWrapper dw;

	public bool update = true;
	
	
	private Texture2D tex;
	// Use this for initialization
	void Start () {
		tex = new Texture2D(320,240,TextureFormat.ARGB32,false);
		GetComponent<Renderer> ().material.mainTexture = tex;
	}
	
	// Update is called once per frame
	void Update () {
		if (update && dw.pollDepth())
		{
			//tex.SetPixels32(convertDepthToColor(dw.depthImg));
			tex.SetPixels32(convertPlayersToCutout(dw.segmentations));
			tex.Apply(false);
		}
	}
	private Color32[] convertPlayersToCutout(bool[,] players)
	{
		Color32[] img = new Color32[320*240];
		for (int pix = 0; pix < 320*240; pix++)
		{
			if(players[0,pix])
			{
				img[pix].r = (byte)255;
				img[pix].a = (byte)255;
			}
			else if (players[1,pix]) {
				img[pix].a = (byte)255;
				img[pix].b = (byte)255;
			}else if (players[2,pix]) {
				img[pix].a = (byte)255;
				img[pix].g = (byte)255;
			}else if (players[3,pix]) {
				img[pix].b = (byte)255;
				img[pix].a = (byte)255;
				img[pix].r = (byte)255;
			}else if (players[4,pix]) {
				img[pix].b = (byte)255;
				img[pix].a = (byte)255;
				img[pix].g = (byte)255;
			}else if (players[5,pix]) {
				img[pix].b = (byte)255;
				img[pix].g = (byte)255;
				img[pix].a = (byte)255;
				img[pix].r = (byte)255;
				
			} else {
				img[pix].a = (byte)0;
				img[pix].r = (byte)0;
			}
		}
		return img;
	}
}
