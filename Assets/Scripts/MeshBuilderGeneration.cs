using UnityEngine;
using System.Collections;

public enum MeshFace
{
	Front,
	Back,
	Both
}

public abstract class MeshBuilderGeneration {

	public MeshFace meshFace;

	public abstract MeshBuilder AddToMeshBuilder (MeshBuilder meshBuilder = null);

	protected void AddQuadTriangles(MeshBuilder meshBuilder, int index0, int index1, int index2, int index3)
	{
		if (meshFace == MeshFace.Front || meshFace == MeshFace.Both) {
			meshBuilder.AddTriangle(index0, index1, index2);
			meshBuilder.AddTriangle(index1, index3, index2);
		}
		if(meshFace == MeshFace.Back || meshFace == MeshFace.Both){
			meshBuilder.AddTriangle(index1, index0, index2);
			meshBuilder.AddTriangle(index1, index2, index3);
		}
	}
	
}
