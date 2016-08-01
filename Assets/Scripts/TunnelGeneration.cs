using UnityEngine;
using System.Collections;

public class TunnelGeneration : CircleGeneration {

	public int heightVerticesNumber = 2;

	public float tunnelHeight = 2f;

	public Vector3 tunnelOrigin;

	bool addVertices = true;

	public TunnelGeneration() : base()
	{
		addOriginVertice = false;
	}

	// Use this for initialization
	public override MeshBuilder AddToMeshBuilder (MeshBuilder meshBuilder = null) {
	
		if (meshBuilder == null)
		{
			meshBuilder = new MeshBuilder();
		}

		float heightPerVertice = tunnelHeight / (heightVerticesNumber-1);

		circleOrigin = tunnelOrigin;
		circleOrigin.y += tunnelHeight / 2f; 

		for(int h = 0; h < heightVerticesNumber; h++)
		{
			if (h == heightVerticesNumber -1)
			{
				addVertices = false;
			}

			meshBuilder = base.AddToMeshBuilder(meshBuilder);

			circleOrigin.y -= heightPerVertice;
		}

		return meshBuilder;
	}

	protected override void AddTrianglesAtIndex(MeshBuilder meshBuilder, int originIndex, int i)
	{
		if (!addVertices)
		{
			return;
		}

		AddQuadTriangles(meshBuilder,
						 originIndex + i,
						 originIndex + (i+1) % curveVerticesNumber,
						 originIndex + i + curveVerticesNumber,
						 originIndex + (i+1) % curveVerticesNumber + curveVerticesNumber);
	}
}
