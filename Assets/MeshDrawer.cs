using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshDrawer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// http://qiita.com/2dgames_jp/items/231a18454348cfebd49d
		
		mesh = new Mesh();
	}

	private Mesh mesh;
	public Material material;

	private List<Vector3> vertices;
	private List<int> indices;
	private List<Color> colors;

//	private List<MeshTopology> meshTopologies;

	int subMeshCount = 0;
	int indexCount = 0;


	public void Init()
	{
		vertices = new List<Vector3>();
		indices = new List<int>();
		colors = new List<Color>();
//		meshTopologies = new List<MeshTopology>();
		indexCount = 0;
	}
	public void Render()
	{
		// render
		mesh.Clear();

		mesh.vertices = vertices.ToArray();
		mesh.colors = colors.ToArray();
		int[] ids = indices.ToArray();
		mesh.SetIndices(ids, MeshTopology.Lines, 0);
		Matrix4x4 mtx = this.transform.localToWorldMatrix;
		Graphics.DrawMesh(mesh, mtx, material, 0, Camera.main, 0);
	}
	//
	public void DrawLine (float x0, float y0, float x1, float y1, Color color)
	{
		vertices.Add(new Vector3 (x0, y0, 0));
		vertices.Add(new Vector3 (x1, y1, 0));
		
		indices.Add(indexCount); indexCount++;
		indices.Add(indexCount); indexCount++;

		colors.Add(color);
		colors.Add(color);
	}
	public void DrawLine (Vector3 p0, Vector3 p1, Color color)
	{
		vertices.Add(p0);
		vertices.Add(p1);
		
		indices.Add(indexCount); indexCount++;
		indices.Add(indexCount); indexCount++;
		
		colors.Add(color);
		colors.Add(color);
	}
	
	public void DrawRect (float xMin, float yMin, float w, float h, Color color)
	{
		Vector3 p0 = new Vector3 (xMin, yMin, 0);
		Vector3 p1 = new Vector3 (xMin + w, yMin, 0);
		Vector3 p2 = new Vector3 (xMin + w, yMin + h, 0);
		Vector3 p3 = new Vector3 (xMin, yMin + h, 0);

		vertices.Add(p0);
		vertices.Add(p1);
		vertices.Add(p2);
		vertices.Add(p3);

		int startIndex = indexCount;
		indices.Add(startIndex); indexCount++;
		indices.Add(indexCount); 
		indices.Add(indexCount); indexCount++;
		indices.Add(indexCount); 
		indices.Add(indexCount); indexCount++;
		indices.Add(indexCount); 
		indices.Add(indexCount); indexCount++;
		indices.Add(startIndex); 

		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	public void DrawRect (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color)
	{
//		Vector3 p0 = new Vector3 (xMin, yMin, 0);
//		Vector3 p1 = new Vector3 (xMin + w, yMin, 0);
//		Vector3 p2 = new Vector3 (xMin + w, yMin + h, 0);
//		Vector3 p3 = new Vector3 (xMin, yMin + h, 0);
		
		vertices.Add(p0);
		vertices.Add(p1);
		vertices.Add(p2);
		vertices.Add(p3);
		
		int startIndex = indexCount;
		indices.Add(startIndex); indexCount++;
		indices.Add(indexCount); 
		indices.Add(indexCount); indexCount++;
		indices.Add(indexCount); 
		indices.Add(indexCount); indexCount++;
		indices.Add(indexCount); 
		indices.Add(indexCount); indexCount++;
		indices.Add(startIndex); 
		
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

}
