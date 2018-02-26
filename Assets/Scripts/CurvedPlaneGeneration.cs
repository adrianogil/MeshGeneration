using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CurvedPlaneGeneration : MonoBehaviour {

    public int verticesX, verticesY;
    public float sizeX, sizeY;
    public float maxDepth = 1f;

    public float curveVelocityX = 1f, curveVelocityY = 1f;
    public float curveFactorX = 1f, curveFactorY = 1f;

    private float segmentSizeX, segmentSizeY;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void Generate()
    {
        MeshFilter filter = gameObject.GetComponent< MeshFilter >();
        if (filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }

        MeshBuilder meshBuilder = new MeshBuilder();

        float factorSum = curveFactorY + curveFactorX;
        float factorX = curveFactorX / factorSum;
        float factorY = curveFactorY / factorSum;

        segmentSizeX = sizeX / (verticesX-1);
        segmentSizeY = sizeY / (verticesY-1);

        Vector3 initialPoint = Vector3.zero + new Vector3(-0.5f * sizeX, 0.5f * sizeY, 0f);

        Vector3 currentInitialRowPoint = initialPoint;

        float zPoint = 0f;

        // Vertices generation
        for (int y = 0; y < verticesY; y++)
        {
            for (int x = 0; x < verticesX; x++)
            {
                zPoint = maxDepth * (factorX * Mathf.Sin(curveVelocityX * Mathf.PI * x/(verticesX-1)) +
                                     factorY * Mathf.Sin(curveVelocityY * Mathf.PI * y/(verticesY-1)));
                meshBuilder.Vertices.Add(currentInitialRowPoint + new Vector3(x * segmentSizeX, 0f, zPoint));
            }

            currentInitialRowPoint = currentInitialRowPoint + new Vector3(0f, -segmentSizeY, 0f);
        }

        // Triangles generation
        int triangleIndex = 0;

        for (int y = 1; y < verticesY; y++)
        {
            for (int x = 1; x < verticesX; x++)
            {
                triangleIndex = (y-1)*verticesX + (x-1);

                meshBuilder.AddTriangle(triangleIndex, triangleIndex+1, triangleIndex+verticesX);
                meshBuilder.AddTriangle(triangleIndex+1, triangleIndex+verticesX+1, triangleIndex+verticesX);
            }
        }

        filter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CurvedPlaneGeneration))]
public class CurvedPlaneGenerationEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CurvedPlaneGeneration editorObj = target as CurvedPlaneGeneration;

        if (editorObj == null) return;

        if (GUILayout.Button("Generate"))
        {
            editorObj.Generate();
        }
    }

}
#endif
