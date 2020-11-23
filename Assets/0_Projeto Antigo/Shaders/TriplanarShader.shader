Shader "Custom/TriplanarShader"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex("Top Texture", 2D) = "white" {}
        _NormalT("Top Normal", 2D) = "bump" {}
        _MainTexSide("Side/Bottom Texture", 2D) = "white" {}
        _Normal("Side/Bottom Normal", 2D) = "bump" {} 
        _Noise("Noise", 2D) = "white" {} //makes the transition between the textures more natural
        _Scale("Top Scale", Range(-2,2)) = 1 //controls the size of the texture (how many times it will repeat)
        _SideScale("Side Scale", Range(-2,2)) = 1 //controls the size of the texture (how many times it will repeat)
        _NoiseScale("Noise Scale", Range(-2,2)) = 1 //control the size of the noise applied in the model
        _TopSpread("TopSpread", Range(-2,2)) = 1 //control the amout of the model that will be colored with de top texture
        _EdgeWidth("EdgeWidth", Range(0,0.5)) = 1 // controls the width of the edge between to and side textures
        _EdgeColor("Edge Color", Color) = (0.5,0.5,0.5,1) //controls the color of the edge
        _RimPower("Rim Power", Range(-2,20)) = 1 // controls how much rim will be applied
        _RimColor("Rim Color Top", Color) = (0.5,0.5,0.5,1) //controls the rim color at the top
        _RimColor2("Rim Color Side/Bottom", Color) = (0.5,0.5,0.5,1) //controls the rim color at the side
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex, _MainTexSide, _Normal, _Noise, _NormalT;
        float4 _Color, _RimColor, _RimColor2, _EdgeColor;
        float _RimPower;
        float  _TopSpread, _EdgeWidth;
        float _Scale, _SideScale, _NoiseScale;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal; INTERNAL_DATA 
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;


        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 worldNormalE = WorldNormalVector(IN, o.Normal);
            float3 blendNormal = saturate(pow(worldNormalE * 1.4,4));

            //applies the noise in the veriables in all coordinates (separately)
            float3 xn = tex2D(_Noise, IN.worldPos.zy * _NoiseScale);
            float3 yn = tex2D(_Noise, IN.worldPos.zx * _NoiseScale);
            float3 zn = tex2D(_Noise, IN.worldPos.xy * _NoiseScale);

            //applies lerp in textures       
            float3 noisetexture = zn;
            noisetexture = lerp(noisetexture, xn, blendNormal.x);
            noisetexture = lerp(noisetexture, yn, blendNormal.y);

            //controls the scale of the top texture
            float3 xm = tex2D(_MainTex, IN.worldPos.zy * _Scale);
            float3 zm = tex2D(_MainTex, IN.worldPos.xy * _Scale);
            float3 ym = tex2D(_MainTex, IN.worldPos.zx * _Scale);
            
            //lerp all sides of the top texture
            float3 toptexture = zm;
            toptexture = lerp(toptexture, xm, blendNormal.x);
            toptexture = lerp(toptexture, ym, blendNormal.y);
            
            //controls the scale of the normal in the top texture
            float3 xnnt = UnpackNormal(tex2D(_NormalT, IN.worldPos.zy * _Scale));
            float3 znnt = UnpackNormal(tex2D(_NormalT, IN.worldPos.xy * _Scale));
            float3 ynnt = UnpackNormal(tex2D(_NormalT, IN.worldPos.zx * _Scale));

            //lerp toghether all sides for the top texture normal
            float3 toptextureNormal = znnt;
            toptextureNormal = lerp(toptextureNormal, xnnt, blendNormal.x);
            toptextureNormal = lerp(toptextureNormal, ynnt, blendNormal.y);
            
            //controls the normal scale for the side texture
            float3 xnn = UnpackNormal(tex2D(_Normal, IN.worldPos.zy * _SideScale));
            float3 znn = UnpackNormal(tex2D(_Normal, IN.worldPos.xy * _SideScale));
            float3 ynn = UnpackNormal(tex2D(_Normal, IN.worldPos.zx * _SideScale));

            //lerp together all sides for the side texture normal
            float3 sidetextureNormal = znn;
            sidetextureNormal = lerp(sidetextureNormal, xnn, blendNormal.x);
            sidetextureNormal = lerp(sidetextureNormal, ynn, blendNormal.y);
            
            //controls the scale of the side normal
            float3 x = tex2D(_MainTexSide, IN.worldPos.zy * _SideScale);
            float3 y = tex2D(_MainTexSide, IN.worldPos.zx * _SideScale);
            float3 z = tex2D(_MainTexSide, IN.worldPos.xy * _SideScale);
            
            float3 sidetexture = z;
            sidetexture = lerp(sidetexture, x, blendNormal.x);
            sidetexture = lerp(sidetexture, y, blendNormal.y);

            float worldNormalDotNoise = dot(o.Normal + (noisetexture.y + (noisetexture * 0.5)), worldNormalE.y);

            float3 topTextureResult = step(_TopSpread + _EdgeWidth, worldNormalDotNoise) * toptexture;
            float3 topNormalResult = step(_TopSpread, worldNormalDotNoise) * toptextureNormal;

            float3 topTextureEdgeResult = (step(_TopSpread , worldNormalDotNoise) * step(worldNormalDotNoise, _TopSpread + _EdgeWidth)) * _EdgeColor;
            
            float3 sideTextureResult = step(worldNormalDotNoise, _TopSpread) * sidetexture;
            float3 sideNormalResult = step(worldNormalDotNoise, _TopSpread) * sidetextureNormal;

            o.Normal = topNormalResult + sideNormalResult;
            o.Albedo = topTextureResult + sideTextureResult + topTextureEdgeResult;
            o.Albedo *= _Color;

            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

            half rim2 = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
        o.Emission = step(_TopSpread + _EdgeWidth, worldNormalDotNoise) * _RimColor.rgb * pow(rim, _RimPower) + step(worldNormalDotNoise, _TopSpread) * _RimColor2.rgb * pow(rim2, _RimPower);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
