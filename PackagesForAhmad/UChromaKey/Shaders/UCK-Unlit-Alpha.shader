// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UChromaKey/Unlit transparent" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)	
	_CKCol ("ChromaKey Color", Color) = (1,1,1,1)	
	_Range ("Range", Range (0.0, 2.83)) = 0.01
	_HueRange ("Hue Range", Range (0.0, 5.0)) = 0.1
	_EdgeSharp ("Edge sharpness", Range (1.0, 20.0)) = 20.0
	_Opacity ("Opacity", Range (0.0, 1.0)) = 1.0
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fog 
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO 
			};

			sampler2D _UChromaKeyTex;			
			float4 _UChromaKeyTex_ST;
			
			fixed4 _Color;
			fixed4 _CKCol;
			fixed4 _NewColor;
			half _Range;
			half _HueRange;
			half _EdgeSharp;
			half _uvDefX;		
			half _uvCoefX;		
			half _uvDefY;		
			half _uvCoefY;
			half _Opacity;
			half4 _Crop;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _UChromaKeyTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			} 
			
			fixed4 frag (v2f i) : SV_Target
			{
				half2 nuv;
				nuv.x = (_uvDefX + _uvCoefX * i.texcoord.x);
				nuv.y = (_uvDefY + _uvCoefY * i.texcoord.y);
				
				fixed4 c;
				
				if (!(nuv.x > (1 - _Crop.y) || nuv.x < _Crop.x || nuv.y > (1 - _Crop.z) || nuv.y < _Crop.w))
				{
					c = tex2D(_UChromaKeyTex, nuv) * _Color;
					half hueDiff = abs(atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001) - atan2(1.73205 * (_CKCol.g - _CKCol.b), 2 * _CKCol.r - _CKCol.g - _CKCol.b + 0.001));
					c.a = (1 - saturate((1 - ((c.r - _CKCol.r)*(c.r - _CKCol.r) + (c.g - _CKCol.g)*(c.g - _CKCol.g) + (c.b - _CKCol.b)*(c.b - _CKCol.b)) / (_Range * _Range)) * _EdgeSharp)
									* saturate(1.0 - min(hueDiff,6.28319 - hueDiff)/(_HueRange * _HueRange)) * _EdgeSharp) * _Opacity;					
					c.a = saturate(c.a);
				}
				else
				{
					c = 0;
				}				
				UNITY_APPLY_FOG(i.fogCoord, col);
				return c;
			}
		ENDCG
	}
}

}