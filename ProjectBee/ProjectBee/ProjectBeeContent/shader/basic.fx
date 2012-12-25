// XNA 4.0 Shader Programming #1 - Ambient light
#define LIGHT_COUNT 2

float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor;

float4 LightColors[LIGHT_COUNT];
float4 LightPositions[LIGHT_COUNT];

struct VS_INPUT
{
	float4 Position : POSITION;
	float4 Normal : NORMAL;
};

struct VS_OUTPUT
{
	float4 Position : POSITION;
	float4 CamSpacePos : TEXCOORD1;
	float4 Normal : TEXCOORD2;
};

VS_OUTPUT BasicVS(VS_INPUT input)
{
	VS_OUTPUT output;

	float4 pos = mul(input.Position, World);
	output.CamSpacePos = pos;
	pos = mul(pos, View);
	pos = mul(pos, Projection);
	output.Position = pos;

	float4x4 worldWithoutTranslation = World;
	worldWithoutTranslation[3,3] = 0;
	output.Normal = normalize(mul(input.Normal, worldWithoutTranslation));

	return output;
}

float4 BasicPS(VS_OUTPUT input) : COLOR0
{
	float4 light = float4(AmbientColor.rgb, 1) * AmbientColor.a;

	for(int i = 0; i < LIGHT_COUNT; i++) {
		float3 lightDir = {0,0,0};

		if(LightPositions[i].w < 1) {
			lightDir = LightPositions[i].xyz;
		} else {
			lightDir = LightPositions[i].xyz - normalize(input.CamSpacePos).xyz
		}

		float cosAngIncidence = dot(input.Normal, lightDir);
		cosAngIncidence = clamp(cosAngIncidence, 0, 1);

		light += float4(LightColors[i].rgb, 1) * LightColors[i].a * cosAngIncidence;
	}

	return light;
}

float4 GaussLighting(VS_OUTPUT input) : COLOR0
{

}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 BasicVS();
		PixelShader = compile ps_3_0 BasicPS();
	}
}
