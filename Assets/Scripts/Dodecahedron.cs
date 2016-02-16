using UnityEngine;

using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Dodecahedron : MonoBehaviour {

	public List<Vector3> vertices = new List<Vector3>();

	// Use this for initialization
	void Start () {
		MeshBuilder meshBuilder = new MeshBuilder();

		meshBuilder.Vertices.Add(new Vector3(-1f,-1f,-1f)); // 0
		meshBuilder.Vertices.Add(new Vector3(-1f,-1f, 1f)); // 1
		meshBuilder.Vertices.Add(new Vector3(-1f, 1f,-1f)); // 2
		meshBuilder.Vertices.Add(new Vector3(-1f, 1f, 1f)); // 3
		meshBuilder.Vertices.Add(new Vector3( 1f,-1f,-1f)); // 4
		meshBuilder.Vertices.Add(new Vector3( 1f,-1f, 1f)); // 5
		meshBuilder.Vertices.Add(new Vector3( 1f, 1f,-1f)); // 6
		meshBuilder.Vertices.Add(new Vector3( 1f, 1f, 1f)); // 7

		float phi = (1f + Mathf.Sqrt(5f))/2f;

		meshBuilder.Vertices.Add(new Vector3(0f, -1f/phi, -phi)); // 8
		meshBuilder.Vertices.Add(new Vector3(0f, -1f/phi,  phi)); // 9
		meshBuilder.Vertices.Add(new Vector3(0f,  1f/phi, -phi)); // 10
		meshBuilder.Vertices.Add(new Vector3(0f,  1f/phi,  phi)); // 11


		meshBuilder.Vertices.Add(new Vector3(-1f/phi, -phi, 0f)); // 12
		meshBuilder.Vertices.Add(new Vector3(-1f/phi,  phi, 0f)); // 13
		meshBuilder.Vertices.Add(new Vector3( 1f/phi, -phi, 0f)); // 14
		meshBuilder.Vertices.Add(new Vector3( 1f/phi,  phi, 0f)); // 15

		meshBuilder.Vertices.Add(new Vector3(-phi, 0f, -1f/phi)); // 16
		meshBuilder.Vertices.Add(new Vector3( phi, 0f, -1f/phi)); // 17
		meshBuilder.Vertices.Add(new Vector3(-phi, 0f,  1f/phi)); // 18
		meshBuilder.Vertices.Add(new Vector3( phi, 0f,  1f/phi)); // 19

		vertices = meshBuilder.Vertices;

		AddDodecahedronFace(meshBuilder, 7, 19, 17, 6, 15);
		AddDodecahedronFace(meshBuilder, 6, 17, 4, 8, 10);
		AddDodecahedronFace(meshBuilder, 15, 6, 10, 2, 13);
		AddDodecahedronFace(meshBuilder, 2, 10, 8, 0, 16);
		AddDodecahedronFace(meshBuilder, 13, 2, 16, 18, 3);
		AddDodecahedronFace(meshBuilder, 7, 15, 13, 3, 11);
		AddDodecahedronFace(meshBuilder, 4, 17, 19, 5, 14);
		AddDodecahedronFace(meshBuilder, 0, 8, 4, 14, 12);
		AddDodecahedronFace(meshBuilder, 18, 16, 0, 12, 1);
		AddDodecahedronFace(meshBuilder, 11, 3, 18, 1, 9);
		AddDodecahedronFace(meshBuilder, 19, 7, 11, 9, 5);
		AddDodecahedronFace(meshBuilder, 5, 9, 1, 12, 14);

		AddInternalCubeFace(meshBuilder, 2, 6, 4, 0);
		AddInternalCubeFace(meshBuilder, 6, 7, 5, 4);
		AddInternalCubeFace(meshBuilder, 7, 3, 1, 5);
		AddInternalCubeFace(meshBuilder, 3, 2, 0, 1);
		AddInternalCubeFace(meshBuilder, 3, 7, 6, 2);
		AddInternalCubeFace(meshBuilder, 5, 1, 0, 4);

		Mesh mesh = GetComponent<MeshFilter> ().mesh = meshBuilder.CreateMesh ();

		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
	}
	
	void AddDodecahedronFace(MeshBuilder meshBuilder, int v1, int v2, int v3, int v4, int v5)
	{
		meshBuilder.AddTriangle(v1, v4, v5);
		meshBuilder.AddTriangle(v2, v4, v1);
		meshBuilder.AddTriangle(v3, v4, v2);
	}

	void AddInternalCubeFace(MeshBuilder meshBuilder, int v1, int v2, int v3, int v4)
	{
		meshBuilder.AddTriangle(v4, v3, v1);
		meshBuilder.AddTriangle(v3, v2, v1);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		foreach (Vector3 vertice in vertices) {
			Gizmos.DrawSphere(vertice, 0.1f);
		}
	}
}
