using UnityEngine;
using System.Collections;

using BlobDetectionNS;

public class BlobDetectionTest : Processing
{

	BlobDetection theBlobDetection;
	PImage cam;
	PImage img;
	public Texture2D texture;
	private WebCamTexture camTexture;
	private PImage testPImage;
	
	// Use this for initialization
	void Start ()
	{
		Processing.SCREEN_W = Screen.width;
		Processing.SCREEN_H = Screen.height;
		
		int w = texture.width;
		int h = texture.height;
		/*
		texture = new Texture2D(320, 240);
		*/
		cam = new PImage (w, h);
		img = new PImage (w, h); // imgに対して輪郭抽出処理を行う
		
		theBlobDetection = new BlobDetection (img.width, img.height);
		theBlobDetection.setPosDiscrimination (true);
		
//		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
//        if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
//			camTexture = new WebCamTexture (w, h, 30);
//			camTexture.Play ();
//        } else {
//			// Error
//        }
		
	}
	
	void OnApplicationQuit ()
	{
		if (camTexture != null) {
			camTexture.Stop ();
		}
	}
	
	Texture2D tmpTexture;
	public float threshold = 0.2f;
	public int blur = 2;
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();

//		if(camTexture == null){
//			return;
//		}
		//threshold
		if (Input.GetKeyDown (KeyCode.S)) {
			threshold += 0.01f;
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			threshold += -0.01f;
		}
		threshold = Mathf.Clamp (threshold, 0, 0.5f);
		
		// blur
		if (Input.GetKeyDown (KeyCode.X)) {
			blur += 1;
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			blur += -1;
		}
		blur = Mathf.Clamp (blur, 1, 10);
		
		
		//		
		theBlobDetection.setThreshold (threshold); // will detect bright areas whose luminosity > 0.2f;
		
		// RESIZE
		/*
		tmpTexture = new Texture2D(camTexture.width, camTexture.height, TextureFormat.RGBA32, false);
		tmpTexture.SetPixels32(camTexture.GetPixels32());
		
		TextureScale.Point(tmpTexture, cam.width, cam.height);//RESIZE
		cam.SetPixels32(tmpTexture.GetPixels32());
		*/
		// NORMAL
//		cam.SetPixels32( texture.GetPixels32 ());
		cam.SetPixelsBytes (texture.GetRawTextureData ());
		
		img.copy (cam);
		fastblur (img, blur);
		
		theBlobDetection.computeBlobs (img.pixelsInt);
		
