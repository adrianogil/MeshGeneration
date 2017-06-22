Shader "UEAGames/SurfaceShader"
{

	Properties 
	{

	}	

	Subshader
	{
		CGPROGRAM
		#pragma surface meusurf Lambert

		struct Input
		{
			float4 worldPos;
		};

		void meusurf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = float4(1,0,0,1);
		}

		ENDCG
	}

}