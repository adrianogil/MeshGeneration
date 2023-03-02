using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateCube : MonoBehaviour
{
    public float cubeSize = 1f;

    [ContextMenu("Generate")]
    public void Generate()
    {
        var meshBuilder = CubeMesh.Create(size:cubeSize);

        Mesh mesh = GetComponent<MeshFilter> ().mesh = meshBuilder.CreateMesh ();

        mesh.RecalculateBounds ();
        mesh.RecalculateNormals ();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GenerateCube))]
public class GenerateCubeEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    
        GenerateCube editorObj = target as GenerateCube;
    
        if (editorObj == null) return;
        
        if (GUILayout.Button("Generate"))
        {
            editorObj.Generate();
        }
    }

}
#endif