Shader "Custom/HealingGround" {


    Properties {
        _Color ("Cor Textura", Color) = (1,1,1,1)
		_ColorBorda ("Cor da Borda", Color) = (1,1,1,1)
		_EmissionBorda("Brilho Borda", Range(-50, 100)) = 0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Raio ("Raio", Range(0.5, 1)) = 0.0
		_BorderRadius("Border Radius", Range(0.0, 1.0)) = 0.1
		
		[Space(20)]
		_RaioOnda("Raio Onda", Range(0.4, 1.1)) = 0.0
		_WaveRadius("Wave Radius", Range(0.0, 0.1)) = 0.01

		[Space(20)]
		_Alpha ("Alpha", Range(0, 1)) = 1
		_EmissionPower("Emission Power", Range(0, 1)) = 1
    }
    
	SubShader {
        
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200


		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend
		#pragma target 3.0

			
		sampler2D _MainTex;


		struct Input {

			float2 uv_MainTex;
			float2 uv_MainTex2;

		};


		half _Metallic;
		fixed4 _Color;
		fixed4 _ColorBorda;
		float _Raio;
		float _RaioOnda;
		fixed _BorderRadius;
		float _EmissionBorda;
		float _WaveRadius;
		float _Alpha;
		float _EmissionPower;



		void surf (Input IN, inout SurfaceOutputStandard o)  {

			float x = IN.uv_MainTex.x - 0.5;
			float y = IN.uv_MainTex.y - 0.5;

			
			float c = tex2D(_MainTex, IN.uv_MainTex);


			o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Emission = tex2D(_MainTex, IN.uv_MainTex) * _Color * 0.2;
			o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a * _Alpha;



			clip((1 - (sqrt(pow(x, 2) + pow(y, 2))) - _Raio));


			//Aparição do Circulo
			if( c > ((1 - (sqrt(pow(x, 2) + pow(y, 2))) - _Raio) + _BorderRadius)) {
				o.Emission = c * _ColorBorda * _EmissionBorda * _EmissionPower;

			}


			//Centro do Circulo
			if(c < ((1 - (sqrt(pow(x, 2) + pow(y, 2))) - 0.915f) + 0.935)) {
				o.Alpha = _EmissionPower;
				o.Emission = c * _ColorBorda * _EmissionBorda * _EmissionPower * 1;
			}



			//Waves
			if(  c < ((1 - (sqrt(pow(x, 2) + pow(y, 2))) - _RaioOnda) + 0.95)   &&   c > ((1 - (sqrt(pow(x, 2) + pow(y, 2))) - _RaioOnda) + 0.95 - _WaveRadius)  ) {
				//o.Alpha = 1;
				o.Emission =  c * _ColorBorda * _EmissionBorda * _EmissionPower;
			}


			

			


           
		}
		ENDCG	
    }
    FallBack "Diffuse"
}
