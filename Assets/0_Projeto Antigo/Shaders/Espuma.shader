Shader "Custom/Espuma"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_SliceGuide("Slice Guide (RGB)", 2D) = "white" {}
		_Alpha("Transparency", Range(0,1)) = 1.0
		_Speed("Speed", Range(1,50)) = 5
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows alpha:blend

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _SliceGuide;

			struct Input
			{
				float2 uv_MainTex;
				float4 color: Color;
			};

			half _Speed;
			half _Alpha;
			fixed4 _Color;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				half test = tex2D(_SliceGuide, IN.uv_MainTex).rgb - IN.color.a;
				clip(test);

				o.Albedo = c.rgb;
				o.Alpha = c.a * _Alpha;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
