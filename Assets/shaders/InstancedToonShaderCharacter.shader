Shader "Toon/InstancedToonShaderCharacter"
{
	Properties
	{
		_Color("Clothes Tint", Color) = (1, 1, 1, 1)
		_HairColor("Hair Tint", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_SkinTone ("Skin tone", Range(0.1,0.9)) = 0.5
	}
	
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}
		
		LOD 200

		CGPROGRAM
		#pragma surface surf CelShadingForward
		#pragma target 3.0
		#pragma multi_compile_instancing


		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) 
		{
			half NdotL = ceil(dot(s.Normal, lightDir));
	
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 1.5f);
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};

		UNITY_INSTANCING_CBUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)	
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _HairColor)
			UNITY_DEFINE_INSTANCED_PROP(float, _SkinTone)
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutput o) 
		{
			// R = Clothes
			// G = Skin
			// B = Hair

			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 color = fixed4(0, 0,0,0);
			color += tex.r * UNITY_ACCESS_INSTANCED_PROP(_Color);
			color += tex.g * tex2D(_MainTex, float2(0.9f, UNITY_ACCESS_INSTANCED_PROP(_SkinTone)/3));
			color += tex.b * UNITY_ACCESS_INSTANCED_PROP(_HairColor);
			
			o.Albedo = color.rgb * UNITY_ACCESS_INSTANCED_PROP(_Color).a;
			o.Alpha = color.a;
		}
	ENDCG
	}
	FallBack "Diffuse"
}