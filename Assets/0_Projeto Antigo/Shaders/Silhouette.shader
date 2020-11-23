// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Silhouette"
{
    Properties 
	{
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _RimCol ("Rim Colour" , Color) = (1,0,0,1)
        _RimPow ("Rim Power", Float) = 1.0
    }

    SubShader 
	{
        Pass 
		{
            Name "Behind"
            Tags { "RenderType" = "transparent" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Greater
            Cull Back
            ZWrite Off
            LOD 200                    
               
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
               
            struct v2f
			{
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };
               
            sampler2D _MainTex;
            float4 _RimCol;
            float _RimPow;
               
            float4 _MainTex_ST;
               
            v2f vert (appdata_tan v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normal = normalize(v.normal);
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                return o;
            }
               
            half4 frag (v2f i) : COLOR
            {
                half Rim = 1 - saturate(dot(normalize(i.viewDir), i.normal));                 
                half4 RimOut = _RimCol * pow(Rim, _RimPow);
                return RimOut;
            }
            ENDCG
        }
           
            Pass 
			{
                Name "Regular"
                Tags { "RenderType"="Opaque" }
                ZTest LEqual
                ZWrite On
                Cull Back
                LOD 200
               
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
               
                struct v2f
				{
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };
               
                sampler2D _MainTex;
                float4 _MainTex_ST;
               
                v2f vert (appdata_base v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }
               
                half4 frag (v2f i) : COLOR
                {
                    half4 texcol = tex2D(_MainTex,i.uv);
                    return texcol;
                }
                ENDCG
            }          
        }
    FallBack "VertexLit"
}