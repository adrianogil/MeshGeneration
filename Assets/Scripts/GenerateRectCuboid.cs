using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateRectCuboid : MonoBehaviour
{
    public Vector3 size = Vector3.one;

    [ContextMenu("Generate")]
    public void Generate()
    {
        var meshBuilder = RectCuboidMesh.Create(size:size);

        Mesh mesh = GetComponent<MeshFilter> ().mesh = meshBuilder.CreateMesh ();

        mesh.RecalculateBounds ();
        mesh.RecalculateNormals ();
    }
}
