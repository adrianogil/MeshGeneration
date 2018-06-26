using UnityEngine;

public class ConeMesh
{
    public float radius = 1f, height = 1f;
    public Vector3 direction1 = Vector3.forward;
    public Vector3 direction2 = Vector3.right;
    public Vector3 direction3 = Vector3.up;
    public int totalPerimeterVertices = 20;

    public MeshBuilder Create()
    {
        CircleMesh circleMesh = new CircleMesh();
        circleMesh.radius = radius;
        circleMesh.direction1 = direction1;
        circleMesh.direction2 = direction2;
        circleMesh.totalVertices = totalPerimeterVertices;

        if ( (direction3 - Vector3.up).magnitude < 0.001f)
        {
            direction3 = Vector3.Cross(direction1, direction2).normalized;
        }

        MeshBuilder baseCircle = circleMesh.Create()
                .Translate(-0.5f * height * direction3);

        MeshBuilder top = new MeshBuilder();
        top.AddVertice(new Vector3(0f,-0.5f*height,0f) + height * direction3, "border");

        MeshUnion meshUnion = new MeshUnion();
        meshUnion.size = height;
        meshUnion.totalSegments = 1;

        MeshBuilder cone = meshUnion.Create(top, baseCircle);

        return cone.Join(baseCircle);
    }
}