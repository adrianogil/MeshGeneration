using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SimplificationMethod
{
    Random,
    VertexDistanceToAveragePlane
}

[RequireComponent(typeof(MeshFilter))]
public class MeshSimplification : MonoBehaviour
{
    public SimplificationMethod simplificationMethod;

    public int targetVertices;

    public int currentVertices;
    private MeshSimplificationAlgorithm simplificationAlgorithm;

    [HideInInspector]
    public bool debugMode = false;

    [ContextMenu("Toggle Debug Mode")]
    public void ToggleDebugMode()
    {
        debugMode = !debugMode;
    }

    [ContextMenu("Update Vertice Count")]
    public void UpdateVerticeCount()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        currentVertices = meshFilter.sharedMesh.vertices.Length;
    }

    private Mesh SimplifyMesh(Mesh mesh)
    {
        if (simplificationMethod == SimplificationMethod.Random)
        {
            simplificationAlgorithm = new RandomMeshSimplification();
        } else if (simplificationMethod == SimplificationMethod.VertexDistanceToAveragePlane)
        {
            simplificationAlgorithm = new VDMeshSimplification();
        }

        return simplificationAlgorithm.SimplifyMesh(mesh, targetVertices);
    }

    public void Simplify()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = meshFilter.sharedMesh;

        meshFilter.sharedMesh = SimplifyMesh(mesh);
    }

    public void TestCollapse(int v1, int v2)
    {
        // simplificationAlgorithm = new RandomMeshSimplification();
        // simplificationAlgorithm.HalfEdgeCollapse(v1, v2);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshSimplification))]
public class MeshSimplificationEditor : Editor {

    private int index1 = -1, index2 = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshSimplification editorObj = target as MeshSimplification;

        if (editorObj == null) return;

        if (GUILayout.Button("Simplify"))
        {
            editorObj.Simplify();
        }

        if (GUILayout.Button("Simplify Step-by-step"))
        {
            MeshFilter meshFilter = editorObj.GetComponent<MeshFilter>();
            int currentVertices = meshFilter.sharedMesh.vertices.Length;

            editorObj.targetVertices = currentVertices - 1;
            editorObj.Simplify();
        }

        if (editorObj.debugMode)
        {
            index1 = EditorGUILayout.IntField("Index1:", index1);
            index2 = EditorGUILayout.IntField("Index2:", index2);

            if (GUILayout.Button("Test Collapse"))
            {
                editorObj.TestCollapse(index1, index2);
            }
        }

    }

}
#endif