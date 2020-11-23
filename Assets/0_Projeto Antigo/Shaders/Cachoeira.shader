Shader "Custom/Cachoeira"
{
    Properties
    {
        _Color ("Color Main", Color) = (1,1,1,1)
		_Color2 ("Color Foam", Color) = (1,1,1,1)
        _MainTex ("Main (RGB)", 2D) = "white" {}
	    _MainTex2("Foam (RGB)", 2D) = "white" {}
		_Alpha ("Transparency", Range(0,5)) = 1.0
		_Speed ("Speed Main", Range(0,100)) = 50.0
		_Speed2("Speed Foam", Range(0,100)) = 50.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
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

		half _Speed;
		half _Speed2;
		half _Alpha;
        fixed4 _Color;
		fixed4 _Color2;

        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed4 c = (tex2D(_MainTex, IN.uv_MainTex + float2(0, _Time.x * _Speed)) * _Color) + (tex2D(_MainTex2, IN.uv_MainTex2 + float2(0, _Time.x * _Speed2)) * _Color2);
            o.Albedo = c.rgb;
			o.Alpha = c.a * _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
