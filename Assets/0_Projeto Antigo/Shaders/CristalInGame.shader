Shader "Custom/CristalInGame"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_EmissionColor("Emission Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionPower("Emission Power", Range(0,1)) = 0.0
		_Speed("Speed", Range(-20,20)) = 1.0
		_Alpha("Transparency", Range(0.01,1)) = 1.0
		_AlphaHeight("Alpha Height", Range(-1,1)) = 0.5
		_AlphaSmooth("Alpha Smooth", Range(0.1,1)) = 0.5
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 200
		Cull Off
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend

        #pragma target 3.0

        sampler2D _MainTex;
		
        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
		half _Emission;
		half _Alpha;
		half _Speed;
		half _AlphaHeight;
		half _AlphaSmooth;
		half _EmissionPower;
        fixed4 _Color;
		fixed4 _Color2;
		fixed4 _EmissionColor;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, (IN.uv_MainTex) + float2(0, _Time.x * _Speed)) * _Color2;
			if (c.a < 1) {
				c = c + _Color;
			}

            o.Albedo = c.rgb;
			
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
			half fadeAlpha = lerp(1,localPos.y + _AlphaHeight, _Alpha);
			fadeAlpha = max(_Alpha, smoothstep(_Alpha, max(_Alpha, _AlphaSmooth), fadeAlpha));
			o.Alpha = fadeAlpha;


			o.Emission = _EmissionColor * o.Albedo * _EmissionPower;
			
			

        }
        ENDCG
    }
    FallBack "Diffuse"
}
