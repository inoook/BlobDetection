using UnityEngine;
using System.Collections;

public class FramesPerSecond : MonoBehaviour {
	
	public float updateInterval = 0.5f;
	private float accum = 0.0f;
	private int frames = 0;
	private float timeleft;
	private string fps;
	
	public Color color = Color.white;
	public int fontSize = 12;
	private GUIStyle style;
	
	public ScreenSnap m_snap;
	
	public enum ScreenSnap
    {
        UpperLeftCorner,    ///< the base position (x,y) is an offset from the upper left corner
        UpperRightCorner,   ///< the base position (x,y) is an offset from the upper right corner
        LowerLeftCorner,    ///< the base position (x,y) is an offset from the lower left corner
        LowerRightCorner    ///< the base position (x,y) is an offset from the lower right corner
    }
	
	// Use this for initialization
	void Start () {
		timeleft = updateInterval;
		style = new GUIStyle();
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 ) {
			// display two fractional digits (f2 format)
			fps = "" + (accum/frames).ToString("f2");
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}
	
	private Rect m_placeToDraw = new Rect(0,0,70,20);
	public float width = 100;
	void OnGUI () {
		style.normal.textColor = color;
		style.fontSize = fontSize;
		
		m_placeToDraw.height = fontSize;
		m_placeToDraw.width = width;
		Rect posToPut = m_placeToDraw;
        switch (m_snap)
        {
            case ScreenSnap.UpperRightCorner:
                {
                    posToPut.x = Screen.width - m_placeToDraw.x - m_placeToDraw.width;
                    break;
                }
            case ScreenSnap.LowerLeftCorner:
                {
                    posToPut.y = Screen.height - m_placeToDraw.y - m_placeToDraw.height;
                    break;
                }
            case ScreenSnap.LowerRightCorner:
                {
                    posToPut.x = Screen.width - m_placeToDraw.x - m_placeToDraw.width;
                    posToPut.y = Screen.height - m_placeToDraw.y - m_placeToDraw.height;
                    break;
                }

        }
		
		GUI.Label(posToPut, "FPS " + fps, style);
	}
}
