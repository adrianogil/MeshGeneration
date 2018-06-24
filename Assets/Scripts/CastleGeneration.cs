using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CastleGeneration : MeshGeneratorBehaviour
{

    public float height;

    protected override Mesh GenerateMesh()
    {
        // CylinderGeneration cylinderGeneration = new CylinderGeneration();
        // cylinderGeneration.cylinderHeight = 3f;
        // cylinderGeneration.cylinderOrigin = Vector3.zero;

        // MeshBuilder meshBuilder = new MeshBuilder();

        // cylinderGeneration.AddToMeshBuilder(meshBuilder);
        
        CylinderMesh cylinder = new CylinderMesh();
        cylinder.radius = 0.8f;
        cylinder.direction1 = new Vector3(1f, 0f, 0f);
        cylinder.direction2 = new Vector3(0f, 0f, 1f);
        cylinder.totalPerimeterVertices = 30;
        cylinder.height = height;

        MeshBuilder meshBuilder = cylinder.Create();

        return meshBuilder.CreateMesh();
    } 
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(CastleGeneration))]
public class CastleGenerationEditor : MeshGeneratorEditor {

}
#endif