#ifndef SDF
#define SDF

float sdf_box (float3 p, float3 c, float3 s)
{
    float x = max
    (   p.x - c.x - float3(s.x / 2., 0, 0),
        c.x - p.x - float3(s.x / 2., 0, 0)
    );

    float y = max
    (   p.y - c.y - float3(s.y / 2., 0, 0),
        c.y - p.y - float3(s.y / 2., 0, 0)
    );

    float z = max
    (   p.z - c.z - float3(s.z / 2., 0, 0),
        c.z - p.z - float3(s.z / 2., 0, 0)
    );

    float d = x;
    d = max(d,y);
    d = max(d,z);
    return d;
}

float vmax(float3 v)
{
    return max(max(v.x, v.y), v.z);
}

float sdf_boxcheap(float3 p, float3 c, float3 s)
{
    return vmax(abs(p-c) - s);
}

float sdf_sphere (float3 p, float3 c, float r)
{
    return distance(p,c) - r;
}

float sdf_blend(float d1, float d2, float a)
{
    return a * d1 + (1 - a) * d2;
}

float sd_torus( float3 p, float2 t)
{
  float2 q = float2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

float sd_cylinder( float3 p, float3 c )
{
  return length(p.xz-c.xy)-c.z;
}

float sd_cone( float3 p, float2 c )
{
    // c must be normalized
    float q = length(p.xy);
    return dot(c,float2(q,p.z));
}

float sd_hex_prism( float3 p, float2 h )
{
    float3 q = abs(p);
    return max(q.z-h.y,max((q.x*0.866025+q.y*0.5),q.y)-h.x);
}

float sd_tri_prism( float3 p, float2 h )
{
    float3 q = abs(p);
    return max(q.z-h.y,max(q.x*0.866025+p.y*0.5,-p.y)-h.x*0.5);
}

float sd_capsule( float3 p, float3 a, float3 b, float r )
{
    float3 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

float sd_capped_cone( in float3 p, in float3 c )
{
    float2 q = float2( length(p.xz), p.y );
    float2 v = float2( c.z*c.y/c.x, -c.z );
    float2 w = v - q;
    float2 vv = float2( dot(v,v), v.x*v.x );
    float2 qv = float2( dot(v,w), v.x*w.x );
    float2 d = max(qv,0.0)*qv/vv;
    return sqrt( dot(w,w) - max(d.x,d.y) ) * sign(max(q.y*v.x-q.x*v.y,w.y));
}

float sd_ellipsoid( in float3 p, in float3 r )
{
    return (length( p/r ) - 1.0) * min(min(r.x,r.y),r.z);
}


float length2(float2 v)
{
    return v.x*v.x + v.y*v.y;
}

float length8(float2 v)
{
    return length2(v) * length2(v) * length2(v);
}

float sd_torus82( float3 p, float2 t )
{
  float2 q = float2(length2(p.xz)-t.x,p.y);
  return length8(q)-t.y;
}

float sd_torus88( float3 p, float2 t )
{
  float2 q = float2(length8(p.xz)-t.x,p.y);
  return length8(q)-t.y;
}

// Union
float op_union( float d1, float d2 )
{
    return min(d1,d2);
}

// Substraction
float op_substraction( float d1, float d2 )
{
    return max(-d1,d2);
}

// Intersection
float op_intersection( float d1, float d2 )
{
    return max(d1,d2);
}

// Domain operations
// Where "primitive", in the examples below, is any distance formula
// really (one of the basic primitives above, a combination, or a
// complex distance field).

// Repetition  q = repeat(p,c); d = primitive(q)
float3 op_repeat( float3 p, float3 c)
{
    // float3 q = (p % c) + offset;
    float3 q = abs(fmod(p,c)) - 0.5 * c;
    // float3 q = fmod(p,c);
    return q;
    // return primitve( q );
}

// float4x4 inverse(float4x4 input)
//  {
//      #define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
//      //determinant(float3x3(input._22_23_23, input._32_33_34, input._42_43_44))

//      float4x4 cofactors = float4x4(
//           minor(_22_23_24, _32_33_34, _42_43_44),
//          -minor(_21_23_24, _31_33_34, _41_43_44),
//           minor(_21_22_24, _31_32_34, _41_42_44),
//          -minor(_21_22_23, _31_32_33, _41_42_43),

//          -minor(_12_13_14, _32_33_34, _42_43_44),
//           minor(_11_13_14, _31_33_34, _41_43_44),
//          -minor(_11_12_14, _31_32_34, _41_42_44),
//           minor(_11_12_13, _31_32_33, _41_42_43),

//           minor(_12_13_14, _22_23_24, _42_43_44),
//          -minor(_11_13_14, _21_23_24, _41_43_44),
//           minor(_11_12_14, _21_22_24, _41_42_44),
//          -minor(_11_12_13, _21_22_23, _41_42_43),

//          -minor(_12_13_14, _22_23_24, _32_33_34),
//           minor(_11_13_14, _21_23_24, _31_33_34),
//          -minor(_11_12_14, _21_22_24, _31_32_34),
//           minor(_11_12_13, _21_22_23, _31_32_33)
//      );
//      #undef minor
//      return transpose(cofactors) / determinant(input);
//  }

// // Rotation/Translation q = op_tx(p, m); d = primitive(q)
// float3 op_tx( float3 p, float4x4 m )
// {
//     float3 q = inverse(m)*p;
//     return q;
//     // return primitive(q);
// }

// Scale
// float op_scale( float3 p, float s )
// {
//     return primitive(p/s)*s;
// }

//  You must be carefull when using distance transformation functions,
// as the field created might not be a real distance function anymore.
// You will probably need to decrease your step size, if you are using
// a raymarcher to sample this. The displacement example below is using
// sin(20*p.x)*sin(20*p.y)*sin(20*p.z) as displacement pattern, but
// you can of course use anything you might imagine. As for smin()
// function in opBlend(), please read the smooth minimum article in
// this same site.

// exponential smooth min (k = 32);
float exp_smin( float a, float b, float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}

// polynomial smooth min (k = 0.1);
float poly_smin( float a, float b, float k )
{
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return lerp( b, a, h ) - k*h*(1.0-h);
}

// power smooth min (k = 8);
float pow_smin( float a, float b, float k )
{
    a = pow( a, k ); b = pow( b, k );
    return pow( (a*b)/(a+b), 1.0/k );
}

float smin( float a, float b, float k )
{
    return poly_smin(a, b, k);
}

// Displacement
// float op_displace( float3 p )
// {
//     float d1 = primitive(p);
//     float d2 = displacement(p);
//     return d1+d2;
// }

// Blend
// float op_blend( float3 p )
// {
//     float d1 = primitiveA(p);
//     float d2 = primitiveB(p);
//     return smin( d1, d2 );
// }

// Domain Deformations
// Domain deformation functions do not preserve distances neither.
// You must decrease your marching step to properly sample these
// functions (proportionally to the maximun derivative of the
// domain distortion function). Of course, any distortion
// function can be used, from twists, bends, to random noise
// driven deformations.

// Twist q = op_twist(p); d = primitive(q)
float op_twist( float3 p )
{
    float c = cos(20.0*p.y);
    float s = sin(20.0*p.y);
    float2x2  m = float2x2(c,-s,s,c);
    float3  q = float3(mul(m,p.xz),p.y);
    return q;
    // return primitive(q);
}

// Cheap Bend q = op_cheap_bend(p); d = primitive(q)
float op_cheap_bend( float3 p )
{
    float c = cos(20.0*p.y);
    float s = sin(20.0*p.y);
    float2x2  m = float2x2(c,-s,s,c);
    float3  q = float3(mul(m,p.xy),p.z);
    return q;
    // return primitive(q);
}

#endif