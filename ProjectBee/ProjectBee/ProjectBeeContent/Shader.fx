// XNA 4.0 Shader Programming #1 - Ambient light

	float4x4 World;
	float4x4 View;
	float4x4 Projection;

	float AmbientIntensity;
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
		float4 camSpacePos : TEXCOORD1;
		float4 camSpaceNormal : TEXCOORD2;
	};

	VS_OUTPUT BasicVS(VS_INPUT input)
	{
		VS_OUTPUT output;

		float4x4 mvp = mul( mul(World, View), Projection);
		float4 projPosition = mul(input.Position, mvp);
		output.Position = projPosition;
		output.camSpacePos = projPosition;
		output.camSpaceNormal = mul(input.Normal, mvp);

		return output;
	}

	float4 BasicPS(VS_OUTPUT input) : COLOR0
	{
		return AmbientIntensity * AmbientColor;
	}

	technique Technique1
	{
		pass Pass1
		{
			VertexShader = compile vs_3_0 BasicVS();
			PixelShader = compile ps_3_0 BasicPS();
		}
	}
