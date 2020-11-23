// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Agua" 
{
	Properties 
	{
		_NoiseTex("Noise text", 2D) = "white" {} //Textura usada para fazer ondas e para a espuma na superfície da água.
		_Color ("Color", Color) = (1,1,1,1) //Cor da água rasa e da espuma.
		_Color2 ("Color 2", Color) = (1,1,1,1) //Cor da água funda.
		_Period ("Period", Range(0,50)) = 1 //Período utilizado no calculo da distorção da água.
		_Magnitude ("Magnitude", Range(0,0.5)) = 0.05 //Multiplica a distorção para aumentar ou diminuir o seu y.
		_Scale ("Scale", Range(0,10)) = 1 //Usado no calculo de distorção da água.
		_CubeMap("ReflectionCube", CUBE) = " " {} //CubeMap que será utilizado na reflexão da água.
		_DepthMax("Max Depth", Float) = 1 //Distância máxima pra identificar a profundidade da água, o que for mais fundo que isso não vai mudar de cor (entre color e color2)
		_Tess("Tessellation", Range(1,500)) = 4 //Controla a densidade do polígono.
		_FoamNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.8 //Usado para as espumas na superfície da água, cortando a imagem do noise para mostrar somente algumas áreas com essa espuma.
		_FoamDistance("Foam Distance", Float) = 0.8 //Distância da espuma em relação a borda
		_FoamNoiseScroll("Surface Noise Scroll", Vector) = (0.03, 0.03, 0, 0) //Multiplica o _Time.y ao animar a textura da espuma, podendo mais fácil mudar a velocidade da animação para x e y.
		
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off 
		Lighting Off 
		Cull Off 
		Fog { Mode Off } 
		Blend One Zero

		LOD 110

		GrabPass { "_GrabTexture" }
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert tessellate:tessFixed
			#pragma fragment frag 

			#include "UnityCG.cginc"

			sampler2D _GrabTexture;
			sampler2D _NoiseTex;
			sampler2D _CameraDepthTexture;
			samplerCUBE _CubeMap;
			fixed4 _Color;
			fixed4 _Color2;
			float  _Period;
			float  _Magnitude;
			float  _Scale;		
			float _Tess;
			float _FoamNoiseCutoff;
			float _DepthMax;
			float _FoamDistance;
			float2 _FoamNoiseScroll;
			float4 _NoiseTex_ST; 


			struct vin
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float4 uvgrab : TEXCOORD2;
				float3 worldRef1 : TEXCOORD3;
				float distortion : TEXCOORD4;
				float2 noiseUV : TEXCOORD5;
			};

			float4 tessFixed() {
				return _Tess;
			}

			

			v2f vert (vin v)
			{
				
				v2f o;
				float sinT = sin(_Time.w / _Period);
				float cosT = cos(_Time.w / _Period);
				float distortion = tex2Dlod(_NoiseTex, float4(mul(unity_ObjectToWorld, v.vertex).xz / _Scale + float2(sinT, cosT), 0, 0.1)) - 0.5; 
				v.vertex.y += distortion; //Com a conta da distortion é utilizado a textura noise e das fórmulas de seno e cosseno, fazendo os vértices mexerem em y, fazendo o efeito de onda.
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.distortion = distortion;
				o.color = v.color;
				o.texcoord = v.texcoord;
				o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex); //Será usado para fazer a espuma na superfície da água.

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				float3 worldViewDir = normalize(UnityWorldSpaceViewDir(o.worldPos.xyz));
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldRef1 = reflect(-worldViewDir, worldNormal); //Reflexão da superfície da água.
				o.uvgrab = ComputeGrabScreenPos(o.vertex);

				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				float sinT = sin(_Time.w / _Period);
				half4 Reflection = texCUBE(_CubeMap, i.worldRef1);

				float2 distortion = float2(	tex2D(_NoiseTex, i.worldPos.xy / _Scale + float2(sinT, 0) ).r - 0.5,
					                        tex2D(_NoiseTex, i.worldPos.xy / _Scale + float2(0, sinT) ).r - 0.5 ); //Fórmula usada para distorção da água em relação ao _NoiseTex

				i.uvgrab.xy += distortion * _Magnitude; //É multiplicado pela _Magnitude, podendo mudar mais facilmente a distorção no inspetor.

				fixed4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				if (i.distortion > 0)
					col += i.distortion;

				float currentDepth = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uvgrab)).r;
				float currentDepthLinear = LinearEyeDepth(currentDepth); //Separação entre a parte rasa e funda da água através do ponto de vista da câmera.

				float depthDifference = currentDepthLinear - i.uvgrab.w;

				float waterDepthDifference = saturate(depthDifference / _DepthMax);
				float4 waterColor = lerp(_Color, _Color2, waterDepthDifference); //É feito um lerp para dar cor a água entre as duas cores de acordo com a diferença da profundidade encontrada.

				float2 noiseUV = float2(i.noiseUV.x + _Time.y * _FoamNoiseScroll.x, i.noiseUV.y + _Time.y * _FoamNoiseScroll.y); //Animação da surperfície da água para dar o efeito de espuma.
				float foamNoiseSample = tex2D(_NoiseTex, noiseUV).r;

				float foamDepthDifference = saturate(depthDifference / _FoamDistance);
				float foamNoiseCutoff = foamDepthDifference * _FoamNoiseCutoff;
				
				float foamNoise = foamNoiseSample > foamNoiseCutoff ? 1 : 0; //É usado a textura de noise e é considerado qual parte será formada a espuma através do foamNoiseCutoff, onde o qual mais cortado mais partes da textura serão desconsideradas de acordo com sua cor, em que onde esteja próximo de 1 é mais considerado e perto de 0 não, mudando de acordo com a propriedade _FoamNoiseCutoff.
				float4 foamNoiseColor = _Color * foamNoise;

				return  (foamNoiseColor * foamNoise * foamNoiseSample + waterColor) * col * 1.5 + Reflection * 0.5; //Junção dos efeitos calculados acima.
			}
		
			ENDCG
		} 
	}
}