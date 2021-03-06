using UnityEngine;
using System.Collections;

using BlobDetectionNS;

public class BlobDetectionTest : Processing
{

	BlobDetection theBlobDetection;
	PImage cam;
	PImage img;
	public Texture2D texture;
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
		
	}
	
	void OnApplicationQuit ()
	{

	}
	
	Texture2D tmpTexture;
	public float threshold = 0.2f;
	public int blur = 2;
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();

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
		cam.SetPixelsBytes (texture.GetRawTextureData ());
		
		img.copy (cam);
		fastblur (img, blur);
		
		theBlobDetection.computeBlobs (img.pixelsInt);
		
//		drawBlobsAndEdges (true, true);
		drawBlobsAndEdges(640, 480);
	}

	public bool drawBlobs = true;
	public bool drawEdges = true;

	void drawBlobsAndEdges (int width, int height)
	{
		Blob b;
		EdgeVertex eA, eB;
		int num = theBlobDetection.getBlobNb ();

		meshDrawer.Init();

//		Vector3 t_pos0 = Camera.main.ScreenToWorldPoint(new Vector3( 0, 0, 10 ));
//		Vector3 t_pos1 = Camera.main.ScreenToWorldPoint(new Vector3( Screen.width, Screen.height, 10 ));
//		meshDrawer.DrawLine(t_pos0, t_pos1, Color.green);

		Vector3 offset = new Vector3(-0.5f, -0.5f, 0);

		for (int n=0; n < num; n++) {
			b = theBlobDetection.getBlob (n);
			
			if (b != null) {
				// Edges
				if (drawEdges) {
					for (int m=0; m < b.getEdgeNb(); m++) {
						eA = b.getEdgeVertexA (m);
						eB = b.getEdgeVertexB (m);
						if (eA != null && eB != null) {
							Vector3 pos0 = new Vector3( eA.x, eA.y, 0 ) + offset;
							Vector3 pos1 = new Vector3( eB.x, eB.y, 0 ) + offset;
							meshDrawer.DrawLine(
								pos0,
								pos1,
								Color.green
								);
						}
					}
				}
				
				// Blobs
				if (drawBlobs) {
					Vector3 rect_pos0 = (new Vector3( b.xMin, b.yMin, 0 )) + offset;
					Vector3 rect_pos1 = (new Vector3( b.xMin + b.w, b.yMin, 0 )) + offset;
					Vector3 rect_pos2 = (new Vector3( b.xMin + b.w, b.yMin + b.h, 0 )) + offset;
					Vector3 rect_pos3 = (new Vector3( b.xMin, b.yMin + b.h, 0 )) + offset;
					meshDrawer.DrawRect (
						rect_pos0,
						rect_pos1,
						rect_pos2,
						rect_pos3,
						Color.red
						);
				}
				
			}
		}

		meshDrawer.Render();

	}

	public MeshDrawer meshDrawer;

	void OnGUI ()
	{
		
		GUI.depth = 20;
		
		int width = 640;
		int height = 480;
		
//		GUI.DrawTexture (new Rect (0, 0, width, height), texture);
//		Graphics.DrawTexture(new Rect (8, 8, width, height), texture, null);

//		CreateLineMaterial();
		
		// drawBlobsAndEdges --------------
//		drawBlobsAndEdges(width, height);
		
		string str = "threshold: " + threshold.ToString ("0.00") + " (A, S)\n";
		str += "blur: " + blur.ToString () + " (Z, X)";
		GUI.Label (new Rect (10, 10, 200, 100), str);

		//
		Blob b;
		EdgeVertex eA, eB;
		int num = theBlobDetection.getBlobNb ();
		
		for (int n=0; n < num; n++) {
			b = theBlobDetection.getBlob (n);
			
			if (b != null) {
				// Blobs
				Rect rect = new Rect(b.xMin * width, (1 - b.yMin) * height,
					                 b.w * width, -b.h * height);

				GUI.Label(new Rect(rect.x, rect.y, 200,200), "#"+n);
			}
		}
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
