using UnityEngine;

public class CircleMesh : MonoBehaviour
{
    public static MeshBuilder Create(float radius, float angleStart, float angleEnd, int totalVertices = 20)
    {
        MeshBuilder builder = new MeshBuilder();

        float anglesPerVertices = (Mathf.PI/180f) * (angleEnd - angleStart) / totalVertices;

        float radiusAmount = 0f;

        for (int i = 0; i < totalVertices; i++)
        {
            radiusAmount = i * anglesPerVertices + angleStart * (Mathf.PI/180f);
            Vector3 point = new Vector3(radius * Mathf.Cos(radiusAmount), 0f, radius * Mathf.Sin(radiusAmount));
            builder.Vertices.Add(point);
        }

        builder.Vertices.Add(Vector3.zero);

        int v1, v2;

        for (int i = 0; i < totalVertices; i++)
        {
            v1 = i;
            v2 = (i+1) % totalVertices;

            builder.AddTriangle(v1, v2, totalVertices);
            builder.AddTriangle(v2, v1, totalVertices);
        }

        return builder;
    }

    public static MeshBuilder Create(float radius, float angleStart, float angleEnd, Vector3 d1, Vector3 d2, int totalVertices = 20)
    {
        MeshBuilder builder = new MeshBuilder();

        float anglesPerVertices = (Mathf.PI/180f) * (angleEnd - angleStart) / totalVertices;

        float radiusAmount = 0f;

        for (int i = 0; i < totalVertices; i++)
        {
            radiusAmount = i * anglesPerVertices + angleStart * (Mathf.PI/180f);
            Vector3 point = radius * Mathf.Cos(radiusAmount) * d1 + radius * Mathf.Sin(radiusAmount) * d2;
            builder.Vertices.Add(point);
        }

        builder.Vertices.Add(Vector3.zero);

        int v1, v2;

        for (int i = 0; i < totalVertices; i++)
        {
            v1 = i;
            v2 = (i+1) % totalVertices;

            builder.AddTriangle(v1, v2, totalVertices);
            builder.AddTriangle(v2, v1, totalVertices);
        }

        return builder;
    }
}