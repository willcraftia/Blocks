//-----------------------------------------------------------------------------
// InstancedModel.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
// このソースコードはクリエータークラブオンラインのMeshInstancingの
// コードのコメントを翻訳、変更したもの
// http://creators.xna.com/en-US/sample/meshinstancing
//=============================================================================
#include "Basic.fx"

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal	: NORMAL0;
//    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Diffuse	: COLOR0;
    float4 Specular	: COLOR1;
//    float2 TexCoord : TEXCOORD0;
};

//-----------------------------------------------------------------------------
// 任意の回転軸で回転するクォータニオンの生成
//=============================================================================
float4 QuaternionFromAxisAngle( float3 axis, float angle)
{
	float halfAngle = angle * 0.5f;
	return float4( axis * sin(halfAngle), cos(halfAngle));
}

//-----------------------------------------------------------------------------
// クォータニオンによる頂点変換
//=============================================================================
float3 TransformByQuaternion( float3 position, float4 quaternion)
{
	float4 Q = quaternion;
	float3 v = position;
	
	return 
		( 2.0f * Q.w * Q.w - 1.0f ) * v +
		( 2.0f * dot( v, Q.xyz ) * Q.xyz ) +
		( 2.0f * Q.w * cross( Q.xyz, v ) );
}

//-----------------------------------------------------------------------------
// HWインスタンスの頂点シェーダー
// インスタンス毎の情報は２番目の頂点ストリームから読み込まれる
//=============================================================================
VertexShaderOutput HardwareInstancingVS(VertexShaderInput input,
                                                float4 instanceParam0 : TEXCOORD1,
                                                float4 instanceParam1 : TEXCOORD2 )
{
    VertexShaderOutput output;
    
    // 頂点変換
    float3 pos_os = instanceParam0.xyz;
    float scale = instanceParam0.w;
    float3 rotationAxis = instanceParam1.xyz;
    float rotation = instanceParam1.w;
    
    float4 Q = QuaternionFromAxisAngle( rotationAxis, rotation );
    float4 pos_ws = float4( pos_os +
								TransformByQuaternion( input.Position * scale, Q ), 1 );
 
    float4 pos_vs = mul(pos_ws, View);
    output.Position = mul(pos_vs, Projection);

    // 光源処理
    float3 N = TransformByQuaternion( input.Normal, Q );
	float3 posToEye = EyePosition - pos_ws;
	float3 E = normalize(posToEye);
	ColorPair lightResult = ComputeLights(E, N);
    
    output.Diffuse = float4(lightResult.Diffuse, 1);
    output.Specular = float4( lightResult.Specular, 1 );

    // 入力テクスチャ座標をそのまま出力
//    output.TexCoord = input.TexCoord;

    return output;
}

//-----------------------------------------------------------------------------
// ピクセルシェーダー
//=============================================================================
float4 PS( VertexShaderOutput pin ) : COLOR0
{
	return /*tex2D( TextureSampler, pin.TexCoord ) * */
					pin.Diffuse + float4(pin.Specular.rgb, 0);
}

//-----------------------------------------------------------------------------
// 各テクニックの宣言
//=============================================================================
// HWインスタンス
technique HardwareInstancing
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 HardwareInstancingVS();
        PixelShader = compile ps_2_0 PS();
    }
}
