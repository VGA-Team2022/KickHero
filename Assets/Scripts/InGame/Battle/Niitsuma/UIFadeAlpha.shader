Shader "Unlit/UIFadeAlpha"
{
	Properties
	{
		[PerRendererData] _MaskTex("Texture", 2D) = "white" {}
		[PerRendererData] _Color("Tint", Color) = (1,0,1,1)
		_Range("Range", Range(0, 1)) = 0
	}
		SubShader
		{
			Tags
			{
				"Queue" = "AlphaTest"
				"IgnoreProjector" = "True"
				"RenderType" = "TransparentCutout"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"


				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					half2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
				};

				fixed4 _Color;
				fixed4 _TextureSampleAdd;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.worldPosition = IN.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

					OUT.texcoord = IN.texcoord;

					#ifdef UNITY_HALF_TEXEL_OFFSET
					OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1,1);
					#endif

					OUT.color = IN.color * _Color;
					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _MaskTex;
				float _Range;

				fixed4 frag(v2f IN) : SV_Target
				{
					half4 color = _Color;
					half mask = tex2D(_MaskTex, IN.texcoord).a - (-1 + _Range * 2);
					color.a = mask;

					clip(mask - 0.001);

					return color;
				}
			ENDCG
		}
		}
}
