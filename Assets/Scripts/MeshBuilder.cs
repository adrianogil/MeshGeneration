using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshBuilder {

	private List<Vector3> m_Vertices = new List<Vector3>();
	public List<Vector3> Vertices { get { return m_Vertices; }}

	private List<Vector3> m_Normals = new List<Vector3>();
	public List<Vector3> Normals { get { return m_Normals; }}

	private List<Vector2> m_UVs = new List<Vector2>();
	public List<Vector2> UVs { get { return m_UVs; }}

	private List<Vector4> m_Tangents = new List<Vector4>();
	public List<Vector4> Tangents { get { return m_Tangents; }}

	private List<int> m_Indices = new List<int>();

	private Dictionary<string, List<Vector3> > vertDictionary = new Dictionary<string, List<Vector3> >();

	public int[] GetTriangles()
	{
		return m_Indices.ToArray();
	}

	public void ClearTriangles()
	{
		m_Indices.Clear();
	}

	public void AddTriangle(int index0, int index1, int index2)
	{
		m_Indices.Add (index0);
		m_Indices.Add (index1);
		m_Indices.Add (index2);
	}

	public void AddVertice(Vector3 vertice, string verticeCategory="default")
	{
		m_Vertices.Add(vertice);

		if (vertDictionary.ContainsKey(verticeCategory))
		{
			vertDictionary[verticeCategory].Add(vertice);
		} else {
			vertDictionary.Add(verticeCategory, new List<Vector3>());
			vertDictionary[verticeCategory].Add(vertice);
		}
	}

	public List<Vector3> GetVertices(string verticeCategory)
	{
		if (!vertDictionary.ContainsKey(verticeCategory))
			vertDictionary.Add(verticeCategory, new List<Vector3>());

		return vertDictionary[verticeCategory];
	}

	public MeshBuilder ScaleVertices(Vector3 v)
	{
		List<Vector3> newVertices = new List<Vector3>();

		for (int i = 0; i < Vertices.Count; i++)
		{
			newVertices.Add(Vector3.Scale(Vertices[i],v));
		}

		m_Vertices = newVertices;

		Dictionary<string, List<Vector3> > newVertDictionary = new Dictionary<string, List<Vector3> >();
		foreach (var pair in vertDictionary)
        {
            if (pair.Value != null)
            {
            	newVertices = new List<Vector3>();
            	for (int i = 0; i < pair.Value.Count; i++)
            	{
            		newVertices.Add(Vector3.Scale(pair.Value[i],v));
            	}
            	newVertDictionary.Add(pair.Key, newVertices);
            }
        }
        vertDictionary = newVertDictionary;

		return this; // Fluid interface
	}

	public MeshBuilder ScaleVertices(float v)
	{
		List<Vector3> newVertices = new List<Vector3>();

		for (int i = 0; i < Vertices.Count; i++)
		{
			newVertices.Add(Vertices[i] * v);
		}

		m_Vertices = newVertices;

		Dictionary<string, List<Vector3> > newVertDictionary = new Dictionary<string, List<Vector3> >();
		foreach (var pair in vertDictionary)
        {
            if (pair.Value != null)
            {
            	newVertices = new List<Vector3>();
            	for (int i = 0; i < pair.Value.Count; i++)
            	{
            		newVertices.Add(pair.Value[i] * v);
            	}
            	newVertDictionary.Add(pair.Key, newVertices);
            }
        }
        vertDictionary = newVertDictionary;

		return this; // Fluid interface
	}

	public MeshBuilder Translate(Vector3 v)
	{
		List<Vector3> newVertices = new List<Vector3>();

		for (int i = 0; i < Vertices.Count; i++)
		{
			newVertices.Add(Vertices[i] + v);
		}

		m_Vertices = newVertices;

		Dictionary<string, List<Vector3> > newVertDictionary = new Dictionary<string, List<Vector3> >();
		foreach (var pair in vertDictionary)
        {
            if (pair.Value != null)
            {
            	newVertices = new List<Vector3>();
            	for (int i = 0; i < pair.Value.Count; i++)
            	{
            		newVertices.Add(pair.Value[i] + v);
            	}
            	newVertDictionary.Add(pair.Key, newVertices);
            }
        }
        vertDictionary = newVertDictionary;

		return this; // Fluid interface
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh ();

		mesh.vertices = m_Vertices.ToArray ();
		mesh.triangles = m_Indices.ToArray ();



		// UVs sao opcionais
		if (m_UVs.Count == m_Vertices.Count) {
			mesh.uv = m_UVs.ToArray();
		}

		// Tangents sao opcionais
		if (m_Tangents.Count == m_Vertices.Count) {
			mesh.tangents = m_Tangents.ToArray();
		}


		// Normals sao opcionais
		if (m_Normals.Count == m_Vertices.Count) {
			mesh.normals = m_Normals.ToArray();
		} else {
			mesh.RecalculateNormals ();
		}
		mesh.RecalculateBounds ();

		return mesh;
	}

	public MeshBuilder Join(MeshBuilder b)
	{
		return MeshBuilder.Join(this, b);
	}

	public static MeshBuilder Join(MeshBuilder b1, MeshBuilder b2)
	{
		MeshBuilder builder = new MeshBuilder();

		builder.Vertices.AddRange(b1.Vertices);
		builder.Vertices.AddRange(b2.Vertices);

		builder.Normals.AddRange(b1.Normals);
		builder.Normals.AddRange(b2.Normals);

		builder.UVs.AddRange(b1.UVs);
		builder.UVs.AddRange(b2.UVs);

		builder.Tangents.AddRange(b1.Tangents);
		builder.Tangents.AddRange(b2.Tangents);

		int[] triangles1 = b1.GetTriangles();
		int[] triangles2 = b2.GetTriangles();

		for (int i = 0; i < triangles1.Length; i++)
		{
			builder.m_Indices.Add(triangles1[i]);
		}

		for (int i = 0; i < triangles2.Length; i++)
		{
			builder.m_Indices.Add(triangles2[i] + b1.Vertices.Count);
		}

		return builder;
	}

}
