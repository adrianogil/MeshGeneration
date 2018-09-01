using UnityEngine;
using System.Collections.Generic;

public abstract class MeshSimplificationAlgorithm
{
    protected Mesh m_mesh;
    protected List<Vector3> vertices;
    protected List<Vector3> normals;
    protected List<int> verticesIndex;
    protected List<int> triangles;
    protected int m_targetVerticesNumber;

    protected virtual Mesh ProcessMesh(Mesh mesh) { return mesh; }

    public Mesh SimplifyMesh(Mesh mesh, int target)
    {
        m_mesh = mesh;
        m_targetVerticesNumber = target;

        vertices = new List<Vector3>(mesh.vertices);
        normals = new List<Vector3>(mesh.normals);
        triangles = new List<int>(mesh.triangles);
        verticesIndex = new List<int>();

        for (int i = 0; i < vertices.Count; i++)
        {
            verticesIndex.Add(i);
        }

        return ProcessMesh(mesh);
    }

    /// <summary>
    /// HalfEdgeCollapse(int v1, int v2)
    ///     - Removes v1 and update triangles from v1 to v2
    /// </summary>
    public void HalfEdgeCollapse(int v1, int v2)
    {
        // Debug.Log("GilLog - MeshSimplificationAlgorithm::HalfEdgeCollapse - v1 " + v1 + " v2 " + v2 + " ");

        List<int> newTriangles = new List<int>();

        int t1, t2, t3;

        bool v1Present = false;
        bool v2Present = false;

        for (int t = 0; t < triangles.Count/3; t++)
        {
            t1 = triangles[3*t];
            t2 = triangles[3*t+1];
            t3 = triangles[3*t+2];

            v1Present = (t1 == v1 || t2 == v1 || t3 == v1);
            v2Present = (t1 == v2 || t2 == v2 || t3 == v2);

            if (v1Present && v2Present)
            {
                Debug.Log("Removed triangle " + t);
            } else if (v1Present)
            {
                if (t1 == v1)
                {
                    t1 = v2;
                } else if (t2 == v1)
                {
                    t2 = v2;
                } else {
                    t3 = v2;
                }

                newTriangles.Add(t1);
                newTriangles.Add(t2);
                newTriangles.Add(t3);
            } else {
                newTriangles.Add(t1);
                newTriangles.Add(t2);
                newTriangles.Add(t3);
            }
        }

        triangles = newTriangles;
        verticesIndex.Remove(v1);
    }
}