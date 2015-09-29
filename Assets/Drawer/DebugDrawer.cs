using UnityEngine;
using System.Collections;

public class DebugDrawer : Drawer {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void DrawLine (float x0, float y0, float x1, float y1, Color color)
	{
		Debug.DrawLine (new Vector3 (x0, y0, 0), new Vector3 (x1, y1, 0), color);
	}
	
	public override void DrawRect (float xMin, float yMin, float w, float h, Color color)
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
}
