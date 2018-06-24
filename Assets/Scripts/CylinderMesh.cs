using UnityEngine;

public class CylinderMesh
{
    public float radius;
    public Vector3 direction1;
    public Vector3 direction2;
    public float height; 
    public int totalPerimeterVertices = 20;
    public int totalVerticalSegments = 1;

    public MeshBuilder Create() 
    {
        CircleMesh circleMesh = new CircleMesh();
        circleMesh.radius = radius;
        circleMesh.direction1 = direction1;
        circleMesh.direction2 = direction2;
        circleMesh.totalVertices = totalPerimeterVertices;

        Vector3 direction3 = Vector3.Cross(direction1, direction2).normalized;

        MeshBuilder upCircle = circleMesh.Create()
                .Translate(0.5f * height * direction3);

        MeshBuilder downCircle = circleMesh.Create()
                .Translate(-0.5f * height * direction3);

        MeshUnion meshUnion = new MeshUnion();
        meshUnion.size = height;
        meshUnion.totalSegments = totalVerticalSegments;

        MeshBuilder tunnel = meshUnion.Create(upCircle, downCircle);

        return MeshBuilder.Join(upCircle, downCircle).Join(tunnel);
    }
}