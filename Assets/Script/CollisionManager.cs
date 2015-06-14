using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

	public Renderer player;

	private Texture2D p_tex;
	
	//private Texture2D fused_tex;
	
	public int sizeX = 320;
	public int sizeY = 240;

	private void setTextures() {
		p_tex = player.GetComponent<Renderer> ().material.mainTexture as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
		setTextures ();
	}

	public Vector2 getTexturePosition(Vector3 pos) {
		RaycastHit hit;
		if (!Physics.Raycast(new Ray(pos,new Vector3(0,0,1)), out hit))
			return new Vector2();
		
		Renderer rend = hit.transform.GetComponent<Renderer>();
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
			return new Vector2();
		
		//Texture2D tex = rend.material.mainTexture as Texture2D;
		Vector2 pixelUV = hit.textureCoord;
		Debug.Log (pixelUV);
		pixelUV.x *= sizeX;
		pixelUV.y *= sizeY;
		return pixelUV;
	}

	public bool getCollision(Vector2 pixel) {
		if (!p_tex)
			return false;
		Color color = p_tex.GetPixel((int)pixel.x,(int)pixel.y);
		if (color.r != 0 || color.g != 0 || color.b != 0) {
			return true;
		}
		return false;
	}

	public void removePixel(Vector2 pixel, int radius) {
		//Vector2 p = new Vector2((int)pixel.x,(int)pixel.y);
		int minX = Mathf.Max(0,(int)pixel.x-radius);
		int maxX = Mathf.Min(sizeX,(int)pixel.x+radius);
		int minY = Mathf.Max(0,(int)pixel.y-radius);
		int maxY = Mathf.Min(sizeX,(int)pixel.y+radius);
		for (int i=minX;i<maxX;i++) {
			for (int j=minY;j<maxY;j++) {
				if (isInCircle(pixel,radius,i,j))
					p_tex.SetPixel(i, j,Color.black);
			}
		}
		p_tex.Apply();
	}

	bool isInCircle(Vector2 pixel, float radius, int x, int y) {
		float r = Mathf.Sqrt(Mathf.Pow(pixel.x-x,2)+Mathf.Pow(pixel.y-y,2));
		if (r < radius)
			return true;
		return false;
	}
}
