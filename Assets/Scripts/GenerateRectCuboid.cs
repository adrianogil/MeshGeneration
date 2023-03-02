using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateRectCuboid : MonoBehaviour
{
    public Vector3 size = Vector3.one;

    [ContextMenu("Generate")]
    public void Generate()
    {
        var meshBuilder = RectCuboidMesh.Create(size:size);

        Mesh mesh = GetComponent<MeshFilter> ().mesh = meshBuilder.CreateMesh ();

        mesh.RecalculateBounds ();
        mesh.RecalculateNormals ();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GenerateRectCuboid))]
public class GenerateRectCuboidEditor : Editor {


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    
        GenerateRectCuboid editorObj = target as GenerateRectCuboid;
    
        if (editorObj == null) return;
        
        if (GUILayout.Button("Generate"))
        {
            editorObj.Generate();
        }
    }

}
#endif