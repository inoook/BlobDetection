using UnityEngine;
using System.Collections;

public class Processing : MonoBehaviour
{
	public static int SCREEN_W = 0;
	public static int SCREEN_H = 0;
	
	public enum PImangeFormat
	{
		RGB
	}
	
	// math
	public static float random (float min, float max)
	{
		return Random.Range (min, max);
	}

	public static float sqrt (float f)
	{
		return Mathf.Sqrt (f);
	}
	
	protected void frameRate (int fps)
	{
		Application.targetFrameRate = fps;
	}
	
	// nouse  --------------------------
	protected void background (Color color)
	{
		Camera.main.backgroundColor = color;
	}
	
	protected void size (int width, int height, int Mode = 0)
	{
		LogNoImplement ("size");
	}

	protected void hint ()
	{
		LogNoImplement ("hint");
	}
	
	protected void lights ()
	{
		LogNoImplement ("lights");
	}
	
	private void LogNoImplement (string str)
	{
		Debug.Log ("implementation: " + str);
	}
	
	// color --------------------------
	
	public enum ColorMode
	{
		RGB,
		HSB
	}

//	public ColorMode currentColorMode = ColorMode.RGB;
	private float m_range1;
	private float m_range2;
	private float m_range3;
	
	public void colorMode (ColorMode mode)
	{
//		currentColorMode = mode;
	}

	public void colorMode (ColorMode mode, float range)
	{
		colorMode (mode);
//		m_range1 = m_range2 = m_range3 = range;
	}

	public void colorMode (ColorMode mode, float range1, float range2, float range3)
	{
		colorMode (mode);
//		m_range1 = range1;
//		m_range2 = range2;
//		m_range3 = range3;
	}
	
	/*
	public colorP5 color(int r, int g, int b){
		return new colorP5((float)r/255.0f, (float)g/255.0f, (float)b/255.0f, 1.0f);
	}
	*/
	public colorP5 color (float r, float g, float b)
	{
		return new colorP5 (r, g, b, 1.0f);
	}
	
	// hue
	public colorP5 colorHue (float hue, float s, float b)
	{
		return new colorP5 (hue, s, b);
	}
	
	public float red (colorP5 color)
	{
		return color.m_color.r;
	}

	public float green (colorP5 color)
	{
		return color.m_color.g;
	}

	public float blue (colorP5 color)
	{
		return color.m_color.b;
	}
	// image  --------------------------
	
	public PImage createImage (int width, int height, PImangeFormat format)
	{
		return new PImage (width, height);
	}
	
	public void image (PImage img, int x, int y, int width, int height)
	{
		/*
		image(画像ファイル名, x, y); – 座標(x, y)を左上にして、画像を表示
		image(画像ファイル名, x, y, width, height); – 座標(x, y)を左上にして、幅(width)と高さ(height)を指定して画像を表示
		*/
	}
	
	// debug  --------------------------
	
	public void println (string str)
	{
		Debug.Log (str);
	}
	
	public int width {
		get{ return Processing.SCREEN_W; }
	}

	public int height {
		get{ return Processing.SCREEN_H; }
	}
	
	protected int frameCount;
	protected float mouseX;
	protected float mouseY;
	protected float pmouseX;
	protected float pmouseY;
	
	void Start ()
	{
		Vector3 mousePos = Input.mousePosition;
		pmouseX = mousePos.x;
		pmouseX = mousePos.y;
		
		frameCount = 0;
		
		setup ();
	}
	
	public virtual void Update ()
	{
		frameCount ++;
		Vector3 mousePos = Input.mousePosition;
		mouseX = mousePos.x;
		mouseY = mousePos.y;
		
		//
		draw ();
		mouseMoved ();
		//
		pmouseX = mouseX;
		pmouseY = mouseY;
		
		if (Input.anyKeyDown) {
			key = Input.inputString;
			if (key != "") {
				keyPressed ();
				//Debug.LogWarning(key);
			}
		}
		if (Input.GetMouseButtonDown (0)) {
			mousePressed ();
		}
	}
	
	protected string key;
	/*
	public virtual void OnGUI()
	{
		Event e = Event.current;
        if (e.isKey){
			key = (e.keyCode).ToString();
		}
		
		if(e.isMouse){
			if(e.type == EventType.MouseDown){
				mousePressed();
			}	
		}
		
	}
	*/
	public virtual void setup ()
	{
		
	}
	
	public virtual void mouseMoved ()
	{
	}
	
	public virtual void draw ()
	{
		
	}
	
	public virtual void mousePressed ()
	{
	   
	}
	
	public virtual void keyPressed ()
	{
		
	}
	
}

public class PImage
{
	//[HideInInspector]
	//public colorP5[] pixels;
		
	public byte[] pixelsInt;
	public int width;
	public int height;
		
