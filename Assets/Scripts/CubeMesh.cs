using UnityEngine;

public class CubeMesh
{
    public float size;

    public MeshBuilder Create()
    {
        return CubeMesh.Create(size);
    }

    public static MeshBuilder Create(float size)
    {
        MeshBuilder builder = new MeshBuilder();

        // Considering local (0,0,0) as the center of the cube
        var halfSize = size * 0.5f;

        // Face - Up
		var p1 = Vector3.up * halfSize + Vector3.forward * halfSize + Vector3.right * halfSize;
		var p2 = p1 - Vector3.forward * size;
		var p3 = p1 - Vector3.right * size;
		var p4 = p1 - Vector3.forward * size - Vector3.right * size;
		builder.AddTriangleMesh(p1, p2, p3);
		builder.AddTriangleMesh(p2, p4, p3);

		// Face - Down
		var p5 = Vector3.down * halfSize + Vector3.forward * halfSize + Vector3.right * halfSize;
		var p6 = p5 - Vector3.forward * size;
		var p7 = p5 - Vector3.right * size;
		var p8 = p5 - Vector3.forward * size - Vector3.right * size;
		builder.AddTriangleMesh(p5, p7, p6);
		builder.AddTriangleMesh(p6, p7, p8);

		// Face - Left
		var p9 = Vector3.left * halfSize + Vector3.forward * halfSize + Vector3.up * halfSize;
		var p10 = p9 - Vector3.forward * size;
		var p11 = p9 - Vector3.up * size;
		var p12 = p9 - Vector3.forward * size - Vector3.up * size;
		builder.AddTriangleMesh(p9, p10, p11);
		builder.AddTriangleMesh(p10, p12, p11);

		// Face - Right
		var p13 = Vector3.right * halfSize + Vector3.forward * halfSize + Vector3.up * halfSize;
		var p14 = p13 - Vector3.forward * size;
		var p15 = p13 - Vector3.up * size;
		var p16 = p13 - Vector3.forward * size - Vector3.up * size;
		builder.AddTriangleMesh(p13, p15, p14);
		builder.AddTriangleMesh(p14, p15, p16);

		// Face - Front
		var p17 = Vector3.forward * halfSize + Vector3.right * halfSize + Vector3.up * halfSize;
		var p18 = p17 - Vector3.right * size;
		var p19 = p17 - Vector3.up * size;
		var p20 = p17 - Vector3.right * size - Vector3.up * size;
		builder.AddTriangleMesh(p17, p18, p19);
		builder.AddTriangleMesh(p18, p20, p19);

		// Face - Back
		var p21 = Vector3.back * halfSize + Vector3.right * halfSize + Vector3.up * halfSize;
		var p22 = p21 - Vector3.right * size;
		var p23 = p21 - Vector3.up * size;
		var p24 = p21 - Vector3.right * size - Vector3.up * size;
		builder.AddTriangleMesh(p21, p23, p22);
		builder.AddTriangleMesh(p22, p23, p24);        
        
        return builder;
    }
}