using UnityEngine;
using System.Collections;

public class CircleGeneration : MeshBuilderGeneration {

	// How much vertices will be used on curves
	public int curveVerticesNumber;

	public float radius = -1f;
	public Vector2 ovalRadius;

	public Vector2 frequency;

	public Vector3 circleOrigin;

	protected bool addOriginVertice = true;

	public CircleGeneration()
	{
		frequency = new Vector2(1f,1f);
		meshFace = MeshFace.Front;
		circleOrigin = Vector3.zero;
	}

	// Use this for initialization
	public override MeshBuilder AddToMeshBuilder (MeshBuilder meshBuilder = null) {
	
		if (meshBuilder == null)
		{
			meshBuilder = new MeshBuilder();
		}

		Vector3 point;
		float radiusAmount;

		int originIndex = meshBuilder.Vertices.Count;

		if (addOriginVertice) {
			meshBuilder.Vertices.Add(circleOrigin);
		}

		for (int i = 0; i < curveVerticesNumber; i++)
		{
			radiusAmount = ((float)i / curveVerticesNumber) * (2*Mathf.PI);
			if (radius == -1f) {
				point = new Vector3(ovalRadius.x * Mathf.Cos (frequency.x * radiusAmount), 0f, 
								    ovalRadius.y * Mathf.Sin (frequency.y * radiusAmount));
			} else {
				point = new Vector3(radius * Mathf.Cos (frequency.x * radiusAmount), 0f, 
									radius * Mathf.Sin (frequency.y * radiusAmount));
			}

			point = point + circleOrigin;

			meshBuilder.Vertices.Add(point);

			AddTrianglesAtIndex(meshBuilder, originIndex, i);

		}

		return meshBuilder;
	}

	protected virtual void AddTrianglesAtIndex(MeshBuilder meshBuilder, int originIndex, int i)
	{
		if (meshFace == MeshFace.Front || meshFace == MeshFace.Both) {
			meshBuilder.AddTriangle(originIndex, 
									originIndex + i + 1,
									originIndex + (i + 1) % curveVerticesNumber + 1);
		}
		if(meshFace == MeshFace.Back || meshFace == MeshFace.Both){
			meshBuilder.AddTriangle(originIndex, 
									originIndex + (i + 1) % curveVerticesNumber + 1,
									originIndex + i + 1);
		}
	}
	
}
