// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#define GRABPIXELX(weight, kernelx) tex2Dproj(_SpriteBlurTextureH, UNITY_PROJ_COORD(half4(i.uvgrab.x + _SpriteBlurTextureH_TexelSize.x * kernelx * _Size * i.color.a, i.uvgrab.y, i.uvgrab.z, i.uvgrab.w))) * weight
#define GRABPIXELY(weight, kernely) tex2Dproj(_SpriteBlurTextureV, UNITY_PROJ_COORD(half4(i.uvgrab.x, i.uvgrab.y + _SpriteBlurTextureV_TexelSize.y * kernely * _Size * i.color.a, i.uvgrab.z, i.uvgrab.w))) * weight

struct appdata_t {
    half4 vertex : POSITION;
    half2 texcoord: TEXCOORD0;
    half4 color : COLOR;
};

struct v2f {
    half4 vertex : POSITION;
    half4 uvgrab : TEXCOORD0;
    half2 uvmain : TEXCOORD1;
    half4 color : COLOR;
};

half4 _MainTex_ST;
sampler2D _SpriteBlurTextureV;
half4 _SpriteBlurTextureV_TexelSize;
sampler2D _SpriteBlurTextureH;
half4 _SpriteBlurTextureH_TexelSize;
half _Size;
sampler2D _MainTex;

v2f vert(appdata_t v) {
	
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);

    #if UNITY_UV_STARTS_AT_TOP
    float scale = -1.0;
    #else
    float scale = 1.0;
    #endif
    
	o.uvgrab.xy = (half2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
    #ifdef UNITY_HALF_TEXEL_OFFSET
	o.vertex.xy += (_ScreenParams.zw - 1.0) * half2(-1, 1);
	o.uvgrab.xy += half2(0.0005, 0.0008);
    #else
    #endif
    o.uvgrab.zw = o.vertex.zw;
    
    o.color = v.color;
    o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);
    
    return o;
    
}

half4 fragX(v2f i) : SV_Target {

    half4 sum = half4(0, 0, 0, 0);
	half4 tint = tex2D(_MainTex, i.uvmain);
    //half4 col = tex2Dproj(_SpriteBlurTexture, UNITY_PROJ_COORD(i.uvgrab));
    half4 col = tex2Dproj(_SpriteBlurTextureH, i.uvgrab);
    
    sum += GRABPIXELX(0.05, -4.0);
    sum += GRABPIXELX(0.09, -3.0);
    sum += GRABPIXELX(0.12, -2.0);
    sum += GRABPIXELX(0.15, -1.0);
    sum += GRABPIXELX(0.18,  0.0);
    sum += GRABPIXELX(0.15, +1.0);
    sum += GRABPIXELX(0.12, +2.0);
    sum += GRABPIXELX(0.09, +3.0);
    sum += GRABPIXELX(0.05, +4.0);

    sum.rgb *= i.color.r;
    return lerp(col, sum, tint.a);
    
}
          
half4 fragY(v2f i) : SV_Target {
	
    half4 sum = half4(0, 0, 0, 0);
	half4 tint = tex2D(_MainTex, i.uvmain);
    //half4 col = tex2Dproj(_SpriteBlurTexture, UNITY_PROJ_COORD(i.uvgrab));
    half4 col = tex2Dproj(_SpriteBlurTextureV, i.uvgrab);
    
    sum += GRABPIXELY(0.05, -4.0);
    sum += GRABPIXELY(0.09, -3.0);
    sum += GRABPIXELY(0.12, -2.0);
    sum += GRABPIXELY(0.15, -1.0);
    sum += GRABPIXELY(0.18,  0.0);
    sum += GRABPIXELY(0.15, +1.0);
    sum += GRABPIXELY(0.12, +2.0);
    sum += GRABPIXELY(0.09, +3.0);
    sum += GRABPIXELY(0.05, +4.0);

    sum.rgb *= i.color.r;
    return lerp(col, sum, tint.a);
    
}
                