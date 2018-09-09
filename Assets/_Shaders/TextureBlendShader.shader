Shader "Custom/TextureBlendShader" {
	Properties {
		_MainColor ("Main Color", Color) = (1,1,1,1)
		_SecondColor("Sencondary Color", Color) = (1,1,1,1)
		_MainTex ("Primary Albedo (RGB)", 2D) = "white" {}
		_SecondTex ("Secondary Albedoo (RGB)", 2D) = "white" {}
		_Blend ("Blend", Range(0, 1)) = 0.5
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SecondTex;

		float _Blend;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SecondTex;
		};

		half _Glossiness;
		half _Metallic;

		fixed4 _MainColor;
		fixed4 _SecondColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float4 MainColor = tex2D(_MainTex, IN.uv_MainTex) * _MainColor;
			float4 SecondColor = tex2D(_SecondTex, IN.uv_SecondTex) * _SecondColor;

			o.Albedo.rgb = MainColor.rgb * _Blend + SecondColor.rgb * (1 - _Blend);

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = MainColor.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
