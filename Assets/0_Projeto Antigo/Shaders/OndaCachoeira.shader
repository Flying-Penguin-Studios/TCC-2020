Shader "Custom/OndaCachoeira"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed x = IN.uv_MainTex.x;
			fixed y = IN.uv_MainTex.y;

		    //IN.uv_MainTex.x = IN.uv_MainTex.x * sin(_Time.y);
		    IN.uv_MainTex.y = IN.uv_MainTex.y * abs(sin(_Time.y));
				

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = c.rgb;
            o.Alpha = c.a * 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
