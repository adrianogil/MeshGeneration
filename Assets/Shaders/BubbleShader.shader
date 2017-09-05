Shader "UEA/Bubble"
{
    Properties
    {
        _BubblePos ("Bubble Position", Vector) = (0.5, 0.5, 0, 0) // (R, G, B, A)
        _LightDir ("Light Direction", Vector) = (0.7, 0.7, 0, 0) // (R, G, B, A)
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

            float2 _BubblePos;
            float2 _LightDir;

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

            float3 soap(float3 v) {                              // Make a soap film
               return sin(15. * v);
               // return sin(15. * v + vec3(10.,10.,10.) *
               //          turbulence(v));
            }


            float H(float2 v) {                                // Make a highlight
              return max(0., 1. - v.x * v.x - v.y * v.y);
            }

            float D(float x, float y, float r) {             // Make a disk shape
               float zz = 1. - (x * x + y * y) / (r * r);
               return sqrt(max(0., zz));
            }

            // Based on http://mrl.nyu.edu/~perlin/bubble_breakdown/
            float4 frag(vert_output o) : COLOR
            {
                // return simpleLambert(o.normal, tex2D(_MainTex, o.uv));
                float3 c = float3(.05,.12,.3);                    // Blue sky
                c = lerp(float3(0.045, 0.02, 0.02), c, 0.5+0.5*o.uv.y);

                float2 orig = float2(0.5, 0.5);
                float x = o.uv.x - orig.x;
                float y =  o.uv.y - orig.y;
                float radius = 0.4;
                float z = D(x, y, radius);
               if (z > 0.) {
                    float2 t = float2((D(x + .01, y, radius) - z) / .025,    // Surface tilt
                    (D(x, y + .01, radius) - z) / .025);

                  c *= lerp(0.8, 1, z);
                  c += .3*float3(z,z,z) * pow(H(t+_LightDir)+.8*H(t-_LightDir),8.); // Highlights
                  c *= 1. + .2 * soap(.25 * float3(t, z));          // Soap film
               }

                return float4(sqrt(c), 1.);             // Final pixel color
            }

            ENDCG
        }
    }


}