Shader "Toon/ToonShaderFade"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"RenderQueue" = "Transparent"
		}
		
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		Cull Back

		CGPROGRAM
		#pragma surface surf CelShadingForward alpha:fade
		#pragma target 3.0

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) 
		{
			half NdotL = ceil(dot(s.Normal, lightDir));
	
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 1.5f);
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = color.rgb;
			o.Alpha = _Color.a;
		}
	ENDCG
	}
	FallBack "Diffuse"
}