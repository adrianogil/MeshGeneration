Shader "Eyes/Eyeball"
{
    Properties
    {
        _IrisRadius("Iris Radius", Range (0.0, 1.0)) = 0.1
        _IrisExternalRadius("Iris External Radius", Range (0.0, 1.0)) = 0.1
        _IrisPositionX("Iris Position X", Range (0.0, 1.0)) = 0.5
        _IrisPositionY("Iris Position Y", Range (0.0, 1.0)) = 0.5
        _LightIluminationEffectIris("Light Factor Iris", Range (0.0, 1.0)) = 1.0
        _LightIluminationEffectEye("Light Factor Eye", Range (0.0, 1.0)) = 1.0
        _IrisColorA("Iris Color A", COLOR) = (1,0,0,1)
        _IrisColorB("Iris Color B", COLOR) = (0,1,0,1)
        _IrisExternalColor("Iris External Color", COLOR) = (0,1,0,1)
        _MainTex("Image", 2D) = "white"
    }
    Subshader
    {
        Tags {"LightMode"="ForwardBase"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc" // for _LightColor0

            #define min4(x,y,z,w) min(x, min(y, min(z, w)))
            #define PI 3.14

            sampler _MainTex;

            float _IrisRadius, _IrisExternalRadius;
            float _IrisPositionX;
            float _IrisPositionY;
            float _LightIluminationEffectIris;
            float _LightIluminationEffectEye;

            float4 _IrisColorA, _IrisColorB, _IrisExternalColor;

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

            float4 simpleLambert (float3 normal, float4 color) {
                float3 lightDir = _WorldSpaceLightPos0.xyz; // Light direction
                float3 lightCol = _LightColor0.rgb;     // Light color

                float NdotL = max(dot(normal, lightDir),0);
                float4 c;
                c.rgb = color * lightCol * NdotL;
                c.a = 1 * color.a;
                return c;
            }

            float3 getVectorFromLatLong(float latValue, float longValue)
            {
                float a1 = PI * latValue;
                float sin1 = sin(a1);
                float cos1 = cos(a1);

                float a2 = 2 * PI * longValue;
                float sin2 = sin(a2);
                float cos2 = cos(a2);

                float3 _IrisVertexPos = float3( sin1 * cos2, cos1, sin1 * sin2);

                return _IrisVertexPos;
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

                float3 _IrisVertexPos = getVectorFromLatLong(_IrisPositionX, _IrisPositionY);

                float distanceToCenterIris = length(_IrisVertexPos -
                                                    float3(o.obj_vertex.x,
                                                           o.obj_vertex.y,
                                                           o.obj_vertex.z));
                float4 finalColorLight;

                if (distanceToCenterIris < _IrisRadius)
                {
                    float t = (distanceToCenterIris - 0.5) / (_IrisRadius - 0.5);
                    finalColor.rgb = lerp(_IrisColorA, _IrisColorB, t);

                    finalColorLight = simpleLambert(o.obj_vertex.xyz, finalColor);
                    finalColor = lerp(finalColor, finalColorLight, _LightIluminationEffectIris);
                } else if (distanceToCenterIris < _IrisExternalRadius)
                {
                    finalColor = _IrisExternalColor;
                    finalColorLight = simpleLambert(o.obj_vertex.xyz, finalColor);
                    finalColor = lerp(finalColor, finalColorLight, _LightIluminationEffectIris);
                } else {
                    finalColor.rgb = float3(1,1,1);
                    finalColorLight = simpleLambert(o.obj_vertex.xyz, finalColor);
                    finalColor = lerp(finalColor, finalColorLight, _LightIluminationEffectEye);
                }

                // finalColor.rgb = distanceToCenterIris * float3(1,1,1);
                // finalColor.rgb = saturate(50 * (1 - (distanceToCenterIris / _IrisRadius))) * float3(1,1,1);

                return finalColor;
            }

            ENDCG
        }
    }
}