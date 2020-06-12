Shader "Hidden/UChromaKeyAlpha" 
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}		
	}

	SubShader
	{
		Pass
		{
		
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag		
		#pragma target 3.0
		
		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;		
		fixed4 _CKCol;		
		half _Range;
		half _HueRange;
		half _uvDefX;		
		half _uvCoefX;		
		half _uvDefY;		
		half _uvCoefY;
		half _Opacity;
		half _EdgeSharp;
		half4 _Crop;
		
		
		float4 frag(v2f_img i) : COLOR
		{
			half2 nuv = i.uv;
			nuv.x = nuv.x * _uvCoefX + _uvDefX;
			nuv.y = nuv.y * _uvCoefY + _uvDefY;
			fixed4 mc = tex2D(_MainTex, i.uv);			
			half alpha = mc.a;
			if (!(nuv.x > (1 - _Crop.y) || nuv.x < _Crop.x || nuv.y > (1 - _Crop.z) || nuv.y < _Crop.w))
			{										
				half hueDiff = abs(atan2(1.73205 * (mc.g - mc.b), 2 * mc.r - mc.g - mc.b + 0.001) - atan2(1.73205 * (_CKCol.g - _CKCol.b), 2 * _CKCol.r - _CKCol.g - _CKCol.b + 0.001));
				
				alpha = (1 - saturate((1 - ((mc.r - _CKCol.r)*(mc.r - _CKCol.r) + (mc.g - _CKCol.g)*(mc.g - _CKCol.g) + (mc.b - _CKCol.b)*(mc.b - _CKCol.b)) / (_Range * _Range)) * _EdgeSharp)
				                  * saturate(1.0 - min(hueDiff,6.28319 - hueDiff)/(_HueRange * _HueRange)) * _EdgeSharp) * _Opacity; 				
			}
			
			mc.a = alpha;
			
			return mc;
		}

		ENDCG
		} 
	}
}