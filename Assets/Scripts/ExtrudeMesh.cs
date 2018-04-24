using UnityEngine;
using System.Collections.Generic;

public class ExtrudeMesh : MonoBehaviour
{
    private static bool IsEdgeInTriangles(int[] triangles, int v1, int v2)
    {
        for (int i = 0; i < triangles.Length; i=i+3)
        {
            if ((triangles[i] == v1 && (triangles[i+1] == v2 || triangles[i+2] == v2)) ||
                (triangles[i+1] == v1 && (triangles[i+2] == v2 || triangles[i] == v2)) ||
                (triangles[i+2] == v1 && (triangles[i] == v2 || triangles[i+1] == v2))
                )
            {
                return true;
            }
        }

        return false;
    }


    public static MeshBuilder From(MeshBuilder baseMesh, Vector3 direction)
    {
        MeshBuilder extrudedMesh = new MeshBuilder();

        List<Vector3> baseVertices = baseMesh.Vertices;

        int[] triangles = baseMesh.GetTriangles();

        for (int i = 0; i < baseVertices.Count; i++)
        {
            extrudedMesh.Vertices.Add(baseVertices[i]);
        }

        for (int i = 0; i < baseVertices.Count; i++)
        {
            extrudedMesh.Vertices.Add(baseVertices[i] + direction);
        }

        int v1, v2;

        for (int i = 0; i < baseVertices.Count; i++)
        {
            v1 = i;
            v2 = (i+1) % baseVertices.Count;

            if (IsEdgeInTriangles(triangles, v1, v2))
            {
                extrudedMesh.AddTriangle(baseVertices.Count+v1, baseVertices.Count+v2, v1);
                extrudedMesh.AddTriangle(baseVertices.Count+v2, v2, v1);
                extrudedMesh.AddTriangle(baseVertices.Count+v1, v1, baseVertices.Count+v2);
                extrudedMesh.AddTriangle(baseVertices.Count+v2, v1, v2);
            }
        }

        return extrudedMesh;
    }

}