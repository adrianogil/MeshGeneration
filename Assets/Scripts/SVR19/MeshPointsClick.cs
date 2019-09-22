using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshPointsClick : MonoBehaviour
{
    public MeshCollider meshCollider;
    public List<Vector3> clickedPoints;
    public Material meshMaterial;

    public float extrusionLevel;

    public bool editMesh = false;

    public MeshFilter meshFilter;

    Mesh mesh = null;

    Vector3 meanPoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (meshCollider.Raycast(ray, out hit, 100.0f))
            {
                clickedPoints.Add(hit.point);
            }
        }

        Vector3 h = Vector3.up;

        if (editMesh && mesh != null)
        {
            Debug.Log("Update");
            Vector3[] vertices = mesh.vertices;

            vertices[0] = meanPoint + extrusionLevel * h;

            for (int i = 1; i <= clickedPoints.Count; i++)
            {
                vertices[i] = clickedPoints[i - 1] + extrusionLevel * h;
            }

            mesh.vertices = vertices;
            meshFilter.mesh = mesh;

            mesh.RecalculateBounds ();
            mesh.RecalculateNormals ();
        }
    }

    void OnDrawGizmos()
    {
        if (clickedPoints != null)
        {
            for (int i = 0; i < clickedPoints.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(clickedPoints[i], 0.5f);
            }
        }
    }

    public void GenerateExtrusion()
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        meanPoint = Vector3.zero;

        for (int i = 0; i < clickedPoints.Count; i++)
        {
            meanPoint += clickedPoints[i];
        }

        meanPoint = (1f/clickedPoints.Count) * meanPoint;

        int verticesCount = 0;
        int meanPointIndex = 0;

        extrusionLevel = 0.1f;
        Vector3 h = new Vector3(0f, 0.1f, 0f);

        meshBuilder.AddVertice(meanPoint + h);

        for (int i = 0; i < clickedPoints.Count; i++)
        {
            meshBuilder.AddVertice(clickedPoints[i] + h);
            meshBuilder.AddTriangle(i+1, meanPointIndex, 1 + ((i+1) % clickedPoints.Count));
        }

        meshBuilder.AddVertice(meanPoint);
        meanPointIndex = meshBuilder.Vertices.Count;

        int a, b, c, d;

        for (int i = 0; i < clickedPoints.Count; i++)
        {
            meshBuilder.AddVertice(clickedPoints[i]);

            a = i+1;
            b = 1 + ((i+1) % clickedPoints.Count);
            c = clickedPoints.Count+i+2;;
            d = clickedPoints.Count + 2 + ((i+1) % clickedPoints.Count);

            meshBuilder.AddTriangle(c, meanPointIndex, d);
            meshBuilder.AddTriangle(a, d, c);
            meshBuilder.AddTriangle(a, b, d);
        }

        mesh = meshBuilder.CreateMesh();

        GameObject go = new GameObject("MeshObject");
        meshFilter = go.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        meshRenderer.material = meshMaterial;

        editMesh = true;

        // clickedPoints.Clear();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshPointsClick))]
public class MeshPointsClickEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshPointsClick editorObj = target as MeshPointsClick;

        if (editorObj == null) return;

        if (GUILayout.Button("Generate extrusion"))
        {
            editorObj.GenerateExtrusion();
        }
    }

}
#endif