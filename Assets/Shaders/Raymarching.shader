// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// http://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm

Shader "Raymarching/Test"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	Subshader
	{
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "sdf.cginc"

			#define _Steps 10
			#define _MinDistance 0.02

			uniform float4 _Color;

			fixed4 simpleLambert (fixed3 normal) {
				fixed3 lightDir = _WorldSpaceLightPos0.xyz;	// Light direction
				fixed3 lightCol = _LightColor0.rgb;		// Light color

				fixed NdotL = max(dot(normal, lightDir),0);
				fixed4 c;
				c.rgb = _Color * lightCol * NdotL;
				c.a = 1;
				return c;
			}

			float map (float3 p)
			{
				float3 q = op_repeat(p, float3(5,5,5));
				// float d = sdf_boxcheap(q, float3(0,0,0), float3(1,1,1));
				float d = sd_torus(q, float2(0.5,0.5));

				return d;

				// float d = sdf_blend(
				// 		sdf_sphere(p, 0, 2),
				// 		sdf_boxcheap(p, 0, 2),
				// 		(_SinTime[3] + 1.) / 2
				// 	);



				// d = min(d, sdf_sphere(p, - float3 (3, 0, 0), 2));
				// d = min(d, sdf_sphere(p, + float3 (3, 0, 0), 2));
				// d = min(d, sdf_boxcheap(p, float3(2,3,0), float3(1,1,1)));

			 //    return d;
			}

			float3 normal (float3 p)
			{
				const float eps = 0.01;

				return normalize
				(	float3
					(	map(p + float3(eps, 0, 0)	) - map(p - float3(eps, 0, 0)),
						map(p + float3(0, eps, 0)	) - map(p - float3(0, eps, 0)),
						map(p + float3(0, 0, eps)	) - map(p - float3(0, 0, eps))
					)
				);
			}

			fixed4 renderSurface(float3 p)
			{
				float3 n = normal(p);
				return simpleLambert(n);
			}

			fixed4 raymarch (float3 position, float3 direction)
			{
				for (int i = 0; i < _Steps; i++)
				{
					float distance = map(position);
					if (distance < _MinDistance)
						return renderSurface(position);

					position += distance * direction;
				}
				return fixed4(1,1,1,1);
			}

			struct v2f {
				float4 pos : SV_POSITION;	// Clip space
				float3 wPos : TEXCOORD1;	// World position
			};

			// Vertex function
			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			// Fragment function
			fixed4 frag (v2f i) : SV_Target
			{
				float3 worldPosition = i.wPos;
				float3 viewDirection = normalize(i.wPos - _WorldSpaceCameraPos);
				return raymarch (worldPosition, viewDirection);
			}

			ENDCG
		}
	}
}