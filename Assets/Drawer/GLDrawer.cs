using UnityEngine;
using System.Collections;

public class GLDrawer : Drawer
{

	static Material lineMaterial;

	static void CreateLineMaterial ()
	{
		if (!lineMaterial) {
			lineMaterial = new Material ("Shader \"Lines/Colored Blended\" {" +
				"SubShader { Pass { " +
				"    Blend SrcAlpha OneMinusSrcAlpha " +
				"    ZWrite Off Cull Off Fog { Mode Off } " +
				"    BindChannels {" +
				"      Bind \"vertex\", vertex Bind \"color\", color }" +
				"} } }");
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	void Start ()
	{

	}

	public override void DrawLine (float x0, float y0, float x1, float y1, Color color)
	{
		CreateLineMaterial ();

		GL.PushMatrix ();
		GL.LoadIdentity ();
		lineMaterial.SetPass (0);
		
		GL.Begin (GL.LINES);
		GL.Color (color);
		
		GL.Vertex (new Vector3 (x0, y0, 0));
		GL.Vertex (new Vector3 (x1, y1, 0));
		
		GL.End ();
		GL.PopMatrix ();
	}

	public override void DrawRect (float xMin, float yMin, float w, float h, Color color)
	{
		CreateLineMaterial ();

		Vector3 p0 = new Vector3 (xMin, yMin, 0);
		Vector3 p1 = new Vector3 (xMin + w, yMin, 0);
		Vector3 p2 = new Vector3 (xMin + w, yMin + h, 0);
		Vector3 p3 = new Vector3 (xMin, yMin + h, 0);
		
		GL.PushMatrix ();
		GL.LoadIdentity ();
		lineMaterial.SetPass (0);
		
		GL.Begin (GL.LINES);
		GL.Color (color);
		
		GL.Vertex (p0);
		GL.Vertex (p1);
		
		GL.Vertex (p1);
		GL.Vertex (p2);
		
		GL.Vertex (p2);
		GL.Vertex (p3);
		
		GL.Vertex (p3);
		GL.Vertex (p0);
		
		GL.End ();
		GL.PopMatrix ();
	}
}
