Shader "SVR19/ColorExtrusion"
{
	Properties
	{
		_ColorA ("Color A", Color) = (0,0,0,1)
		_ColorB ("Color B", Color) = (1,1,1,1)
		_ColorVelocity("Color velocity", FLOAT) = 1
	}

	Subshader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float4 _ColorA, _ColorB;
			float _ColorVelocity;

			struct v2f {
				float4 pos : SV_POSITION;	// Clip space
				float3 wPos : TEXCOORD1;	// World position
			};

			v2f vert(appdata_base i)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(i.vertex);
				o.wPos = mul(unity_ObjectToWorld, i.vertex).xyz;

				return o;
			}

			float4 frag(v2f o) : COLOR
			{
				return lerp(_ColorA, _ColorB, frac(_ColorVelocity * o.wPos.y));
			}

			ENDCG
		}
	}
}

