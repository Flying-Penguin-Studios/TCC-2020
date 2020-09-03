Shader "Custom/Spark"
{
    Properties
    {
        
        _TotalLinhas ("Total Linhas", float) = 10
        _TotalColunas ("Total Colunas", float) = 7
        _LinhaDesejada ("Linha Desejada", float) = 1
        _ColunaDesejada ("Coluna Desejada", float) = 1
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)

    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

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
        half _TotalLinhas;
        half _TotalColunas;
        half _LinhaDesejada;
        half _ColunaDesejada;


        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            IN.uv_MainTex.x = (IN.uv_MainTex.x /_TotalColunas + (_TotalColunas + _ColunaDesejada * 1 / _TotalColunas));
            IN.uv_MainTex.y = (IN.uv_MainTex.y/_TotalLinhas + (_TotalLinhas + _LinhaDesejada * 1/_TotalLinhas));
            

            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;

            o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

			o.Emission = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
