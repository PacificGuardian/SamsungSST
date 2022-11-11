Shader "Stencil/StencilCover"
{
	//Transparent Stencil
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

		_Color("Color (RGBA)", Color) = (0, 0, 0, 0)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" "Queue" = "Transparent+2"}//here set
			Stencil{
				Ref 5
				Comp Always
				Pass Replace
			}//here set
			//ZWrite Off//here set

			LOD 100

			Pass
			{

				Blend Zero One
				//Blend SrcAlpha OneMinusSrcAlpha//here set
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return fixed4(1,1,1,0.5);//here set
			}
			ENDCG
		}
		}
}
