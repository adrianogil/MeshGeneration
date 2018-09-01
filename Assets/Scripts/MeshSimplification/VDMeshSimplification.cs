using UnityEngine;
using System.Collections.Generic;

public class VDMeshSimplification : MeshSimplificationAlgorithm
{
    protected float VertexDistanceToAveragePlane(int v)
    {
        // Debug.Log("GilLog - VDMeshSimplification::VertexDistanceToAveragePlane - v " + v + " ");

        List<int> nearTriangles = new List<int>();

        for (int t = 0; t < triangles.Count; t++)
        {
            if (triangles[t] == v)
                nearTriangles.Add(t/3);
        }

        Vector3 x = Vector3.zero;
        float sumArea = 0f;
        float area = 0f;

        Vector3 v1, v2, v3, n1, n2, n3;

        Vector3 s = Vector3.zero;

        int nT = -1, t1, t2, t3;

        for (int n = 0; n < nearTriangles.Count; n++)
        {
            nT = nearTriangles[n];
            // Debug.Log("GilLog - VDMeshSimplification::VertexDistanceToAveragePlane - v " + v + "  - nearTriangles[n] " + nearTriangles[n] + " ");

            t1 = triangles[3*nT];
            t2 = triangles[3*nT+1];
            t3 = triangles[3*nT+2];

            // Debug.Log("GilLog - VDMeshSimplification::VertexDistanceToAveragePlane - v " + v + "  - t1 " + t1 + "  - t2 " + t2 + "  - t3 " + t3 + " ");

            v1 = vertices[t1];
            v2 = vertices[t2];
            v3 = vertices[t3];

            n1 = normals[t1];
            n2 = normals[t2];
            n3 = normals[t3];

            area = (0.5f * Vector3.Cross(v1-v2, v3-v2).magnitude);

            x = x + area * (v1 + v2 + v3) / 3f;
            s = s + area * (n1 + n2 + n3) / 3f;
            sumArea = sumArea + area;
        }

        x = (1f / sumArea) * x;
        s = (1f / sumArea) * s;

        return Mathf.Abs(Vector3.Dot(s.normalized, vertices[v] - x));
         // - 0.1f*(nearTriangles.Count*1f/triangles.Count);
    }

    protected override Mesh ProcessMesh(Mesh mesh) {

        int randomTriangle = -1;
        int verticesInTriangle = -1;
        int indexToRemove = -1;
        int indexToCollapse = -1;

        int currentTriangle = -1;
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;
        float metricValue = -1f;

        while (verticesIndex.Count > m_targetVerticesNumber) {
            // Debug.Log("GilLog - VDMeshSimplification::ProcessMesh - Search vertice");

            minValue = float.MaxValue;
            maxValue = float.MinValue;
            indexToRemove = -1;

            for (int v = 0; v < verticesIndex.Count; v++)
            {
                metricValue = VertexDistanceToAveragePlane(verticesIndex[v]);
                // Debug.Log("GilLog - VDMeshSimplification::ProcessMesh  - metricValue " + metricValue + "  - verticesIndex[v] " + verticesIndex[v] + " ");

                if (metricValue < minValue)
                // if (metricValue > maxValue)
                {
                    minValue = metricValue;
                    // maxValue = metricValue;
                    indexToRemove = verticesIndex[v];
                }
            }

            int maxTriangles = 0, totalTriangles = 0;
            int possibleIndex = 0;

            for (int t = 0; t < triangles.Count; t++)
            {
                if (triangles[t] == indexToRemove)
                {
                    currentTriangle = t/3;
                    verticesInTriangle = t % 3;

                    for (int i = 1; i < 3; i++)
                    {
                        possibleIndex = triangles[3*currentTriangle + (verticesInTriangle+i) % 3];
                        totalTriangles = 0;

                        for (int t2 = 0; t2 < triangles.Count; t2++)
                        {
                            if (triangles[t2] == possibleIndex)
                            {
                                totalTriangles++;
                            }
                        }

                        if (totalTriangles > maxTriangles)
                        {
                            maxTriangles = totalTriangles;
                            indexToCollapse = possibleIndex;
                        }
                    }
                }
            }

            HalfEdgeCollapse(indexToRemove, indexToCollapse);
        }

        Vector3[] newVertices = new Vector3[verticesIndex.Count];
        int[] newTriangles = new int[triangles.Count];

        for (int v = 0; v < verticesIndex.Count; v++)
        {
            newVertices[v] = vertices[verticesIndex[v]];

            for (int t = 0; t < triangles.Count; t++)
            {
                if (triangles[t] == verticesIndex[v])
                    newTriangles[t] = v;
            }
        }

        mesh.triangles = newTriangles;
        mesh.vertices = newVertices;

        return mesh;
    }
}