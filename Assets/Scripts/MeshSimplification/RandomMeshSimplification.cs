using UnityEngine;

public class RandomMeshSimplification : MeshSimplificationAlgorithm
{
    protected override Mesh ProcessMesh(Mesh mesh) {

        int randomTriangle = -1;
        int verticesInTriangle = -1;
        int indexToRemove = -1;
        int indexToCollapse = -1;

        while (verticesIndex.Count > m_targetVerticesNumber) {
            randomTriangle = Random.Range(0, triangles.Count/3);

            verticesInTriangle = Random.Range(0, 3);
            indexToRemove = 3*randomTriangle + verticesInTriangle;
            indexToCollapse = 3*randomTriangle + (verticesInTriangle + 1) % 3;

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