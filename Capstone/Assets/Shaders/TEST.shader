Shader "Unlit/TEST"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5, 0.5, 0.5, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline color", Color) = (0, 0, 0, 1)
		_OutlineWidth("Outline width", Range(0.0, 2.0)) = 0.1
	}

		CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : POSITION;
			float3 normal : NORMAL;

		};

		float _OutlineWidth;
		float _OutlineColor;

		v2f vert(appdata v)
		{
			v.vertex.xyz += v.normal.xyz * _OutlineWidth;

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}
		ENDCG

	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass // Render the Outline
		{
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// get vertex, return color
			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}
			ENDCG
		}

		Pass // Normal render
		{
			ZWrite On

			Material
			{
				Diffuse [_Color]
				Ambient [_Color]
			}

			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}
		}
	}
}
