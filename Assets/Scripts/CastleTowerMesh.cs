using UnityEngine;

public class CastleTowerMesh
{
    public float radius, height;

    public MeshBuilder Create()
    {
        CylinderMesh cylinder = new CylinderMesh();

        cylinder.radius = radius;
        cylinder.direction1 = new Vector3(1f, 0f, 0f);
        cylinder.direction2 = new Vector3(0f, 0f, 1f);
        cylinder.totalPerimeterVertices = 30;
        cylinder.height = 0.4f*height;

        MeshBuilder meshBuilder = cylinder.Create();
            meshBuilder.Translate(0.2f*height*Vector3.up);

        cylinder.radius = 0.7f*radius;
        cylinder.height = 0.2f*height;

        meshBuilder = cylinder.Create()
            .Translate(0.5f*height*Vector3.up)
            .Join(meshBuilder);

        ConeMesh cone = new ConeMesh();
        cone.radius = 0.9f*radius;
        cone.height = 0.4f * height;
        cone.totalVerticalSegments = 5;
        MeshBuilder coneMeshBuilder = cone.Create()
                    .Translate((height - 0.5f*cone.height) * Vector3.up);

        meshBuilder = meshBuilder.Join(coneMeshBuilder);

        return meshBuilder;
    }

}