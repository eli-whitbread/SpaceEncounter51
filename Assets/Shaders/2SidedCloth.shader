Shader "Custom/2SidedCloth" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Specular("Metallic", Range(0,1)) = 0.0

		_EdgeLength("Edge Length", Range(2,50)) = 15
		_DispTex("Disp Texture", 2D) = "gray"{}
		_NormalMap("Normalmap", 2D) = "bump"{}
		_Displacement("Displacement", Range(0,1.0)) = 0.3
		_SpecColor("Spec Color", color) = (0.5,0.5,0.5,0.5)
	}
		SubShader{
			
			Tags { "RenderType" = "Opaque"}
			
			
				LOD 200
				Cull Off
				CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
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
			sampler2D _MainTex;
			float _Displacement;

			float4 tessEdge(appdata v0, appdata v1, appdata v2)
			{
				//return UnityEdgeLengthBasedTessCull(v0.vertex, v1.vertex, v2.vertex, _EdgeLength, _Displacement);
				return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _EdgeLength);

			}

			sampler2D _DispTex;

			void disp(inout appdata v)
			{
				//float2 uv_MainTex;
				float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
				v.vertex.xyz += v.normal * d;
			}

			struct Input {
				float2 uv_MainTex;

			};

			half _Glossiness;
			half _Specular;
			sampler2D _NormalMap;
			fixed4 _Color;


			void surf(Input IN, inout SurfaceOutput o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Specular = _Specular;
				o.Gloss = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
		
			
		FallBack "Diffuse"
}
