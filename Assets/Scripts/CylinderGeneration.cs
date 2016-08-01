using UnityEngine;
using System.Collections;

public class CylinderGeneration : TunnelGeneration {

	public float cylinderHeight;
	public Vector3 cylinderOrigin;

	public override MeshBuilder AddToMeshBuilder (MeshBuilder meshBuilder = null) {

		if (meshBuilder == null)
		{
			meshBuilder = new MeshBuilder();
		}

		// Generating circles
		Vector3 circleOrigin = cylinderOrigin;
		circleOrigin.y += cylinderHeight / 2f;

		CircleGeneration circleGeneration = new CircleGeneration()
		{
			curveVerticesNumber = curveVerticesNumber,
			radius = radius,
			meshFace = MeshFace.Both,
			circleOrigin = circleOrigin
		};
		meshBuilder = circleGeneration.AddToMeshBuilder(meshBuilder);

		circleOrigin.y -= cylinderHeight;
		circleGeneration.circleOrigin = circleOrigin;
		meshBuilder = circleGeneration.AddToMeshBuilder(meshBuilder);

		tunnelHeight = cylinderHeight;
		tunnelOrigin = cylinderOrigin;

		// Generating Tunnel
		return base.AddToMeshBuilder(meshBuilder);
	}

}
