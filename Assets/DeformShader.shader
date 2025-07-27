Shader "Unlit/DeformShader"
{
    Properties
	{
		_MainTex ("Sprite", 2D) = "white" {}
		_ContactPos ("Contact Position",Vector) = (0.5,0.5,0,0)
		_DeformRadius ("Deform Radius", Float) = 0.2
		_DeformAmount ("Deform Amount",Float) = 0.05
	}
	
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off ZWrite Off
		Ligthing Off
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float2 _ContactPos;
			float _DeformRadius;
			float _DeformAmount;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float2 dir = i.uv - _ContactPos;
				float dist = length(dir);
				
				float deform = smootstep(_DeformRadius, 0.0, dist) * _DeformAmount;
				float2 newUV = i.uv - normalize(dir) * deform;
				
				fixed4 col = tex2D(_MainTex, newUV);
				return col;
			}
			ENDCG
		}
	}
}
