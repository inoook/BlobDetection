using UnityEngine;
using System.Collections;

public class DrawerTest : MonoBehaviour {

	public GLDrawer glDrawer;
	public DebugDrawer debugDrawer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		debugDrawer.DrawLine(0,0,1,1, Color.red);
		debugDrawer.DrawRect(1,1,1,1, Color.green);
	}

	void OnGUI()
	{
		glDrawer.DrawLine(100,0,200,100, Color.red);
		glDrawer.DrawRect(200,100,100,100, Color.green);
	}
}
