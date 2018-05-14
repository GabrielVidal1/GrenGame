// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MossTest3" {
	Properties {
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_MainTex("Albedo", 2D) = "white" {}

		[Gamma] _Metallic("Metallic", Range(0, 1)) = 0
		_Smoothness("Smoothness", Range(0, 1)) = 0.5

	}
	SubShader {

		Pass {

			Tags {
				"LightMode" = "ForwardBase"
			}
			
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#pragma target 3.0

			#include "UnityPBSLighting.cginc"


			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _Tint;

			float _Metallic;
			float _Smoothness;

			struct FragmentInput {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;

			};

			struct VertexInput {
				float4 position : POSITION;
				float3 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};


			FragmentInput MyVertexProgram (VertexInput v)
			{
				FragmentInput i;
				i.normal = UnityObjectToWorldNormal(v.normal);

				i.worldPos = mul(unity_ObjectToWorld, v.position);

				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				i.position = UnityObjectToClipPos(v.position);
				return i;
			}

			float4 MyFragmentProgram(FragmentInput i) : SV_TARGET
			{
				i.normal = normalize(i.normal);

				float3 lightDir = _WorldSpaceLightPos0.xyz;

				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

				float3 lightColor = _LightColor0.rgb;

				//float3 reflectionDir = reflect(-lightDir, i.normal);
				//float3 halfVector = normalize(lightDir + viewDir);


				float3 albedo = tex2D( _MainTex, i.uv).rgb * _Tint;
				//albedo *= 1 - max(_SpecularTint.r, max(_SpecularTint.g, _SpecularTint.b));

				float3 specularTint;// = albedo * _Metallic;
				float oneMinusReflectivity;// = 1 - _Metallic;

				albedo = DiffuseAndSpecularFromMetallic(
				albedo, _Metallic, specularTint, oneMinusReflectivity);
				//float3 specular = specularTint * lightColor * pow(DotClamped( halfVector, i.normal), _Smoothness*100);

				//float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);


				UnityLight light;
				light.color = lightColor;
				light.dir = lightDir;
				light.ndotl = DotClamped(i.normal, lightDir);

				UnityIndirect indirectLight;
				indirectLight.diffuse = 0;
				indirectLight.specular = 0;

				return UNITY_BRDF_PBS(albedo, specularTint, 
									oneMinusReflectivity, _Smoothness, 
									i.normal, viewDir,
									light, indirectLight);
			}
			ENDCG
		}
	}
}
