using UnityEngine;
using System.Collections;

using BlobDetectionNS;

public class BlobDetectionWebCamTest : Processing
{

	BlobDetection theBlobDetection;
	PImage cam;
	PImage img;
	public Texture2D texture;
	private WebCamTexture camTexture;
	private PImage testPImage;


	public int w = 320;
	public int h = 240;

	public float min = 0.01f;
	
	// Use this for initialization
	void Start ()
	{
		Processing.SCREEN_W = Screen.width;
		Processing.SCREEN_H = Screen.height;

		camTexture = new WebCamTexture();
		camTexture.Play ();

		cam = new PImage (w, h);
		img = new PImage (w, h); // imgに対して輪郭抽出処理を行う
		
		theBlobDetection = new BlobDetection (img.width, img.height);
		theBlobDetection.setPosDiscrimination (true);
		
	}
	
	void OnApplicationQuit ()
	{
		if (camTexture != null) {
			camTexture.Stop ();
		}
	}
	public float threshold = 0.2f;
	public int blur = 2;
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();

		theBlobDetection.min = min;

		
		//threshold
		if (Input.GetKeyDown (KeyCode.S)) {
			threshold += 0.01f;
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			threshold += -0.01f;
		}
		threshold = Mathf.Clamp (threshold, 0, 1.0f);
		
		// blur
		if (Input.GetKeyDown (KeyCode.X)) {
			blur += 1;
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			blur += -1;
		}
		blur = Mathf.Clamp (blur, 0, 10);

		//
		theBlobDetection.setThreshold (threshold); // will detect bright areas whose luminosity > 0.2f;
		
		// RESIZE
//		tmpTexture.SetPixels32(camTexture.GetPixels32());
//		Texture2D dstTexture = ScaleTexture(tmpTexture, 320, 240);
//		cam.SetPixels32(dstTexture.GetPixels32());

//		Color32[] colors = ScaleTextureColors(camTexture.GetPixels32(), camTexture.width, camTexture.height, w, h);
//		cam.SetPixels32(colors);

		byte[] colorBytes = ScaleTextureBytes(camTexture.GetPixels32(), camTexture.width, camTexture.height, w, h);
		cam.SetPixelsBytes(colorBytes);


		img.copy (cam);
		fastblur (img, blur);
		
		theBlobDetection.computeBlobs (img.pixelsInt);
		
//		drawBlobsAndEdges (true, true);
	}

	// http://jon-martin.com/?p=114
	private Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) {
		Texture2D result=new Texture2D(targetWidth,targetHeight, source.format, true);
		
		Color[] rpixels = new Color[targetWidth * targetHeight];
		float incX=(1.0f / (float)targetWidth);
		float incY=(1.0f / (float)targetHeight); 
		for(int px=0; px<rpixels.Length; px++) { 
			rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth))); 
		} 
		result.SetPixels(rpixels,0);
		result.Apply();
		return result; 
	}

	private Color32[] ScaleTextureColors(Color32[] srcPixels, int srcWidth, int srcHeight, int dstWidth, int dstHeight) {
		Color32[] dstPixels = new Color32[dstWidth * dstHeight];
		float incX=(1.0f / (float)dstWidth);
		float incY=(1.0f / (float)dstHeight);

		for(int i = 0; i < dstPixels.Length; i++) {
			int x = i % dstWidth;
			int y = Mathf.FloorToInt(i/dstWidth);
			int index = (int)(srcWidth * incX*((float)x)) + (int)(srcHeight * incY*((float)y)) * srcWidth;
			dstPixels[i] = srcPixels[index];
		} 
		return dstPixels;
	}

	private byte[] ScaleTextureBytes(Color32[] srcPixels, int srcWidth, int srcHeight, int dstWidth, int dstHeight) {
		byte[] dstPixels = new byte[dstWidth * dstHeight * 4];
		float incX=(1.0f / (float)dstWidth);
		float incY=(1.0f / (float)dstHeight);

		byte[] srcBytes = Utils.Color32ArrayToByteArray(srcPixels);
		int num = dstWidth * dstHeight;

		for(int i = 0; i < num; i++) {
			int x = i % dstWidth;
			int y = Mathf.FloorToInt(i/dstWidth);
			int index = (int)(srcWidth * incX*((float)x)) + (int)(srcHeight * incY*((float)y)) * srcWidth;
			dstPixels[i*4+0] = srcBytes[index*4+0];
			dstPixels[i*4+1] = srcBytes[index*4+1];
			dstPixels[i*4+2] = srcBytes[index*4+2];
			dstPixels[i*4+3] = srcBytes[index*4+3];
		} 
		return dstPixels;
	}
	
	
	public bool drawBlobs = true;
	public bool drawEdges = true;

	void drawBlobsAndEdges (int width, int height)
	{
		Blob b;
		EdgeVertex eA, eB;
		int num = theBlobDetection.getBlobNb ();

		for (int n=0; n < num; n++) {
			b = theBlobDetection.getBlob (n);
			
			if (b != null) {
				// Edges
				if (drawEdges) {
					for (int m=0; m < b.getEdgeNb(); m++) {
						eA = b.getEdgeVertexA (m);
						eB = b.getEdgeVertexB (m);
						if (eA != null && eB != null) {
							glDrawer.DrawLine (
								eA.x * width, (1 - eA.y) * height, 
								eB.x * width, (1 - eB.y) * height,
								Color.green
								);
						}
					}
				}
				
				// Blobs
				if (drawBlobs) {
					glDrawer.DrawRect (
						b.xMin * width, (1 - b.yMin) * height,
						b.w * width, -b.h * height,
						Color.red
						);
				}
				
			}
		}
	}
	

	public Drawer glDrawer;
	
	void OnGUI ()
	{

		GUI.depth = 20;
		
		int width = camTexture.width;
		int height = camTexture.height;
		
		GUI.DrawTexture (new Rect (0, 0, width, height), camTexture);
		
		// drawBlobsAndEdges --------------
		drawBlobsAndEdges(width, height);
		
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

				GUI.Label(new Rect(rect.x + rect.width/2, rect.y + rect.height/2, 200,200), "#"+n + "/ id: "+b.id + " / "+ (b.w*b.h));
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
				data [yi * 4 + 0] = (dv [rsum]);
				data [yi * 4 + 1] = (dv [gsum]);
				data [yi * 4 + 2] = (dv [bsum]);
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
