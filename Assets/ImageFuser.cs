using UnityEngine;
using System.Collections;

public class ImageFuser : MonoBehaviour {

	public Renderer depth;
	public Renderer color;
	public Renderer player;
	public Renderer playerFuse;

	private Texture2D d_tex;
	private Texture2D c_tex;
	private Texture2D p_tex;
	private Texture2D f_tex;

	private Texture2D fused_tex;

	public int sizeX = 320;
	public int sizeY = 240;
	public int displayDepth = 126;

	public int moveBack = 0;

	public bool gameMode;
	
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
		f_tex = playerFuse.GetComponent<Renderer> ().material.mainTexture as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
		setTextures ();
		Color32[] img = new Color32[sizeX*sizeY];
		Color32[] p_t;
		if (!gameMode)
			p_t = p_tex.GetPixels32 ();
		else
			p_t = f_tex.GetPixels32 ();
			

		Color32[] c_t = c_tex.GetPixels32 ();
		Color32[] d_t = d_tex.GetPixels32 ();
		moveBack = 0;
		for (int pix = 0; pix < img.Length; pix++)
		{
			int x = pix %sizeX;
			int y = pix /sizeX;
			if ((p_t[pix].r == 0 && p_t[pix].g == 0 && p_t[pix].b == 0)) {
				
				img[pix].r = 0;
				img[pix].g = 0;
				img[pix].b = 0;
				img[pix].a = 0;
			} else {
				if ( d_t[pix].r > displayDepth) {
					img[pix].r = p_t[pix].r;
					img[pix].g = p_t[pix].g;
					img[pix].b = p_t[pix].b;
					img[pix].a = 255;
					moveBack++;
				} else {
					img[pix].r = 128;
					img[pix].g = 128;
					img[pix].b = 128;
					img[pix].a = 128;
				}

			}
		}

		fused_tex.SetPixels32 (img);
		fused_tex.Apply(false);
	}
}
