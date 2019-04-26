

Shader "PlayerHealth"
{
	Properties{
		_Offset("Time", Range(0, 1)) = 0.0
		_Color("Tint (RGB)", Color) = (1,1,1,1)
		_SurfaceTex("Texture (RGB)", 2D) = "Red" {}
		
	}
		SubShader{
			ZWrite Off
			Tags { "Queue" = "Transparent" }
			Blend One One
			Cull Off

			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				//#pragma fragmentoption ARB_fog_exp2

				#include "UnityCG.cginc" 


				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
					float3 normal : TEXCOORD2;
				};

				uniform float _Offset;

				v2f vert(appdata_base v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);

					float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
					v.texcoord.x = v.texcoord.x + _Offset;
					v.texcoord.y = v.texcoord.y;
					o.uv = TRANSFORM_UV(1);
					o.uv2 = float2(abs(dot(viewDir, v.normal)), 0.5);
					o.normal = v.normal;
					return o;
				}

				float4 _Color;
				
				uniform sampler2D _SurfaceTex : register(s0);

				fixed4 frag(v2f i) : COLOR
				{
					// sample the texture
					fixed4 col = tex2D(_SurfaceTex, i.uv) * _Color;
					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG

			}
	}
		
}