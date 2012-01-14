//-----------------------------------------------------------------------------
// BasicEffectで使われているパラメーターの宣言と平行光源計算
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Texture sampler
//-----------------------------------------------------------------------------

uniform const texture BasicTexture;

uniform const sampler TextureSampler : register(s0) = sampler_state
{
	Texture = (BasicTexture);
	MipFilter = Linear;
	MinFilter = Linear;
	MagFilter = Linear;
};


//-----------------------------------------------------------------------------
// Fog settings
//-----------------------------------------------------------------------------

uniform const float		FogEnabled		: register(c0);
uniform const float		FogStart		: register(c1);
uniform const float		FogEnd			: register(c2);
uniform const float3	FogColor		: register(c3);

uniform const float3	EyePosition		: register(c4);		// in world space


//-----------------------------------------------------------------------------
// Material settings
//-----------------------------------------------------------------------------

uniform const float3	DiffuseColor	: register(c5) = 1;
uniform const float		Alpha			: register(c6) = 1;
uniform const float3	EmissiveColor	: register(c7) = 0;
uniform const float3	SpecularColor	: register(c8) = 1;
uniform const float		SpecularPower	: register(c9) = 16;


//-----------------------------------------------------------------------------
// Lights
// All directions and positions are in world space and must be unit vectors
//-----------------------------------------------------------------------------

uniform const float3	AmbientLightColor		: register(c10);

uniform const float3	DirLight0Direction		: register(c11);
uniform const float3	DirLight0DiffuseColor	: register(c12);
uniform const float3	DirLight0SpecularColor	: register(c13);

uniform const float3	DirLight1Direction		: register(c14);
uniform const float3	DirLight1DiffuseColor	: register(c15);
uniform const float3	DirLight1SpecularColor	: register(c16);

uniform const float3	DirLight2Direction		: register(c17);
uniform const float3	DirLight2DiffuseColor	: register(c18);
uniform const float3	DirLight2SpecularColor	: register(c19);


//-----------------------------------------------------------------------------
// Matrices
//-----------------------------------------------------------------------------

uniform const float4x4	World		: register(vs, c20);	// 20 - 23
uniform const float4x4	View		: register(vs, c24);	// 24 - 27
uniform const float4x4	Projection	: register(vs, c28);	// 28 - 31


//-----------------------------------------------------------------------------
// Structure definitions
//-----------------------------------------------------------------------------

struct ColorPair
{
	float3 Diffuse;
	float3 Specular;
};


//-----------------------------------------------------------------------------
// Compute lighting
// E: Eye-Vector
// N: Unit vector normal in world space
//-----------------------------------------------------------------------------
ColorPair ComputeLights(float3 E, float3 N)
{
	ColorPair result;
	
	result.Diffuse = AmbientLightColor;
	result.Specular = 0;

	// 平行光源０の計算
	float3 L = -DirLight0Direction;
	float3 H = normalize(E + L);
	float2 ret = lit(dot(N, L), dot(N, H), SpecularPower).yz;
	result.Diffuse += DirLight0DiffuseColor * ret.x;
	result.Specular += DirLight0SpecularColor * ret.y;
	
	// 平行光源１の計算
	L = -DirLight1Direction;
	H = normalize(E + L);
	ret = lit(dot(N, L), dot(N, H), SpecularPower).yz;
	result.Diffuse += DirLight1DiffuseColor * ret.x;
	result.Specular += DirLight1SpecularColor * ret.y;
	
	// 平行光源２の計算
	L = -DirLight2Direction;
	H = normalize(E + L);
	ret = lit(dot(N, L), dot(N, H), SpecularPower).yz;
	result.Diffuse += DirLight2DiffuseColor * ret.x;
	result.Specular += DirLight2SpecularColor * ret.y;
		
	result.Diffuse *= DiffuseColor;
	result.Diffuse	+= EmissiveColor;
	result.Specular	*= SpecularColor;
		
	return result;
}


//-----------------------------------------------------------------------------
// Compute fog factor
//-----------------------------------------------------------------------------
float ComputeFogFactor(float d)
{
    return clamp((d - FogStart) / (FogEnd - FogStart), 0, 1) * FogEnabled;
}
