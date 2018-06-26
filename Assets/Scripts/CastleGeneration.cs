using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CastleGeneration : MeshGeneratorBehaviour
{

    public float height, radius;

    protected override Mesh GenerateMesh()
    {
        CastleTowerMesh tower = new CastleTowerMesh();
        tower.height = height;
        tower.radius = radius;

        MeshBuilder towerMeshBuilder =  tower.Create();

        return towerMeshBuilder.CreateMesh();
    } 
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(CastleGeneration))]
public class CastleGenerationEditor : MeshGeneratorEditor {

}
#endif