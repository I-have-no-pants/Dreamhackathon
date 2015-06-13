using UnityEngine;
using System.Collections;

public class ImageFuser : MonoBehaviour {

	public Renderer depth;
	public Renderer color;
	public Renderer player;

	private Texture2D d_tex;
	private Texture2D c_tex;
	private Texture2D p_tex;

	private Texture2D fused_tex;

	public int sizeX = 320;
	public int sizeY = 240;
	
	private Renderer renderer;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();

		fused_tex = new Texture2D(sizeX,sizeY,TextureFormat.ARGB32,false);
		renderer.material.mainTexture = fused_tex;
	}

	private void setTextures() {
		d_tex = depth.GetComponent<Renderer> ().material.mainTexture as Texture2D;
		c_tex = color.GetComponent<Renderer> ().material.mainTexture as Texture2D;
		p_tex = player.GetComponent<Renderer> ().material.mainTexture as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
		setTextures ();
		Color32[] img = new Color32[sizeX*sizeY];
		Color32[] p_t = p_tex.GetPixels32 ();
		Color32[] c_t = c_tex.GetPixels32 ();
		Color32[] d_t = d_tex.GetPixels32 ();

		for (int pix = 0; pix < img.Length; pix++)
		{
			int x = pix %sizeX;
			int y = pix /sizeX;
			if (p_t[pix].r == 0 && p_t[pix].g == 0 && p_t[pix].b == 0) {
				
				img[pix].r = 0;
				img[pix].g = 0;
				img[pix].b = 0;
				img[pix].a = 0;
			} else {
				img[pix].r = c_t[pix].r;
				img[pix].g = c_t[pix].g;
				img[pix].b = c_t[pix].b;
				img[pix].a = 255;

			}
		}

		fused_tex.SetPixels32 (img);
		fused_tex.Apply(false);
	}
}
