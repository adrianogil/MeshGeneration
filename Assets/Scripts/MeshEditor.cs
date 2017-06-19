using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class VertexData
{
    public float positionX;
    public float positionY;
    public float positionZ;

    public float uValue;
    public float vValue;

    public int triangleIndex;
    public int triangleOrder;

    public bool isEditMode;
    public bool isGoingToRemove;

    public Vector3 GetPosition()
    {
        return new Vector3(positionX, positionY, positionZ);
    }
}

[Serializable]
public class TriangleData
{
    public int triangleIndex;
    public int vertexId0, vertexId1, vertexId2;
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshEditor : MonoBehaviour
{
    public Material meshMaterial;

    [HideInInspector]
    public List<VertexData> vertexData = new List<VertexData>();

    [HideInInspector]
    public List<TriangleData> triangleData = new List<TriangleData>();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void Reset()
    {
        vertexData.Clear();
        vertexData.Add(new VertexData()
        {
            triangleIndex = 0,
            triangleOrder = 0,
            positionX = 1f
        });
        vertexData.Add(new VertexData()
        {
            triangleIndex = 0,
            triangleOrder = 1,
            positionY = 1f
        });
        vertexData.Add(new VertexData()
        {
            triangleIndex = 0,
            triangleOrder = 2,
            positionZ = 1f
        });

        triangleData.Clear();
        triangleData.Add(new TriangleData()
        {
            triangleIndex = 0,
            vertexId0 = 0,
            vertexId1 = 1,
            vertexId2 = 2
        });
    }

    public void AddTriangle()
    {
        triangleData.Add(new TriangleData()
        {
            triangleIndex = triangleData.Count
        });
    }

    public void AddVertex()
    {
        vertexData.Add(new VertexData()
        {
            triangleIndex = GetNumberOfTriangles() - 1
        });
    }

    public int GetNumberOfTriangles()
    {
        return triangleData.Count;
    }

    public void LoadMesh()
    {
        MeshFilter filter = GetComponent<MeshFilter>();

        if (filter == null) return;

        Mesh mesh = filter.sharedMesh;

        Vector3[] vertices = mesh.vertices;
        Vector2[] uv = mesh.uv;

        int[] triangles = mesh.triangles;

        vertexData.Clear();

        for (int i = 0; i < vertices.Length; i++)
        {
            vertexData.Add(new VertexData()
            {
                positionX = vertices[i].x,
                positionY = vertices[i].y,
                positionZ = vertices[i].z,
                uValue = uv[i].x,
                vValue = uv[i].y
            });
        }

        triangleData.Clear();
        for (int t = 0; t < triangles.Length/3; t++)
        {
            triangleData.Add(new TriangleData()
            {
                triangleIndex = t,
                vertexId0 = triangles[3*t + 0],
                vertexId1 = triangles[3*t + 1],
                vertexId2 = triangles[3*t + 2]
            });
        }
    }

    public void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[vertexData.Count];
        Vector2[] uv = new Vector2[vertexData.Count];
        int numberOfTriangles = triangleData.Count * 3;
        int[] triangles = new int[numberOfTriangles];

        for (int i = 0; i < vertexData.Count; i++)
        {
            vertices[i] = vertexData[i].GetPosition();
            uv[i] = new Vector2(vertexData[i].uValue, vertexData[i].vValue);
        }

        for (int t = 0; t < triangleData.Count; t++)
        {
            triangles[triangleData[t].triangleIndex * 3 + 0] = triangleData[t].vertexId0;
            triangles[triangleData[t].triangleIndex * 3 + 1] = triangleData[t].vertexId1;
            triangles[triangleData[t].triangleIndex * 3 + 2] = triangleData[t].vertexId2;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        GetComponent<MeshRenderer>().material = meshMaterial;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshEditor))]
public class MeshEditorEditor : Editor {

    private MeshEditor meshEditor;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private int selectedVertexIndex = -1;

