Shader "Custom/ArcherDissolveEnemy" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		//Dissolve properties
		_DissolveTexture("Dissolve Texutre", 2D) = "white" {}
		_Amount("Amount", Range(0,1)) = 0
	}

		SubShader{

			Tags { "RenderType" = "Opaque" }
			LOD 200
			Cull Off //Fast way to turn your material double-sided

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;


			//Dissolve properties
			sampler2D _DissolveTexture;
			half _Amount;


			void surf(Input IN, inout SurfaceOutputStandard o) {

				//Basic shader function
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

				//Dissolve function
				half dissolve_value = tex2D(_DissolveTexture, IN.worldPos.rg).r;
				clip(dissolve_value - _Amount);

				o.Emission = fixed3(1, 0.3, 0.3) * step(dissolve_value - _Amount, 0.025f);


				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;

			}

			ENDCG
		}
			FallBack "Diffuse"
}