	public PImage (int w, int h)
	{
			
		width = w;
		height = h;
			
		pixelsInt = new byte[width * height * 4];
		for (int i = 0; i < width*height; i++) {
			pixelsInt [i * 4 + 0] = 0;
			pixelsInt [i * 4 + 1] = 0;
			pixelsInt [i * 4 + 2] = 0;
			pixelsInt [i * 4 + 3] = 255;
		}
	}
		
	public void copy (PImage srcImg)
	{
		width = srcImg.width;
		height = srcImg.height;
		/*
			pixelsInt = new int[srcImg.pixelsInt.Length];
			for(int i = 0; i < pixelsInt.Length; i++){
				pixelsInt[i] = srcImg.pixelsInt[i];
			}
			*/
		pixelsInt = srcImg.pixelsInt;
	}
		
	// unity
	public void SetTexture2D (Texture2D texture_)
	{
		width = texture_.width;
		height = texture_.height;
			
		Color32[] colors = texture_.GetPixels32 ();
		SetPixels32 (colors);
	}

	public void SetPixels32 (Color32[] colors)
	{
		pixelsInt = new byte[colors.Length * 4];
		for (int i = 0; i < colors.Length; i++) {
			Color32 c = colors [i];
//				pixelBytes[i] = c.a << 24 | c.r << 16 | c.g << 8 | c.b;
			pixelsInt [i * 4 + 0] = c.r;
			pixelsInt [i * 4 + 1] = c.g;
			pixelsInt [i * 4 + 2] = c.b;
			pixelsInt [i * 4 + 3] = c.a;
		}
	}

	public void SetPixelsBytes (byte[] bytes)
	{
		pixelsInt = bytes;
	}
	// unity
	public Texture2D GetTexture2D ()
	{
		Texture2D texture = new Texture2D (width, height, TextureFormat.RGBA32, false);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Trilinear;
			
		// pixelsIntでテクスチャを描画
		texture.LoadRawTextureData (pixelsInt);
		texture.Apply (false);
			
		return texture;
	}
		
	//
	public static Color ConvColor (colorP5 cP5)
	{
		return cP5.m_color;
	}
		
	public static Color[] ConvColors (colorP5[] cP5s)
	{
		Color[] colors = new Color[cP5s.Length];
		for (int i = 0; i < cP5s.Length; i++) {
			colors [i] = ConvColor (cP5s [i]);
		}
		return colors;
	}
		
	public static Color32[] ConvColors32 (colorP5[] cP5s)
	{
		Color32[] colors = new Color32[cP5s.Length];
		for (int i = 0; i < cP5s.Length; i++) {
			colors [i] = (Color32)(cP5s [i].m_color);
		}
		return colors;
	}

	public static Color32[] ConvColorsInt (int[] intColors)
	{
		Color32[] colors = new Color32[intColors.Length];
		for (int i = 0; i < intColors.Length; i++) {
			int intColor = intColors [i];
			byte r = (byte)(intColor >> 16 & 0xFF);
			byte g = (byte)(intColor >> 8 & 0xFF);
			byte b = (byte)(intColor & 0xFF);
			byte a = 0x00;
			colors [i] = new Color32 (r, g, b, a);
		}
		return colors;
	}
	/*
		 cc.r = (byte)((c & 0x00FF0000) >> 16);
		cc.g = (byte)((c & 0x0000FF00) >> 8);
		cc.b = (byte)((c & 0x000000FF));
		cc.a = (byte)((c & 0xFF000000) >> 24);
		//
		pixelsInt[i] = c.a << 24 | c.r << 16 | c.g << 8 | c.b;
		*/
}

[System.Serializable]
public class colorP5
{
	public Color m_color;
		
	public colorP5 (float r, float g, float b, float a)
	{
		m_color = colorToColor (r, g, b, a);
	}
		
	public colorP5 (float h, float s, float b)
	{
		m_color = colorToColor (new HSBColor (h / 360.0f, s, b).ToColor ());
	}
	/*
		public colorP5(int r, int g, int b, int a)
		{
			m_color = colorToColor32(r, g, b, a);
		}
		*/
		
	public static Color colorToColor (Color color)
	{
		return color;
	}
		
	public static Color colorToColor (float r, float g, float b, float a)
	{
		return new Color (r, g, b, a);
	}
		
	public static Color32 colorToColor32 (Color color)
	{
		return new Color32 ((byte)((int)(color.r * 255.0f)), (byte)((int)(color.g * 255.0f)), (byte)((int)(color.b * 255.0f)), (byte)((int)(color.a * 255.0f)));
	}
		
