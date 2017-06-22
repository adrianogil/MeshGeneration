Shader "UEAGames/VertexShader"
{

    Properties 
    {
        _Color ("Tint", Color) = (0,0,0,1)
        _MainTex ("Image", 2D) = "white"
    }   

    Subshader
    {
        Pass {
            CGPROGRAM
            #include "Lighting.cginc"

            #pragma vertex vert
            #pragma fragment frag

            float4 _Color;

            sampler _MainTex;

            struct vert_input
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };


            struct vert_output
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            vert_output vert(vert_input i)
            {
                vert_output o;

                float4 newVertex = i.vertex;
                newVertex.z = newVertex.z + sin( (i.uv.x - (_Time.y * 2)) * 2) * (i.uv.x * 0.8);
                o.vertex = UnityObjectToClipPos(newVertex);
                o.uv = i.uv;
                o.normal = i.normal;

                return o;
            }

            float4 lambert(float4 color, float3 normal)
            {
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 lightCol = _LightColor0.rgb;

                float NormaldotLight = max(dot(normal, lightDir), 0);

                float4 c = 1;

                c.rgb = color * lightCol * NormaldotLight;

                return c;
            }

            float4 frag(vert_output o) : COLOR
            {
                return lambert(_Color * tex2D(_MainTex, o.uv), o.normal);
            }

            ENDCG
        }
        













    }
}