    public void OnSceneGUI()
    {
        meshEditor = target as MeshEditor;

        if (meshEditor == null) return;

        handleTransform = meshEditor.transform;
        handleRotation = handleTransform.rotation;

        List<VertexData> triangleVertexData = new List<VertexData>();

        int numberOfTriangles = meshEditor.GetNumberOfTriangles();
        for (int t = 0; t < numberOfTriangles; t++)
        {
            triangleVertexData.Clear();

            for (int i = 0; i < meshEditor.vertexData.Count; i++)
            {
                if (meshEditor.vertexData[i].triangleIndex == t)
                {
                    meshEditor.vertexData[i] = ShowVertexEditSceneUI(meshEditor.vertexData[i], i);
                    triangleVertexData.Add(meshEditor.vertexData[i]);
                }
            }
        }

        for (int t = 0; t < meshEditor.triangleData.Count; t++)
        {
            Handles.color = Color.gray;
            int index0, index1, index2;
            Vector3 p0, p1, p2;

            index0 = meshEditor.triangleData[t].vertexId0;
            index1 = meshEditor.triangleData[t].vertexId1;
            index2 = meshEditor.triangleData[t].vertexId2;

            p0 = handleTransform.TransformPoint(meshEditor.vertexData[index0].GetPosition());
            p1 = handleTransform.TransformPoint(meshEditor.vertexData[index1].GetPosition());
            p2 = handleTransform.TransformPoint(meshEditor.vertexData[index2].GetPosition());

            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p0);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        meshEditor = target as MeshEditor;

        if (meshEditor == null) return;

        if (GUILayout.Button("Load mesh"))
        {
            meshEditor.LoadMesh();
        }

        if (GUILayout.Button("Add vertex"))
        {
            meshEditor.AddVertex();
        }

        int numberOfTriangles = meshEditor.GetNumberOfTriangles();

        GUILayout.BeginVertical();
        EditorGUILayout.Space();

        for (int i = 0; i < meshEditor.vertexData.Count; i++)
        {
                meshEditor.vertexData[i] = ShowVertexEditUI(meshEditor.vertexData[i], i);
        }

        if (GUILayout.Button("Add triangle"))
        {
            meshEditor.AddTriangle();
        }

        for (int t = 0; t < meshEditor.triangleData.Count; t++)
        {
            meshEditor.triangleData[t] = ShowTriangleEditUI(meshEditor.triangleData[t]);

            meshEditor.vertexData[meshEditor.triangleData[t].vertexId0].triangleIndex = t;
            meshEditor.vertexData[meshEditor.triangleData[t].vertexId0].triangleOrder = 0;

            meshEditor.vertexData[meshEditor.triangleData[t].vertexId1].triangleIndex = t;
            meshEditor.vertexData[meshEditor.triangleData[t].vertexId1].triangleOrder = 1;

            meshEditor.vertexData[meshEditor.triangleData[t].vertexId2].triangleIndex = t;
            meshEditor.vertexData[meshEditor.triangleData[t].vertexId2].triangleOrder = 2;
        }

        if (GUILayout.Button("Generate Mesh"))
        {
            meshEditor.GenerateMesh();
        }

        Tools.hidden = selectedVertexIndex != -1;
    }

    private TriangleData ShowTriangleEditUI(TriangleData triangleData)
    {
        int v0, v1, v2;

        EditorGUILayout.LabelField("Triangle " + triangleData.triangleIndex);
        GUILayout.BeginHorizontal();
        v0 = EditorGUILayout.IntField(GUIContent.none, triangleData.vertexId0, GUILayout.Width(30));
        v1 = EditorGUILayout.IntField(GUIContent.none, triangleData.vertexId1, GUILayout.Width(30));
        v2 = EditorGUILayout.IntField(GUIContent.none, triangleData.vertexId2, GUILayout.Width(30));

        if (v0 >= 0 && v0 < meshEditor.vertexData.Count)
        {
            triangleData.vertexId0 = v0;
        }

        if (v1 >= 0 && v1 < meshEditor.vertexData.Count)
        {
            triangleData.vertexId1 = v1;
        }

        if (v2 >= 0 && v2 < meshEditor.vertexData.Count)
        {
            triangleData.vertexId2 = v2;
        }

        GUILayout.EndHorizontal();

        return triangleData;
    }

    private VertexData ShowVertexEditUI(VertexData vertexData, int index)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Vertex " + index);

        vertexData.isEditMode = selectedVertexIndex == index;

        if (!vertexData.isEditMode)
        {
            if (GUILayout.Button("Edit"))
            {
                vertexData.isEditMode = true;
                selectedVertexIndex = index;
            }
        }
        else
        {
            if (GUILayout.Button("OK"))
            {
                vertexData.isEditMode = false;
                selectedVertexIndex = -1;
            }
        }

        if (GUILayout.Button("Remove"))
        {
            vertexData.isGoingToRemove = true;
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Pos", GUILayout.Width(40));
        vertexData.positionX = EditorGUILayout.FloatField(GUIContent.none, vertexData.positionX, GUILayout.Width(50));
        vertexData.positionY = EditorGUILayout.FloatField(GUIContent.none, vertexData.positionY, GUILayout.Width(50));
        vertexData.positionZ = EditorGUILayout.FloatField(GUIContent.none, vertexData.positionZ, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UV", GUILayout.Width(40));
        vertexData.uValue = EditorGUILayout.FloatField(GUIContent.none, vertexData.uValue, GUILayout.Width(50));
        vertexData.vValue = EditorGUILayout.FloatField(GUIContent.none, vertexData.vValue, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        return vertexData;
    }

    private VertexData ShowVertexEditSceneUI(VertexData vertexData, int index)
    {
        Vector3 pos = handleTransform.TransformPoint(vertexData.GetPosition());

        Handles.color = Color.white;

        float size = HandleUtility.GetHandleSize(pos);

        Handles.Label(pos + 0.6f * size * Vector3.up, "" + index);

        if (Handles.Button(pos, handleRotation, 1.2f * size * handleSize, 1.2f * size * pickSize, Handles.DotCap)) {
            vertexData.isEditMode = true;
            selectedVertexIndex = index;
        }

        if (vertexData.isEditMode) {
            EditorGUI.BeginChangeCheck();

            pos = Handles.DoPositionHandle(pos, handleRotation);

            pos = handleTransform.InverseTransformPoint(pos);

            vertexData.positionX = pos.x;
            vertexData.positionY = pos.y;
            vertexData.positionZ = pos.z;

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(meshEditor, "Move vertex");
                EditorUtility.SetDirty(meshEditor);
            }
        }


        return vertexData;
    }
}
#endif