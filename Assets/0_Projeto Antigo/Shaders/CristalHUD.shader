Shader "Custom/CristalHUD"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Color2("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MainTex2("Albedo 2 (RGB)", 2D) = "white" {}
		_EmissionPower("EmissionPower", Range(0,2)) = 1.0
		_SpeedX("SpeedX", Range(-20,20)) = 1.0
		_SpeedY("SpeedY", Range(-20,20)) = 1.0

	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows alpha:blend

			#pragma target 3.0


			sampler2D _MainTex;
			sampler2D _MainTex2;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_MainTex2;
			};

			half _EmissionPower;
			half _SpeedX;
			half _SpeedY;
			fixed4 _Color;
			fixed4 _Color2;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				if (c.a > 0.6) {
					c += tex2D(_MainTex2, IN.uv_MainTex2 + float2(_Time.x * _SpeedX, _Time.x * _SpeedY)) * _Color2;
				}
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Emission = o.Albedo * _EmissionPower;
			}
			ENDCG
		}
			FallBack "Diffuse"
}