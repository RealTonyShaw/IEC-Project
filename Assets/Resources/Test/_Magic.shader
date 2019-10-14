Shader "ShaderTest/_Magic"
{
	properties
	{
		[HDR]_MainColor("Main Color",color) = (1,1,1,1)
		_MainTex("Main Texcoord",2D) = "while" {}
		_StarTex("Star Texcoord",2D) = "while" {}
		_Discard("Discard Value",range(0,9)) = 0
		_NormalScale("Noraml Scale",range(1,2)) = 0.1
	}
 
	SubShader
	{
		Tags{"RenderType"="Opaque" "Queue"="Transparent"}
		LOD 200
		pass
		{
			blend SrcAlpha one
			ZWrite off
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			struct v2f
			{
				float4 pos : POSITION;
				fixed4 uv : TEXCOORD0;
			};
 
			sampler2D _MainTex,_StarTex;
			float4 _MainTex_ST,_StarTex_ST;
			float4 _MainColor;
			float _Discard;
			float _NormalScale;
 
			v2f vert(appdata_full v)
			{
				v2f o;
				v.vertex += v.vertex * _NormalScale;
				float timer = _Time.y;
				v.vertex *= float4(_NormalScale,_NormalScale,_NormalScale,1);
				float4x4 rotate = float4x4
				(
					cos(timer),0,sin(timer),0,
					0,1,0,0,
					-sin(timer),0,cos(timer),0,
					0,0,0,1
				);
				//float4x4 scale = float4x4
				//(
				//	_NormalScale,0,0,0,
				//	0,_NormalScale,0,0,
				//	0,0,_NormalScale,0,
				//	0,0,0,1
				//);
				float4x4 MVP = UNITY_MATRIX_MVP;
				//v.vertex = mul(scale,v.vertex);
				rotate = mul(MVP,rotate);
				o.pos = mul(rotate,v.vertex);
				
 
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy * _StarTex_ST.xy - _Time.x ;
				return o;
			}
 
			fixed4 frag(v2f v) : COLOR
			{
				fixed4 color = tex2D(_MainTex,v.uv.xy);
				fixed4 star = tex2D(_StarTex,v.uv.zw);
				float star_c = (star.r + star.g + star.b);
				color += star;
				color *= _MainColor;
				float c = color.r + color.g + color.b;
				float2 uv = v.uv;
				if(uv.x > 0.8 || uv.x < 0.2 || uv.y > 0.8 || uv.y < 0.2 || c < _Discard)
				{
					discard;
				}
				return color;
			}
			ENDCG
		}
 
		pass
		{
			blend SrcAlpha SrcAlpha
			ZWrite off
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			struct v2f
			{
				float4 pos : POSITION;
				fixed2 uv : TEXCOORD0;
			};
 
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainColor;
			float _Discard;
 
			v2f vert(appdata_full v)
			{
				v2f o;
				float timer = -_Time.y;
				float4x4 rotate = float4x4
				(
					cos(timer),0,sin(timer),0,
					0,1,0,0,
					-sin(timer),0,cos(timer),0,
					0,0,0,1
				);
				float4x4 MVP = UNITY_MATRIX_MVP;
				rotate = mul(MVP,rotate);
 
				o.pos = mul(rotate,v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
 
			fixed4 frag(v2f v) : COLOR
			{
				fixed4 color = tex2D(_MainTex,v.uv);
				color *= _MainColor;
				float c = color.r + color.g + color.b;
				float2 uv = v.uv;
				if(uv.x > 0.8 || uv.x < 0.2 || uv.y > 0.8 || uv.y < 0.2 || c < _Discard)
				{
					discard;
				}
				color.a = _MainColor.a;
				return color;
			}
 
			ENDCG
		}
	}
 
}