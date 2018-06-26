using UnityEngine;

public class MeshUnion
{
    public float size;
    public int totalSegments = 1;

    public MeshBuilder Create(MeshBuilder mb1, MeshBuilder mb2, bool round = true)
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        float segmentSize = size / totalSegments;

        if (mb1.GetVertices("border").Count == mb2.GetVertices("border").Count)
        {
            Debug.Log("GilLog - MeshUnion::Create - round " + round + " 1 ");

            int totalVertices = mb1.GetVertices("border").Count;

            Debug.Log("GilLog - MeshUnion::Create - mb1 " + mb1 + " mb2 " + mb2 + " round " + round + "  - totalVertices " + totalVertices + "  - segmentSize " + segmentSize + " ");

            for (int i = 0; i < totalVertices; i++)
            {
                // Debug.Log("GilLog - MeshUnion::Create - mb1 " + mb1 + " mb2 " + mb2 + " round " + round + "  - mb1.GetVertices(border)[i] " + mb1.GetVertices("border")[i] + "  - mb2.GetVertices(border)[i] " + mb2.GetVertices("border")[i] + " ");
                Vector3 diffVector = (mb1.GetVertices("border")[i] - mb2.GetVertices("border")[i]).normalized;

                for (int s = 0; s < totalSegments; s++)
                {
                    meshBuilder.AddVertice(
                        mb1.GetVertices("border")[i] + s * segmentSize * diffVector
                        );

                    if (i < totalVertices - 1 || round)
                    {
                        int nextIndex = (i + 1) % totalVertices;
                        meshBuilder.AddTriangle(
                            i*(totalSegments+1)+s,
                            i*(totalSegments+1)+s+1,
                            nextIndex*(totalSegments+1)+s
                            );
                        meshBuilder.AddTriangle(
                            i*(totalSegments+1)+s+1,
                            nextIndex*(totalSegments+1)+s+1,
                            nextIndex*(totalSegments+1)+s
                            
                            );
                    }
                }

                meshBuilder.AddVertice(mb2.GetVertices("border")[i]);
            }
        } else {
            MeshBuilder tmp = null;

            if (mb1.GetVertices("border").Count > mb2.GetVertices("border").Count)
            {
                tmp = mb1;
                mb1 = mb2;
                mb2 = tmp;
            }

            int totalVertices1 = mb1.GetVertices("border").Count;
            int totalVertices2 = mb2.GetVertices("border").Count;

            int vertices2ForEachV1 = totalVertices2 / totalVertices1;

            int currentVertice2 = 0;

            int v1Index = 0;

            for (int i = 0; i < totalVertices1; i++)
            {
                v1Index = meshBuilder.Vertices.Count;
                meshBuilder.AddVertice(
                            mb1.GetVertices("border")[i]
                            );

                for (int v2 = currentVertice2; (i == totalVertices1-1 && v2 < totalVertices2) || v2 < currentVertice2 + vertices2ForEachV1; v2++)
                {
                    Debug.Log("GilLog - MeshUnion::Create - mb1 " + mb1 + " mb2 " + mb2 + " round " + round + "  - v2 " + v2 + " ");
                    Vector3 diffVector = (mb1.GetVertices("border")[i] - mb2.GetVertices("border")[v2]).normalized;

                    for (int s = 0; s < totalSegments; s++)
                    {
                        if (s > 0)
                        {
                            meshBuilder.AddVertice(
                                mb1.GetVertices("border")[i] + s * segmentSize * diffVector
                            );
                        }

                        if (v2 < totalVertices2 - 1 || round)
                        {
                            int nextIndex = (v2 + 1) % totalVertices2;
                            meshBuilder.AddTriangle(
                                v1Index,
                                v1Index + v2*totalSegments + s + 1,
                                v1Index + v2*totalSegments + s + 2
                                );
                        }
                    }

                    meshBuilder.AddVertice(mb2.GetVertices("border")[v2]);
                }

                currentVertice2 = currentVertice2 + vertices2ForEachV1;
            }
        }

        return meshBuilder;
    }
}