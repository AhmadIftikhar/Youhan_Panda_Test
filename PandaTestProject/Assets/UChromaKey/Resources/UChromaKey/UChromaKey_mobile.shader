Shader "Hidden/UChromaKey_mobile" 
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
		half _Range;		
		half _uvDefX;
		half _uvCoefX;
		half _uvDefY;
		half _uvCoefY;
		half _opacity;
		half _smoothing;
		
		
		float4 frag(v2f_img i) : COLOR
		{			
			half2 nuv = i.uv;
			nuv.x = nuv.x * _uvCoefX + _uvDefX;
			nuv.y = nuv.y * _uvCoefY + _uvDefY;
			fixed4 c = tex2D(_UChromaKeyTex,nuv);
			fixed4 mc = tex2D(_MainTex, i.uv);		
			
			mc.rgb = lerp(lerp(mc.rgb, c.rgb,_opacity),mc.rgb,saturate((1.0 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range))*_smoothing));
			
			return mc;
		}

		ENDCG
		} 
	}
}