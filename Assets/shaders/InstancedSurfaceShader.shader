// Upgrade NOTE: upgraded instancing buffer 'MyProperties' to new syntax.

Shader "Instanced/Grass Shader vertex"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		_Amplitude("Amplitude", Float) = 1
		_Frequency("Frequency", Float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

		ZWrite On
		ZTest LEqual
		Cull Off

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			UNITY_INSTANCING_BUFFER_START(MyProperties)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr MyProperties
			UNITY_INSTANCING_BUFFER_END(MyProperties)

			sampler2D _MainTex;
			float _Amplitude;
			float _Frequency;

			v2f vert(appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				worldPos.xz = worldPos.xz + _Amplitude * (v.vertex.y + 0.5) * sin(_Frequency * _Time.y + worldPos.xz);

				o.vertex = mul(UNITY_MATRIX_VP, worldPos);
				o.uv = v.texcoord;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);

				fixed4 col = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color) * tex2D(_MainTex, i.uv);

				return col;
			}
			ENDCG
		}
	}
}