Shader "UEA/Image - Lambert"
{
    Properties
    {
        _MainTex("Image", 2D) = "white"
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler _MainTex;

            struct vert_input
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct vert_output
            {
                float4 vertex : SV_POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            fixed4 simpleLambert (fixed3 normal, float4 _Color) {
                fixed3 lightDir = _WorldSpaceLightPos0.xyz; // Light direction
                fixed3 lightCol = _LightColor0.rgb;     // Light color

                fixed NdotL = max(dot(normal, lightDir),0);
                fixed4 c;
                c.rgb = _Color * lightCol * NdotL;
                c.a = 1;
                return c;
            }

            vert_output vert(vert_input i)
            {
                vert_output o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.normal = i.normal;
                o.uv = i.uv;

                return o;
            }

            half4 frag(vert_output o) : COLOR
            {
                return simpleLambert(o.normal, tex2D(_MainTex, o.uv));
            }

            ENDCG
        }
    }


}