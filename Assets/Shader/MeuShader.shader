// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UEA/Shader"
{
	Properties
	{
		_Color ("The Best Tint", Color) = (1,1,1,1)
	}
	
	Subshader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 _Color;

			struct vert_input
			{
				float4 vertex : POSITION;
			};

			struct vert_output
			{
				float4 position : SV_POSITION;
			};

			vert_output vert(vert_input i)
			{
				vert_output o;

				o.position = UnityObjectToClipPos(i.vertex);

				return o;
			}

			half4 frag(vert_output o) : COLOR
			{
				return _Color;	
			}

			ENDCG	
		}
	}
}