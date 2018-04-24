using UnityEngine;

public class MeshProcessing
{
    public static void SubdivideTriangles(MeshBuilder meshBuilder)
    {
        int[] triangles = meshBuilder.GetTriangles();

        meshBuilder.ClearTriangles();

        int totalTriangles = triangles.Length / 3;

        Vector3 p1,p2,p3,p4;

        int t1,t2,t3,t4 = 0;

        for (int i = 0; i < totalTriangles; i++)
        {
            t1 = triangles[3*i+0];
            t2 = triangles[3*i+1];
            t3 = triangles[3*i+2];

            p1 = meshBuilder.Vertices[t1];
            p2 = meshBuilder.Vertices[t2];
            p3 = meshBuilder.Vertices[t3];

            p4 = (p1+p2+p3) / 3f;

            t4 = meshBuilder.Vertices.Count;
            meshBuilder.Vertices.Add(p4);

            meshBuilder.AddTriangle(t1,t2,t4);
            meshBuilder.AddTriangle(t4,t2,t3);
            meshBuilder.AddTriangle(t4,t3,t1);
        }

    }

    public static void SubdivideTrianglesIn4(MeshBuilder meshBuilder)
    {
        int[] triangles = meshBuilder.GetTriangles();

        meshBuilder.ClearTriangles();

        int totalTriangles = triangles.Length / 3;

        Vector3 p1,p2,p3,p4,p5,p6;

        int t1,t2,t3,t4,t5,t6 = 0;

        for (int i = 0; i < totalTriangles; i++)
        {
            t1 = triangles[3*i+0];
            t2 = triangles[3*i+1];
            t3 = triangles[3*i+2];

            p1 = meshBuilder.Vertices[t1];
            p2 = meshBuilder.Vertices[t2];
            p3 = meshBuilder.Vertices[t3];

            p4 = (p1+p2) / 2f;
            p5 = (p2+p3) / 2f;
            p6 = (p3+p1) / 2f;

            t4 = meshBuilder.Vertices.Count;
            meshBuilder.Vertices.Add(p4);

            t5 = meshBuilder.Vertices.Count;
            meshBuilder.Vertices.Add(p5);

            t6 = meshBuilder.Vertices.Count;
            meshBuilder.Vertices.Add(p6);

            meshBuilder.AddTriangle(t1,t4,t6);
            meshBuilder.AddTriangle(t4,t2,t5);
            meshBuilder.AddTriangle(t4,t5,t6);
            meshBuilder.AddTriangle(t6,t5,t3);
        }

    }
}