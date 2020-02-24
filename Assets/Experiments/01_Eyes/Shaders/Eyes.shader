Shader "Eyes/Eyeball"
{
    Properties
    {
        _IrisRadius("Iris Radius", Range (0.0, 1.0)) = 0.1
        _IrisPositionX("Iris Position X", Range (0.0, 1.0)) = 0.5
        _IrisPositionY("Iris Position Y", Range (0.0, 1.0)) = 0.5
        _MainTex("Image", 2D) = "white"
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define min4(x,y,z,w) min(x, min(y, min(z, w)))
            #define PI 3.14

            sampler _MainTex;

            float _IrisRadius;
            float _IrisPositionX;
            float _IrisPositionY;

            struct vert_input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vert_output
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 obj_vertex: TEXCOORD1;
            };

            vert_output vert(vert_input i)
            {
                vert_output o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.obj_vertex = i.vertex;
                o.uv = i.uv;

                return o;
            }

            float4 frag(vert_output o) : COLOR
            {
                float4 finalColor = float4(0,0,0,1);
                float2 _IrisPosition = float2(_IrisPositionX, _IrisPositionY);

                // UV-based approach =/
                // float distanceToCenterIris = min4(
                //     length(o.uv - _IrisPosition),
                //     length(o.uv + float2(1,0) - _IrisPosition),
                //     length(o.uv + float2(0,1) - _IrisPosition),
                //     length(o.uv + float2(1,1) - _IrisPosition));


                float a1 = PI * _IrisPositionY;
                float sin1 = sin(a1);
                float cos1 = cos(a1);

                float a2 = 2 * PI * _IrisPositionX;
                float sin2 = sin(a2);
                float cos2 = cos(a2);

                float3 _IrisVertexPos = float3( sin1 * cos2, cos1, sin1 * sin2);

                float distanceToCenterIris = length(_IrisVertexPos -
                                                    float3(o.obj_vertex.x,
                                                           o.obj_vertex.y,
                                                           o.obj_vertex.z));

                if (distanceToCenterIris < _IrisRadius)
                {
                    finalColor.rgb = float3(0,0,0);
                } else {
                    finalColor.rgb = float3(1,1,1);
                }

                // finalColor.rgb = distanceToCenterIris * float3(1,1,1);


                return finalColor;
            }

            ENDCG
        }
    }
}