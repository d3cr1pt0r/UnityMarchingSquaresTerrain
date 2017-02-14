Shader "d3cr1pt0r/RGB_A"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_AlphaTex ("Alpha Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				fixed2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 mainTex = tex2D(_MainTex, i.uv).rgb;
				fixed alpha = tex2D(_AlphaTex, i.uv).a;

				return fixed4(mainTex, alpha);
			}
			ENDCG
		}
	}
}
