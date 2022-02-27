#ifndef LAMBERT_LIB
#define LAMBERT_LIB

#include "UnityCG.cginc"

fixed4 simpleLambert (fixed3 normal, float4 mainColor) {
	fixed3 lightDir = _WorldSpaceLightPos0.xyz;	// Light direction
	fixed3 lightCol = _LightColor0.rgb;		// Light color

	fixed NdotL = max(dot(normal, lightDir),0);
	fixed4 c;
	c.rgb = mainColor * lightCol * NdotL;
	c.a = 1;
	return c;
}

#endif