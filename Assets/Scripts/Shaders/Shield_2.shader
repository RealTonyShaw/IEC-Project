Shader "Unlit/Shield_2"
{
    Properties
	{
		[HDR]_Color("Main Color",Color) = (1,1,1,1)
		_VertexScale("顶点缩放系数",range(0,1)) = 0
		_Attenuation("内部衰减系数",range(1,36)) = 8
 
		_ScaleTex("外围pass贴图",2D) = "while" {}
 
	}
 
	SubShader
	{
		Tags{ "RenderType"="Opaque" "Queue"="Transparent" "LightMode"="ForwardBase"}
        
		LOD 200
		pass
		{
			//blend srcalpha OneMinusSrcAlpha 
            Blend OneMinusDstColor One 
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
 
			float4 _Color;
			float _ScaleAttenuation;
			float _VertexScale;
			sampler2D _ScaleTex;
			float4 _ScaleTex_ST;
 
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
 
			struct v2f
			{
				float4 pos : POSITION;
				float4 vertex : TEXCOORD4;
				float3 normal : TEXCOORD5;
				float2 uv : TEXCOORD0;
			};
 
			v2f vert(a2v v)
			{
				v2f o;
				v.vertex.xyz += v.normal.xyz * _VertexScale;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vertex = v.vertex;
				o.normal = v.normal;
				o.uv = TRANSFORM_TEX(v.uv, _ScaleTex);
				return o;
			}
 
			fixed4 frag(v2f v): COLOR
			{
				float3 V = normalize(WorldSpaceViewDir(v.vertex)).xyz;
				float3 N = normalize(mul(unity_ObjectToWorld,v.vertex)).xyz;
				float e = 1 - saturate(dot(V,N));
				fixed4 color = fixed4 (1,1,1,0.1);
				fixed4 tex= tex2D(_ScaleTex,v.uv);
				fixed4 t_color = fixed4(0,0,0,1);
 
				t_color.a *= saturate((1 - 0.9));
				t_color.a = max(t_color.a * 10000,0.1);
				
				color += t_color;
				if(tex.r > 0.1 && e < 10)
				{
					discard;
				}
				color.a *= e;
				color.r = _Color.r;
				color.g = _Color.g;
				color.b = _Color.b;
				return color;
			}
			ENDCG
		}
		pass
		{
			blend srcalpha OneMinusSrcAlpha
            //Blend DstColor Zero
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
 
			float4 _Color;
			float _Attenuation;
			struct v2f
			{
				float4 pos : POSITION;
				float4 vertex : TEXCOORD4;
				float3 normal : TEXCOORD5;
			};
 
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vertex = v.vertex;
				o.normal = v.normal;
				return o;
			}
 
			fixed4 frag(v2f v): COLOR
			{
				float3 V = normalize(WorldSpaceViewDir(v.vertex)).xyz;
				float3 N = normalize(mul((float3x3)unity_ObjectToWorld,v.vertex)).xyz;
				float e = 1 - saturate(dot(V,N));
				fixed4 color = fixed4 (1,1,1,1);
				color *= pow(e,_Attenuation);
				color *= _Color;
				return color;
			}
			ENDCG
		}
	}
}
