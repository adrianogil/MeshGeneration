Shader "Custom/CurvedShader"
{
    Properties
    {
        _CenterPosition ("CenterPosition", Vector) = (0,0,0,0)
        _Radius ("Radius", float) = 0.5

    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            float3 _CenterPosition;
            float _Radius;

            struct vert_input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vert_output
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vert_output vert(vert_input i)
            {
                vert_output o;

                float4 vertex = i.vertex;

                float3 wPos = mul(unity_ObjectToWorld, vertex).xyz;

                float4 diff = vertex;
                diff.xyz = _Radius * normalize(wPos - _CenterPosition) + _CenterPosition;

                vertex = mul(unity_WorldToObject, diff);

                o.vertex =  UnityObjectToClipPos(vertex);

                o.uv = i.uv;

                return o;
            }

            float4 frag(vert_output o) : COLOR
            {
                return float4(1,0,0,1);
            }

            ENDCG
        }
    }
}