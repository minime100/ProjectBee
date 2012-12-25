// XNA 4.0 Shader Programming #1 - Ambient light

	float4x4 World;
	float4x4 View;
	float4x4 Projection;

	float4 AmbientColor;
	float3 LightDir;

	struct VS_INPUT
	{
		float4 Position : POSITION;
		float4 Normal : NORMAL;
	};

	struct VS_OUTPUT
	{
		float4 Position : POSITION;
		float4 CamSpacePos : TEXCOORD1;
		float CosAngIncidence : TEXCOORD2;
	};

	VS_OUTPUT BasicVS(VS_INPUT input)
	{
		VS_OUTPUT output;

		float4x4 mvp = mul( mul(View, World), Projection);
		float4 pos = mul(input.Position, World);
		pos = mul(pos, View);
		pos = mul(pos, Projection);
		output.Position = pos;
		output.CamSpacePos = pos;

		float4x4 worldWithoutTranslation = World;
		worldWithoutTranslation[3,3] = 0;
		float4 normal = normalize(mul(input.Normal, worldWithoutTranslation));

		float cosAngIncidence = dot(normal, LightDir);
		output.CosAngIncidence = clamp(cosAngIncidence, 0, 1);

		return output;
	}

	float4 BasicPS(VS_OUTPUT input) : COLOR0
	{
		return float4(AmbientColor.xyz, 1) * AmbientColor.w * input.CosAngIncidence;
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
