using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

	public Renderer player;

	private Texture2D p_tex;
	
	//private Texture2D fused_tex;
	
	public int sizeX = 320;
	public int sizeY = 240;
	
	//private Renderer renderer;

	// Use this for initialization
	void Start () {
		//renderer = GetComponent<Renderer>();
		
		//fused_tex = new Texture2D(sizeX,sizeY,TextureFormat.ARGB32,false);
		//renderer.material.mainTexture = fused_tex;
	}

	private void setTextures() {
		p_tex = player.GetComponent<Renderer> ().material.mainTexture as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
		setTextures ();

		//transform.Translate(Vector3.down*Time.deltaTime);

		//Debug.Log("ett "+p_tex.GetPixel(160,120));
		
		//Vector2 pixel = getTexturePosition(transform.position);

		//Debug.Log(pixelUV.x+" "+pixelUV.y);
		//Debug.Log("två "+p_tex.GetPixel((int)pixelUV.x,(int)pixelUV.y));
		//Color color = p_tex.GetPixel((int)pixel.x,(int)pixel.y);
		//if (!p_tex.GetPixel((int)pixelUV.x,(int)pixelUV.y).Equals(Color.black))
		/*if (getCollision(pixel)) {
			p_tex.SetPixel((int)pixel.x, (int)pixel.y,Color.black);
			p_tex.Apply();
			//Destroy(gameObject);
		}*/
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
		pixelUV.x *= sizeX;
		pixelUV.y *= sizeY;
		return pixelUV;
	}

	public bool getCollision(Vector2 pixel) {
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
