using UnityEngine;

public class QuadMesh : MonoBehaviour
{
    public static void Create(MeshBuilder builder, Vector3 direction1, Vector3 direction2)
    {
        int initialIndex = builder.Vertices.Count;

        builder.Vertices.Add(Vector3.zero);
        builder.Vertices.Add(direction1);
        builder.Vertices.Add(direction1 + direction2);
        builder.Vertices.Add(direction2);

        builder.AddTriangle(initialIndex+0, initialIndex+1, initialIndex+3);
        builder.AddTriangle(initialIndex+1, initialIndex+3, initialIndex+2);
        builder.AddTriangle(initialIndex+0, initialIndex+3, initialIndex+1);
        builder.AddTriangle(initialIndex+1, initialIndex+2, initialIndex+3);
    }
}