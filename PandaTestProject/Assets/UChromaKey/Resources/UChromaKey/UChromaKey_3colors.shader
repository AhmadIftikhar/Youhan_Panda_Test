Shader "Hidden/UChromaKey_3colors" 
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
		uniform sampler2D _UChromaKeyTex;
		fixed4 _PatCol;
		fixed4 _PatCol2;
		fixed4 _PatCol3;
		half _Range;
		half _HueRange;
		half _uvDefX;		
		half _uvCoefX;		
		half _uvDefY;		
		half _uvCoefY;
		half _opacity;
		half _smoothing;
		half4 _Crop;
		
		
		float4 frag(v2f_img i) : COLOR
		{
			half2 nuv = i.uv;
			nuv.x = nuv.x * _uvCoefX + _uvDefX;
			nuv.y = nuv.y * _uvCoefY + _uvDefY;
			fixed4 mc = tex2D(_MainTex, i.uv);
			fixed4 c = tex2D(_UChromaKeyTex,nuv);
			
			if (!(nuv.x > (1 - _Crop.y) || nuv.x < _Crop.x || nuv.y > (1 - _Crop.z) || nuv.y < _Crop.w))
			{										
				half hueDiff = abs(atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001) - atan2(1.73205 * (_PatCol.g - _PatCol.b), 2 * _PatCol.r - _PatCol.g - _PatCol.b + 0.001));
				half hueDiff2 = abs(atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001) - atan2(1.73205 * (_PatCol2.g - _PatCol2.b), 2 * _PatCol2.r - _PatCol2.g - _PatCol2.b + 0.001));
				half hueDiff3 = abs(atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001) - atan2(1.73205 * (_PatCol3.g - _PatCol3.b), 2 * _PatCol3.r - _PatCol3.g - _PatCol3.b + 0.001));
				half coef1 = saturate((1.0 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range))*_smoothing)
								* saturate((1.0 - min(hueDiff,6.28319 - hueDiff)/(_HueRange * _HueRange))*_smoothing);
				half coef2 = saturate((1.0 - ((c.r - _PatCol2.r)*(c.r - _PatCol2.r) + (c.g - _PatCol2.g)*(c.g - _PatCol2.g) + (c.b - _PatCol2.b)*(c.b - _PatCol2.b)) / (_Range * _Range))*_smoothing)
								* saturate((1.0 - min(hueDiff2,6.28319 - hueDiff2)/(_HueRange * _HueRange))*_smoothing);
				half coef3 = saturate((1.0 - ((c.r - _PatCol3.r)*(c.r - _PatCol3.r) + (c.g - _PatCol3.g)*(c.g - _PatCol3.g) + (c.b - _PatCol3.b)*(c.b - _PatCol3.b)) / (_Range * _Range))*_smoothing)
								* saturate((1.0 - min(hueDiff3,6.28319 - hueDiff3)/(_HueRange * _HueRange))*_smoothing);
			
				mc.rgb = lerp(lerp(mc.rgb, c.rgb,_opacity),mc.rgb,
								coef1 + coef2 + coef3);
			}
			
			return mc;
			
		}

		ENDCG
		} 
	}
}