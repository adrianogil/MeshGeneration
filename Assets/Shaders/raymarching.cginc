#ifndef RAYMARCHING_LIB
#define RAYMARCHING_LIB

#include "lambert.cginc"

#define NORMAL(p, VAR, MAP_FUNCTION) \
	const float eps = 0.01; \
	\
	VAR = normalize \
	(	float3 \
		(	MAP_FUNCTION(p + float3(eps, 0, 0)	) - MAP_FUNCTION(p - float3(eps, 0, 0)), \
			MAP_FUNCTION(p + float3(0, eps, 0)	) - MAP_FUNCTION(p - float3(0, eps, 0)), \
			MAP_FUNCTION(p + float3(0, 0, eps)	) - MAP_FUNCTION(p - float3(0, 0, eps)) \
		) \
	);

#define RENDERSURFACE(position, mainColor, MAP_FUNCTION) \
	NORMAL(position, float3 n, MAP_FUNCTION) \
	return simpleLambert(n, mainColor);

#define RAYMARCH(position, direction, mainColor, MAP_FUNCTION) \
	for (int i = 0; i < _Steps; i++) \
	{ \
		float distance = map(position); \
		if (distance < _MinDistance) { \
			RENDERSURFACE(position, mainColor, MAP_FUNCTION) \
		} \
		\
		position += distance * direction; \
	} \
	return fixed4(1,1,1,1);

#endif