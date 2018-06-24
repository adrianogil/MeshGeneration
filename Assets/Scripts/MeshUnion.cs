using UnityEngine;

public class MeshUnion
{
    public float size;
    public int totalSegments = 1;

    public MeshBuilder Create(MeshBuilder mb1, MeshBuilder mb2, bool round = true)
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        if (mb1.GetVertices("border").Count == mb2.GetVertices("border").Count)
        {
            Debug.Log("GilLog - MeshUnion::Create - round " + round + " 1 ");

            int totalVertices = mb1.GetVertices("border").Count;

            float segmentSize = size / totalSegments;

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
        }

        return meshBuilder;
    }
}