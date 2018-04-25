using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProceduralChest : MonoBehaviour {

    public Vector2 baseSize;

    public float height1, height2;

	public void Generate () {

        MeshBuilder baseMesh1 = new MeshBuilder();
        QuadMesh.Create(baseMesh1,
            Vector3.right * baseSize.x,
            Vector3.forward * baseSize.y);

        baseMesh1 = ExtrudeMesh.From(baseMesh1, Vector3.up * height1).Join(baseMesh1);

        MeshBuilder circleMesh = CircleMesh.Create(height2, 0f, 180f,
            -Vector3.forward,
            Vector3.up).
            Translate(Vector3.up * height1 + Vector3.forward * 0.5f * baseSize.y);

        circleMesh = ExtrudeMesh.From(circleMesh, Vector3.right * baseSize.x).Join(circleMesh);

        MeshFilter filter = gameObject.GetComponent< MeshFilter >();
        Mesh mesh = MeshBuilder.Join(baseMesh1, circleMesh).CreateMesh();
        mesh.RecalculateBounds();
        filter.mesh = mesh;
	}

}


#if UNITY_EDITOR
[CustomEditor(typeof(ProceduralChest))]
public class ProceduralChestEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProceduralChest editorObj = target as ProceduralChest;

        if (editorObj == null) return;

        if (GUILayout.Button("Generate"))
        {
            editorObj.Generate();
        }
    }

}
#endif