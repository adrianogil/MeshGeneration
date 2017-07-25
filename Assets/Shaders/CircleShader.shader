Shader "Custom/CircleShader" {
	Properties {
		_Center("Center", Vector) = (0,0,0,0)
		_Radius("Radius", float) = 0.1
		_CircleColor ("Circle Color", Color) = (1,1,1,1)
		_BackgroundColor ("Background Color", Color) = (1,1,1,1)
	}
	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			float4 _Color;

			float _Radius;
			float2 _Center;

			half4 _CircleColor;
			half4 _BackgroundColor;

			struct vert_input
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vert_output
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			vert_output vert(vert_input i)
			{
				vert_output o;

				o.position = UnityObjectToClipPos(i.vertex);
				o.uv = i.uv;

				return o;
			}

			half4 frag(vert_output o) : COLOR
			{
				if (length(o.uv - _Center) < _Radius)
				{
					return _CircleColor;
				}

				return _BackgroundColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
