Shader "Custom/Geometry/Extrude"
{
    Properties
    {
        _Color ("Sample Color", Color) = (1, 1, 1, 1) // (R, G, B, A)
        _Factor ("Factor", Range(0.0, 2.0)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            struct v2g
            {
                float4 vertex: POSITION;
                float3 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };


            struct g2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            float4 _Color;
            float _Factor;

            v2g vert(appdata_base v)
            {
                v2g o;

                o.vertex = v.vertex;
                o.uv = v.texcoord;
                o.normal = v.normal;

                return o;
            }

            [maxvertexcount(24)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> tristream)
            {
                g2f o;

                float3 edgeA = IN[1].vertex - IN[0].vertex;
                float3 edgeB = IN[2].vertex - IN[0].vertex;

                float3 normalFace = normalize(cross(edgeA, edgeB));

                                for(int i = 0; i < 3; i++)
                {
                    o.pos = UnityObjectToClipPos(IN[i].vertex);
                    o.uv = IN[i].uv;
                    o.color = fixed4(0., 0., 0., 1.);
                    tristream.Append(o);

                    o.pos = UnityObjectToClipPos(IN[i].vertex + float4(normalFace, 0) * _Factor);
                    o.uv = IN[i].uv;
                    o.color = fixed4(1., 1., 1., 1.);
                    tristream.Append(o);

                    int inext = (i+1) % 3;

                    o.pos = UnityObjectToClipPos(IN[inext].vertex);
                    o.uv = IN[inext].uv;
                    o.color = fixed4(0., 0., 0., 1.);
                    tristream.Append(o);

                    tristream.RestartStrip();

                    o.pos = UnityObjectToClipPos(IN[i].vertex + float4(normalFace, 0) * _Factor);
                    o.uv = IN[i].uv;
                    o.color = fixed4(1., 1., 1., 1.);
                    tristream.Append(o);

                    o.pos = UnityObjectToClipPos(IN[inext].vertex);
                    o.uv = IN[inext].uv;
                    o.color = fixed4(0., 0., 0., 1.);
                    tristream.Append(o);

                    o.pos = UnityObjectToClipPos(IN[inext].vertex + float4(normalFace, 0) * _Factor);
                    o.uv = IN[inext].uv;
                    o.color = fixed4(1., 1., 1., 1.);
                    tristream.Append(o);

                    tristream.RestartStrip();
                }

                for(int i = 0; i < 3; i++)
                {
                    o.pos = UnityObjectToClipPos(IN[i].vertex + float4(normalFace, 0) * _Factor);
                    o.uv = IN[i].uv;
                    o.color = fixed4(1., 1., 1., 1.);
                    tristream.Append(o);
                }

                tristream.RestartStrip();

                for(int i = 0; i < 3; i++)
                {
                    o.pos = UnityObjectToClipPos(IN[i].vertex);
                    o.uv = IN[i].uv;
                    o.color = fixed4(0., 0., 0., 1.);
                    tristream.Append(o);
                }

                tristream.RestartStrip();

                // for (int i = 0; i < 3; i++)
                // {
                //     o.pos = UnityObjectToClipPos(IN[i].vertex);
                //     o.uv  = IN[i].uv;
                //     o.color = float4(0., 0., 0., 1.);
                //     tristream.Append(o);

                //     o.pos = UnityObjectToClipPos(IN[i].vertex + float4(normalFace, 0) * _Factor);
                //     o.uv  = IN[i].uv;
                //     o.color = float4(1., 1., 1., 1.);
                //     tristream.Append(o);

                //     int inext = (i+1) % 3;

                //     o.pos = UnityObjectToClipPos(IN[inext].vertex);
                //     o.uv  = IN[inext].uv;
                //     o.color = float4(0., 0., 0., 1.);
                //     tristream.Append(o);

                //     tristream.RestartStrip();

                //     o.pos = UnityObjectToClipPos(IN[i].vertex + float4(normalFace, 0) * _Factor);
                //     o.uv  = IN[i].uv;
                //     o.color = float4(1., 1., 1., 1.);
                //     tristream.Append(o);

                //     o.pos = UnityObjectToClipPos(IN[inext].vertex);
                //     o.uv  = IN[inext].uv;
                //     o.color = float4(0., 0., 0., 1.);
                //     tristream.Append(o);

                //     o.pos = UnityObjectToClipPos(IN[inext].vertex + float4(normalFace, 0) * _Factor);
                //     o.uv  = IN[inext].uv;
                //     o.color = float4(1., 1., 1., 1.);
                //     tristream.Append(o);

                //     tristream.RestartStrip();
                // }

                // for (int i = 0; i < 3; i++)
                // {
                //     o.pos = UnityObjectToClipPos(IN[i].vertex + float4(normalFace, 0) * _Factor);
                //     o.uv = IN[i].uv;
                //     o.color = float4(1., 1., 1., 1.);
                //     tristream.Append(o);
                // }

                // tristream.RestartStrip();

                // for (int i = 0; i < 3; i++)
                // {
                //     o.pos = UnityObjectToClipPos(IN[i].vertex);
                //     o.uv = IN[i].uv;
                //     o.color = float4(0., 0., 0., 1.);
                //     tristream.Append(o);
                // }

                // tristream.RestartStrip();
            }


            float4 frag(g2f i) : SV_TARGET
            {
                float4 color = _Color * i.color;

                return color;
            }

            ENDCG
        }
    }
}