Shader "Shader/Color"
{
	Properties
	{
	}

	Subshader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			v2f_img vert(appdata_base i)
			{
				v2f_img o;

				o.pos = UnityObjectToClipPos(i.vertex);

				return o;
			}





			half4 frag(v2f_img o) : COLOR
			{
				float red   = 1.0;
				float green = 0.0;
				float blue  = 0.0;
				float alpha = 1.0;
				return float4(red, green, blue, alpha);
			}





			ENDCG
		}
	}
}

