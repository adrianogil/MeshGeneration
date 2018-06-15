Shader "Custom/CircularDot"
{
    Properties{
        // _Tint("Tint Color", Color) = (.5, .5, .5, .5)
        // [Gamma] _Exposure("Exposure", Range(0, 8)) = 1.0
        // _Rotation("Rotation", Range(0, 360)) = 0
        // [NoScaleOffset] _Tex("Panorama (HDR)", 2D) = "grey" {}
        _ScreenDirection ("Screen Direction", Vector) = (1, 0, 0, 0) // (R, G, B, A)
        _UpScreenDirection ("Screen Direction", Vector) = (0, 1, 0, 0) // (R, G, B, A)
        // _GazeDirection ("Gaze Direction", Vector) = (1, 1, 0, 0) // (R, G, B, A)
        _MinAngle ("Min Angle", Float) = 10// (R, G, B, A)
        _MaxAngle ("Max Angle", Float) = 30// (R, G, B, A)
        _MaxUpScreenDot("Max Up Screen Dot", Float) = 2
        _MaxCrossScreenDot("Max Cross Screen Dot", Float) = 2
    }
    SubShader{
        Tags{ "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off ZWrite Off

        Pass{

	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag

	        #include "UnityCG.cginc"

	        float3 _ScreenDirection;
	        float3 _UpScreenDirection;
	        float _MinAngle;
	        float _MaxAngle;
	        float _MaxUpScreenDot;
	        float _MaxCrossScreenDot;

	        // sampler2D _Tex;
	        // float4 _Tex_HDR;
	        // float4 _Tint;
	        // float _Exposure;
	        // float _Rotation;

	        float4 RotateAroundYInDegrees(float4 vertex, float degrees)
	        {
	            float alpha = degrees * UNITY_PI / 180.0;
	            float sina, cosa;
	            sincos(alpha, sina, cosa);
	            float2x2 m = float2x2(cosa, -sina, sina, cosa);
	            return float4(mul(m, vertex.xz), vertex.yw).xzyw;
	        }

	        struct appdata_t {
	            float4 vertex : POSITION;
	        };

	        struct v2f {
	            float4 vertex : SV_POSITION;
	            float3 texcoord : TEXCOORD0;
	        };

	        v2f vert(appdata_t v)
	        {
	            v2f o;
	            o.vertex = UnityObjectToClipPos(v.vertex);
	            o.texcoord = v.vertex.xyz;
	            return o;
	        }

	        fixed4 frag(v2f i) : SV_Target
	        {
	            float3 dir = normalize(i.texcoord);

	            float3 _CrossDirection = cross(_ScreenDirection, _UpScreenDirection);

	            if (dot(dir, _ScreenDirection) > cos(_MinAngle*UNITY_PI/180.0) &&
	            	abs(dot(dir, _UpScreenDirection)) < _MaxUpScreenDot &&
	            	abs(dot(dir, _CrossDirection)) < _MaxCrossScreenDot )
	            {
	            	return float4(0,1,0,1);
	            } else if (dot(dir, _ScreenDirection) > cos(_MaxAngle*UNITY_PI/180.0))
	            {
	            	return float4(0,0,1,1);
	            }

	            // float2 longlat = float2(atan2(dir.x, dir.z) + UNITY_PI, acos(-dir.y));
	            // float2 uv = longlat / float2(2.0 * UNITY_PI, UNITY_PI);
	            // uv.x = 1.0 - uv.x;
	            // float4 tex = tex2D(_Tex, uv);
	            // float3 c = DecodeHDR(tex, _Tex_HDR);
	            // c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
	            // c *= _Exposure;

	            return float4(1,1,1,1);

	            // return float4(c, 1);
	        }
	        ENDCG
	    }
    }


    Fallback Off
}