	public static Color32 colorToColor32 (float r, float g, float b, float a)
	{
		//return new Color32((byte)((int)(r*255.0f)), (byte)((int)(g*255.0f)), (byte)((int)(b*255.0f)), (byte)((int)(a*255.0f)) );
		return new Color32 (floatToByte (r), floatToByte (g), floatToByte (b), floatToByte (a));
	}

	public static byte floatToByte (float v)
	{
		//return (byte)( UnityEngine.Mathf.FloorToInt(v * 255) );
		return (byte)((UnityEngine.Mathf.FloorToInt (v * 255.0f)) & 0xFF);
	}
		
	public static Color ToColor (colorP5 colorP5_)
	{
		return colorP5_.m_color;
	}
		
	public static colorP5 ToColorP5 (Color color)
	{
		return new colorP5 (color.r, color.g, color.b, color.a);
	}
}

public class PVector
{
	public Vector3 pos;
		
	public PVector (float x, float y, float z = 0)
	{
		pos = new Vector3 ();
		pos.x = x;
		pos.y = y;
		pos.z = z;
	}
		
	public PVector () : this(0, 0, 0)
	{
	}
		
	// clone
	public PVector get ()
	{
		return new PVector (pos.x, pos.y, pos.z);
		//return this;
	}
		
	public void normalize ()
	{
		pos.Normalize ();
	}

	public float heading2D ()
	{
		// radian
		return Mathf.Atan2 (pos.y, pos.x);
	}

	public float heading ()
	{
		// 3Dには対応していない。
		// radian
		return heading2D ();
		//return Quaternion.LookRotation(pos).w; // ????
	}
		
	public void add (PVector p)
	{
		pos += p.pos;
	}

	public static PVector add (PVector v1, PVector v2)
	{
		PVector p = new PVector ();
		p.pos = v1.pos + v2.pos;
		return p;
	}

	public void sub (PVector p)
	{
		pos += -p.pos;
	}

	public static PVector sub (PVector v1, PVector v2)
	{
		PVector p = new PVector ();
		p.pos = v1.pos - v2.pos;
		return p;
	}

	public void mult (float n)
	{
		pos *= n;
	}

	public static PVector mult (PVector v, float n)
	{
		PVector p = new PVector ();
		p.pos = v.pos * n;
		return p;
	}
		
	public void div (float n)
	{
		pos *= 1.0f / n;
	}
		
	public float mag ()
	{
		return pos.magnitude;
	}

	public float magSq ()
	{
		return pos.sqrMagnitude;
	}
		
	public static float dist (PVector a, PVector b)
	{
		return Vector3.Distance (a.pos, b.pos);
	}
		
	public float dot (PVector a)
	{
		return Vector3.Dot (pos, a.pos);
	}
		
	public void limit (float v)
	{
		if (pos.magnitude > v) {
			pos.Normalize ();
			pos *= v;
		}
	}
		
	public float x {
		get{ return pos.x; }
		set{ pos.x = value; }
	}

	public float y {
		get{ return pos.y; }
		set{ pos.y = value; }
	}

	public float z {
		get{ return pos.z; }
		set{ pos.z = value; }
	}
}

public class PApplet
{
	public static float radians (float degree)
	{
		return Mathf.Deg2Rad * degree;
	}

	public static void ellipse (float x, float y, float w, float h)
	{
		ellipse (x, y, w, h, Color.white);
	}

	public static void ellipse (float x, float y, float w, float h, Color color)
	{
		Debug.DrawLine (new Vector3 (x - w / 2, y, 0), new Vector3 (x + w / 2, y, 0), color);
		Debug.DrawLine (new Vector3 (x, y - h / 2, 0), new Vector3 (x, y + h / 2, 0), color);
	}

	public static void ellipse (float x, float y, float z, float w, float h, float d)
	{
		ellipse (x, y, z, w, h, d, Color.white);
	}

	public static void ellipse (float x, float y, float z, float w, float h, float d, Color color)
	{
		Debug.DrawLine (new Vector3 (x - w / 2, y, z), new Vector3 (x + w / 2, y, z), color);
		Debug.DrawLine (new Vector3 (x, y - h / 2, z), new Vector3 (x, y + h / 2, z), color);
		Debug.DrawLine (new Vector3 (x, y, z - d / 2), new Vector3 (x, y, z + d / 2), color);
	}
		
	public static void line (float startX, float startY, float endX, float endY)
	{
		Debug.DrawLine (new Vector3 (startX, startY, 0), new Vector3 (endX, endY, 0));
	}

	public static void line (float startX, float startY, float startZ, float endX, float endY, float endZ)
	{
		Debug.DrawLine (new Vector3 (startX, startY, startZ), new Vector3 (endX, endY, endZ));
	}

	public static void line (PVector p0, PVector p1)
	{
		Debug.DrawLine (new Vector3 (p0.x, p0.y, p0.z), new Vector3 (p1.x, p1.y, p1.z));
	}
		
}