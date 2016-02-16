using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshPlayground : MonoBehaviour {

	public float meshLength, meshWidth, meshHeight;

	public int sizeX, sizeZ;

	public float animationDuration;

	private Vector3[] cubeVertices;
	private Vector3[] sphereVertices;
	private Vector3[] vertices;

	private float progress;

	private Mesh mesh;

	void PrepareForVerticesAnimation() {
		mesh = GetComponent<MeshFilter> ().mesh;
		mesh.MarkDynamic ();

		cubeVertices = mesh.vertices;

		float radius = 2 * meshLength;

		sphereVertices = new Vector3[cubeVertices.Length];

		for (int i = 0; i < cubeVertices.Length; i++) {
			sphereVertices[i] = cubeVertices[i].normalized * radius;
		}

		vertices = mesh.vertices;
	}

	void Update() {
		progress += Time.deltaTime / animationDuration;

		if (progress >= 1f) {
			progress -= 1f;
		}

		// Animacao de Vertices
		for (int i = 0; i < vertices.Length; i++) {
			vertices[i] = cubeVertices[i] + progress * (sphereVertices[i] - cubeVertices[i]);
		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
	}

	// Use this for initialization
	void Start () {
		GenerateCubeMesh ();
	}

	void GenerateQuadMesh() {
		MeshBuilder meshBuilder = new MeshBuilder ();

		float vx, vz;
		Vector3 offset;

		for (int z = 0; z < sizeZ; z++) {
			vz = meshLength * z;

			for (int x = 0; x < sizeX; x++) {
				vx = meshWidth * x;

				offset = new Vector3(vx, Random.Range (0f, meshHeight), vz);

				BuildQuad(meshBuilder, offset);
			}
		}

		GetComponent<MeshFilter> ().mesh = meshBuilder.CreateMesh ();
	}

	void BuildQuad(MeshBuilder meshBuilder, Vector3 offset) {


		meshBuilder.Vertices.Add (new Vector3(0f, 0f, 0f) + offset);
		meshBuilder.UVs.Add (new Vector2(0f, 0f));
		meshBuilder.Normals.Add (Vector3.up);

		meshBuilder.Vertices.Add (new Vector3(0f, 0f, meshLength) + offset);
		meshBuilder.UVs.Add (new Vector2(0f, 1f));
		meshBuilder.Normals.Add (Vector3.up);

		meshBuilder.Vertices.Add (new Vector3(meshWidth, 0f, meshLength) + offset);
		meshBuilder.UVs.Add (new Vector2(1f, 1f));
		meshBuilder.Normals.Add (Vector3.up);

		meshBuilder.Vertices.Add (new Vector3(meshWidth, 0f, 0f) + offset);
		meshBuilder.UVs.Add (new Vector2(1f, 0f));
		meshBuilder.Normals.Add (Vector3.up);

		int baseIndex = meshBuilder.Vertices.Count - 4;

		meshBuilder.AddTriangle (baseIndex, baseIndex+1, baseIndex+2);
		meshBuilder.AddTriangle (baseIndex, baseIndex+2, baseIndex+3);
	}

	void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir, int sizeX, int sizeY) {

		Vector3 normal = Vector3.Cross (lengthDir, widthDir).normalized;

		float vx, vy;

		// Define vertices e triangulos
		for (int y = 0; y <= sizeY; y++) {
			for (int x = 0; x <= sizeX; x++) {
				vx = ((float) x / sizeX);
				vy = ((float) y / sizeY);

				meshBuilder.Vertices.Add (offset + vx * lengthDir + vy * widthDir);
				meshBuilder.UVs.Add (new Vector2(vx, vy));
				meshBuilder.Normals.Add (normal);
			}
		}
		
		int baseIndex = meshBuilder.Vertices.Count - (sizeX+1) * (sizeY+1);

		for (int vi = baseIndex, y = 0; y < sizeY; y++, vi++) {
			for (int x = 0; x < sizeX; x++, vi++) {
				meshBuilder.AddTriangle (vi, vi+1, vi+sizeX+2);
				meshBuilder.AddTriangle (vi, vi+sizeX+2, vi+sizeX+1);
			}
		}

	}

	void GenerateCubeMesh() {
		MeshBuilder meshBuilder = new MeshBuilder ();

		Vector3 upDir = Vector3.up * meshHeight;
		Vector3 rightDir = Vector3.right * meshWidth;
		Vector3 forwardDir = Vector3.forward * meshLength;

		Vector3 nearCorner = - 0.5f * (upDir + rightDir + forwardDir);
		Vector3 farCorner =  nearCorner + upDir + rightDir + forwardDir;

		BuildQuad (meshBuilder, nearCorner, forwardDir, rightDir, sizeX, sizeZ);
		BuildQuad (meshBuilder, nearCorner, rightDir, upDir, sizeX, sizeZ);
		BuildQuad (meshBuilder, nearCorner, upDir, forwardDir, sizeX, sizeZ);

		BuildQuad (meshBuilder, farCorner, -rightDir, -forwardDir, sizeX, sizeZ);
		BuildQuad (meshBuilder, farCorner, -upDir, -rightDir, sizeX, sizeZ);
		BuildQuad (meshBuilder, farCorner, -forwardDir, -upDir, sizeX, sizeZ);

		GetComponent<MeshFilter> ().mesh = meshBuilder.CreateMesh ();

		//Invoke ("Spherify", 3f);

		PrepareForVerticesAnimation ();
	}

	void Spherify() {
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.MarkDynamic ();

		float radius = meshHeight * 2;

		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i < vertices.Length; i++) {
			vertices[i] = vertices[i].normalized * radius;
		}

		mesh.vertices = vertices;

		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();

	}
}
