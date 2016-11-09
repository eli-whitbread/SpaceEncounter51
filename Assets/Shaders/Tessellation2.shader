﻿Shader "Custom/XX121_DistanceTessellation" {
	Properties{
		_Colour("Color", color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_EdgeLength("Edge Length", Range(2,50)) = 15
		_DispTex("Disp Texture", 2D) = "gray"{}
		_NormalMap("Normalmap", 2D) = "bump"{}
		_Displacement("Displacement", Range(0,1.0)) = 0.3
		_SpecColor("Spec Color", color) = (0.5,0.5,0.5,0.5)
		[Toggle]_UseCulling("Use Culling", Range(0,1)) = 0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 300

		CGPROGRAM

#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessEdge nolightmap
#pragma target 3.0
#include "Tessellation.cginc"

		struct appdata {
		float4 vertex : POSITION;
		float4 tangent : TANGENT;
		float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;

		};


	float _EdgeLength;
	float _Displacement;
	int _UseCulling;

	float4 tessEdge(appdata v0, appdata v1, appdata v2)
	{
		if (_UseCulling == 0)
		{
			return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}
		else
		{
			return UnityEdgeLengthBasedTessCull(v0.vertex, v1.vertex, v2.vertex, _EdgeLength, _Displacement);
		}


	}

	sampler2D _DispTex;

	void disp(inout appdata v)
	{
		float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
		v.vertex.xyz += v.normal * d;
	}

	struct Input
	{
		float2 uv_MainTex;
	};

	sampler2D _MainTex;
	sampler2D _NormalMap;
	fixed4 _Colour;

	void surf(Input IN, inout SurfaceOutput o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Colour;
		o.Albedo = c.rgb;
		o.Specular = 0.2;
		o.Gloss = 1.0;
		o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));

	}
	ENDCG
	}
		FallBack "Diffuse"
}
