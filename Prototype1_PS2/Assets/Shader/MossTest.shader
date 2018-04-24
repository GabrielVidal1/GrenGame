Shader "Custom/AlphaMask" {
	Properties {
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_AlphaMultiplier ("Alpha Multiplier", Float) = 1


	}
	SubShader {
		 Tags {"Queue"="Transparent"}
		  Lighting Off
		  ZWrite Off
		  Blend SrcAlpha OneMinusSrcAlpha
		  //AlphaTest GEqual [_AlphaMultiplier]
		  Pass
		  {
		     SetTexture [_Mask] {combine texture}
		     SetTexture [_MainTex] {combine texture , previous * texture }
		     //SetTexture [_Mask2] {combine previous , previous * texture}
		  }



		//Tags { "RenderType"="Opaque" }
		//LOD 200

		//CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		//sampler2D _MainTex;

		//struct Input {
		//	float2 uv_MainTex;
		//};

		//half _Glossiness;
		//half _Metallic;
		//fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		//UNITY_INSTANCING_BUFFER_END(Props)

		//void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
		//	fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
		//	o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
		//	o.Metallic = _Metallic;
		//	o.Smoothness = _Glossiness;
		//	o.Alpha = c.a;
		//}
		//ENDCG
	//}
	//FallBack "Diffuse"
	}

}