		//drawBlobsAndEdges (true, true);
	}
	
	
		
	// ==================================================
	// drawBlobsAndEdges()
	// ==================================================
	void drawBlobsAndEdges (bool drawBlobs, bool drawEdges)
	{
		Blob b;
		EdgeVertex eA, eB;
		int num = theBlobDetection.getBlobNb ();
		//Debug.Log(num);
		for (int n=0; n < num; n++) {
			b = theBlobDetection.getBlob (n);
			if (b != null) {
				// Edges
				if (drawEdges) {
					//Debug.Log("getEdgeNb: "+b.getEdgeNb());
					//stroke(0,255,0);
					for (int m=0; m<b.getEdgeNb(); m++) {
						eA = b.getEdgeVertexA (m);
						eB = b.getEdgeVertexB (m);
						if (eA != null && eB != null) {
							
							line (
								eA.x * width, eA.y * height, 
								eB.x * width, eB.y * height,
								Color.green
							);
						}
					}
				}
				
				// Blobs
				if (drawBlobs) {
					//strokeWeight(1);
					//stroke(255,0,0);
					
					rect (
						b.xMin * width, b.yMin * height,
						b.w * width, b.h * height,
						Color.red
					);
				}
				
			}
	
		}
	}

	void line (float x0, float y0, float x1, float y1, Color color)
	{
		Debug.DrawLine (new Vector3 (x0, y0, 0), new Vector3 (x1, y1, 0), color);
	}

	void rect (float xMin, float yMin, float w, float h, Color color)
	{
		Vector3 p0 = new Vector3 (xMin, yMin, 0);
		Vector3 p1 = new Vector3 (xMin + w, yMin, 0);
		Vector3 p2 = new Vector3 (xMin + w, yMin + h, 0);
		Vector3 p3 = new Vector3 (xMin, yMin + h, 0);
		Debug.DrawLine (p0, p1, color);
		Debug.DrawLine (p1, p2, color);
		Debug.DrawLine (p2, p3, color);
		Debug.DrawLine (p3, p0, color);
	}
	//
	public GLdrawer glDrawer;
	
	void OnGUI ()
	{
		
		GUI.depth = 20;
		
		int width = 640;
		int height = 480;
		
		GUI.DrawTexture (new Rect (0, 0, width, height), texture);

//		CreateLineMaterial();
		
		// drawBlobsAndEdges --------------
		bool drawBlobs = true;
		bool drawEdges = true;
		Blob b;
		EdgeVertex eA, eB;
		int num = theBlobDetection.getBlobNb ();
		//Debug.Log(num);
		for (int n=0; n < num; n++) {
			b = theBlobDetection.getBlob (n);
			
			if (b != null) {
				// Edges
				if (drawEdges) {
					//Debug.Log("getEdgeNb: "+b.getEdgeNb());
					//stroke(0,255,0);
					for (int m=0; m < b.getEdgeNb(); m++) {
						eA = b.getEdgeVertexA (m);
						eB = b.getEdgeVertexB (m);
						if (eA != null && eB != null) {
							/*
							GL_rect (
								eA.x * width, (1-eA.y) * height,
								5, 5,
								Color.red
								);
							*/
							glDrawer.GL_line (
								eA.x * width, (1 - eA.y) * height, 
								eB.x * width, (1 - eB.y) * height,
								Color.green
							);
						}
					}
				}
				
				// Blobs
				if (drawBlobs) {
					//strokeWeight(1);
					//stroke(255,0,0);
					//GUI.Label(new Rect(b.xMin * width, (1-b.yMin) * height, 30,30), "id: "+ (b.id).ToString());
					glDrawer.GL_rect (
						b.xMin * width, (1 - b.yMin) * height,
						b.w * width, -b.h * height,
						Color.red
					);
				}
				
			}
		}
		
		string str = "threshold: " + threshold.ToString ("0.00") + " (A, S)\n";
		str += "blur: " + blur.ToString () + " (Z, X)";
		GUI.Label (new Rect (10, 10, 200, 100), str);
	}

	//
	// ==================================================
	// Super Fast Blur v1.1
	// by Mario Klingemann 
	// <http://incubator.quasimondo.com>
	// ==================================================
	public void fastblur (PImage img, int radius)
	{
		if (radius < 1) {
			return;
		}
		int w = img.width;
		int h = img.height;
		int wm = w - 1;
		int hm = h - 1;
		int wh = w * h;
		int div = radius + radius + 1;
		int[] r = new int[wh];
		int[] g = new int[wh];
		int[] b = new int[wh];
		int rsum, gsum, bsum;
		int x, y, i, p1, p2, yp, yi, yw;
		int[] vmin = new int[Mathf.Max (w, h)];
		int[] vmax = new int[Mathf.Max (w, h)];
		
		byte[] data = img.pixelsInt;
		
		byte[] dv = new byte[256 * div];
		for (i=0; i<256*div; i++) {
			dv [i] = (byte)(i / div);
		}
		
		yw = yi = 0;
		
		for (y=0; y<h; y++) {
			rsum = gsum = bsum = 0;
			for (i=-radius; i<=radius; i++) {
				int index = yi + Mathf.Min (wm, Mathf.Max (i, 0));
				//					p = pix [index];
				rsum += (data [index * 4 + 0]);
				gsum += (data [index * 4 + 1]);
				bsum += (data [index * 4 + 2]);
			}
			for (x=0; x<w; x++) {
				
				r [yi] = dv [rsum];
				g [yi] = dv [gsum];
				b [yi] = dv [bsum];
				
				if (y == 0) {
					vmin [x] = Mathf.Min (x + radius + 1, wm);
					vmax [x] = Mathf.Max (x - radius, 0);
				}
				int index1 = (yw + vmin [x]) * 4;
				int index2 = (yw + vmax [x]) * 4;
				
				rsum += (data [index1 + 0]) - (data [index2 + 0]);
				gsum += (data [index1 + 1]) - (data [index2 + 1]);
				bsum += (data [index1 + 2]) - (data [index2 + 2]);
				
				yi++;
			}
			yw += w;
		}
		for (x=0; x<w; x++) {
			rsum = gsum = bsum = 0;
			yp = -radius * w;
			for (i=-radius; i<=radius; i++) {
				yi = Mathf.Max (0, yp) + x;
				rsum += r [yi];
				gsum += g [yi];
				bsum += b [yi];
				yp += w;
			}
			yi = x;
			
			for (y=0; y<h; y++) {
				data [yi * 4 + 0] = (byte)(dv [rsum]);
				data [yi * 4 + 1] = (byte)(dv [gsum]);
				data [yi * 4 + 2] = (byte)(dv [bsum]);
				data [yi * 4 + 3] = 0xff;
				
				if (x == 0) {
					vmin [y] = Mathf.Min (y + radius + 1, hm) * w;
					vmax [y] = Mathf.Max (y - radius, 0) * w;
				}
				p1 = x + vmin [y];
				p2 = x + vmax [y];
				
				rsum += r [p1] - r [p2];
				gsum += g [p1] - g [p2];
				bsum += b [p1] - b [p2];
				
				yi += w;
			}
		}
	}

}
