using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PumpkinGeneration : MeshGeneratorBehaviour
{
    public float radius;

    public Vector3 pumpkinScale;

    public int nbLong = 10;
    public int nbLat = 10;

    protected override Mesh GenerateMesh()
    {
        SphereMesh sphere = new SphereMesh();
        sphere.radius = radius;

        MeshBuilder pumpkinMeshBuilder =  sphere.Create();
        pumpkinMeshBuilder.ScaleVertices(pumpkinScale);

        return pumpkinMeshBuilder.CreateMesh();
    } 
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(PumpkinGeneration))]
public class PumpkinGenerationEditor : MeshGeneratorEditor {

}
#endif
