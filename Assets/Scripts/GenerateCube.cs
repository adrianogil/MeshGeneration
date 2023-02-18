using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateCube : MonoBehaviour
{
    public float cubeSize = 1f;

    [ContextMenu("Generate")]
    public void Generate()
    {
        var meshBuilder = CubeMesh.Create(size:cubeSize);

        Mesh mesh = GetComponent<MeshFilter> ().mesh = meshBuilder.CreateMesh ();

        mesh.RecalculateBounds ();
        mesh.RecalculateNormals ();
    }
}
