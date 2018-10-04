Shader "Pumpkin/PumpkinSurface"
{
    Properties
    {
        _Color ("Tint", Color) = (0,0,0,1)
        _MainTex("Image", 2D) = "white"
    }
    Subshader
    {
        Pass
        {
            Tags{"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

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
                float3 wPos : TEXCOORD1;
            };

            vert_output vert(vert_input i)
            {
                vert_output o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.wPos = mul(unity_ObjectToWorld, i.vertex).xyz;
                o.uv = i.uv;
                o.normal = i.normal;

                return o;
            }

            float4 lambert(float4 color, float3 normal, float3 vertPos)
            {
                float3 lightDir = _WorldSpaceLightPos0.xyz - vertPos;
                float3 lightCol = _LightColor0.rgb;

                float NormaldotLight = 0.25 + 0.75 * max(dot(normal, lightDir), 0);

                float4 c = 1;

                c.rgb = color * NormaldotLight;

                // float4 c = float4(lightCol, 1);

                return c;
            }

            float4 frag(vert_output o) : COLOR
            {
                return lambert(_Color * tex2D(_MainTex, o.uv), o.normal, o.wPos);
            }

            ENDCG
        }
    }
}