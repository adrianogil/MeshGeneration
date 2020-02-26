Shader "Custom/ColorExperiment"
{
    Properties
    {
        _Red("Red", Range (0.0, 1.0)) = 0.0
        _Green("Green", Range (0.0, 1.0)) = 0.0
        _Blue("Blue", Range (0.0, 1.0)) = 0.0
        _Contrast("Contrast", Range (0.0, 20.0)) = 1.0
        _MainTex("Image", 2D) = "white"
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler _MainTex;

            float _Red, _Green, _Blue, _Contrast;

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

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;

                return o;
            }

            float4 frag(vert_output o) : COLOR
            {
                float4 color = float4(_Red, _Green, _Blue, 1);
                color.rgb = saturate((color.rgb - 0.5) * _Contrast + 0.5);

                return color;
            }

            ENDCG
        }
    }
}