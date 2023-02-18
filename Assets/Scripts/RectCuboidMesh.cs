using UnityEngine;

public class RectCuboidMesh
{
    public Vector3 size;

    public MeshBuilder Create()
    {
        return RectCuboidMesh.Create(size);
    }

    public static MeshBuilder Create(Vector3 size)
    {
        MeshBuilder builder = new MeshBuilder();

        // Considering local (0,0,0) as the center of the cube
        var halfSize = size * 0.5f;

        // Face - Up
		var p1 = Vector3.up * halfSize.y + Vector3.forward * halfSize.z + Vector3.right * halfSize.x;
		var p2 = p1 - Vector3.forward * size.z;
		var p3 = p1 - Vector3.right * size.x;
		var p4 = p1 - Vector3.forward * size.z - Vector3.right * size.x;

		builder.AddTriangleMesh(p1, p2, p3);
		builder.AddTriangleMesh(p2, p4, p3);

		// Face - Down
		var p5 = -Vector3.up * halfSize.y + Vector3.forward * halfSize.z + Vector3.right * halfSize.x;
		var p6 = p5 - Vector3.forward * size.z;
		var p7 = p5 - Vector3.right * size.x;
		var p8 = p5 - Vector3.forward * size.z - Vector3.right * size.x;
		builder.AddTriangleMesh(p5, p7, p6);
		builder.AddTriangleMesh(p6, p7, p8);

		// Face - Right
		var p9 = Vector3.right * halfSize.x + Vector3.forward * halfSize.z + Vector3.up * halfSize.y;
		var p10 = p9 - Vector3.forward * size.z;
		var p11 = p9 - Vector3.up * size.y;
		var p12 = p9 - Vector3.forward * size.z - Vector3.up * size.y;

		builder.AddTriangleMesh(p9, p11, p10);
		builder.AddTriangleMesh(p10, p11, p12);

		// Face - Left
		var p13 = -Vector3.right * halfSize.x + Vector3.forward * halfSize.z + Vector3.up * halfSize.y;
		var p14 = p13 - Vector3.forward * size.z;
		var p15 = p13 - Vector3.up * size.y;
		var p16 = p13 - Vector3.forward * size.z - Vector3.up * size.y;

		builder.AddTriangleMesh(p13, p14, p15);
		builder.AddTriangleMesh(p14, p16, p15);

		// Face - Forward
		var p17 = Vector3.forward * halfSize.z + Vector3.right * halfSize.x + Vector3.up * halfSize.y;
		var p18 = p17 - Vector3.right * size.x;
		var p19 = p17 - Vector3.up * size.y;
		var p20 = p17 - Vector3.right * size.x - Vector3.up * size.y;

		builder.AddTriangleMesh(p17, p18, p19);
		builder.AddTriangleMesh(p18, p20, p19);

		// Face - Back
		var p21 = -Vector3.forward * halfSize.z + Vector3.right * halfSize.x + Vector3.up * halfSize.y;
		var p22 = p21 - Vector3.right * size.x;
		var p23 = p21 - Vector3.up * size.y;
		var p24 = p21 - Vector3.right * size.x - Vector3.up * size.y;

		builder.AddTriangleMesh(p21, p23, p22);
		builder.AddTriangleMesh(p22, p23, p24);        
        
        return builder;
    }
}