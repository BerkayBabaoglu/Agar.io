Shader "Custom/DeformShader"
{
    Properties
	{
		_MainTex ("Sprite", 2D) = "white" {}
		_ContactPos ("Contact Position",Vector) = (0.5,0.5,0,0)
		_DeformRadius ("Deform Radius", Float) = 0.2
		_DeformAmount ("Deform Amount",Float) = 0.05
		_DeformPos2 ("Contact Position 2",Vector) = (0.5,0.5,0,0)
		_DeformPos3 ("Contact Position 3",Vector) = (0.5,0.5,0,0)
		_FluidNoise ("Fluid Noise", 2D) = "white" {}
		_FluidSpeed ("Fluid Speed", Float) = 1.0
		_FluidStrength ("Fluid Strength", Float) = 0.02
		_Elasticity ("Elasticity", Range(0.1, 2.0)) = 1.0
	}
	
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" "PreviewType"="Plane"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off ZWrite Off
		Lighting Off
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _FluidNoise;
			
			float2 _ContactPos;
			float2 _DeformPos2;
			float2 _DeformPos3;
			float _DeformRadius;
			float _DeformAmount;
			float _FluidSpeed;
			float _FluidStrength;
			float _Elasticity;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};
			
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			float2 ApplyDeform(float2 uv, float2 contactPos, float radius, float amount)
			{
				float2 dir = uv - contactPos;
				float dist = length(dir);
				
				if (dist < radius)
				{
					// Daha yumuşak deformasyon eğrisi
					float normalizedDist = dist / radius;
					float smoothDeform = smoothstep(1.0, 0.0, normalizedDist);
					smoothDeform = smoothDeform * smoothDeform; // Daha yumuşak geçiş
					
					float deform = smoothDeform * amount * _Elasticity;
					return uv - normalize(dir) * deform;
				}
				return uv;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				
				// Ana deformasyon
				uv = ApplyDeform(uv, _ContactPos, _DeformRadius, _DeformAmount);
				uv = ApplyDeform(uv, _DeformPos2, _DeformRadius * 0.8, _DeformAmount * 0.6);
				uv = ApplyDeform(uv, _DeformPos3, _DeformRadius * 0.6, _DeformAmount * 0.4);
				
				// Sıvı efekti
				float2 fluidUV = uv + _Time.y * _FluidSpeed;
				float noise = tex2D(_FluidNoise, fluidUV).r;
				uv += (noise - 0.5) * _FluidStrength;
				
				// UV sınırlarını kontrol et
				uv = clamp(uv, 0.0, 1.0);
				
				fixed4 col = tex2D(_MainTex, uv) * i.color;
				return col;
			}
			ENDCG
		}
	}
	
	Fallback "Sprites/Default"
}
