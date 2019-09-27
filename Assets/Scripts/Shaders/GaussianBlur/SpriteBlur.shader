Shader "UI/Sprite Blur" {
    Properties {
    	_MainTex ("Main Texture", 2D) = "white" {}
        _Size ("Size", Range(0, 10)) = 5.0

        [HideInInspector]
		_StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector]
		_Stencil ("Stencil ID", Float) = 0
		[HideInInspector]
		_StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector]
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector]
		_StencilReadMask ("Stencil Read Mask", Float) = 255

    }
 
    Category {
    	
        Tags { 
        	"Queue"="Transparent" 
        	"IgnoreProjector"="True" 
        	"RenderType"="Opaque" 
        }

		Cull Off
		
		Stencil {
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

        SubShader {

            GrabPass {                
            	"_SpriteBlurTextureV"
            }
            
            // Vertical
            Pass {  
                Name "VERTICAL"
                Tags { "LightMode" = "Always" }
               
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment fragY
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
                #include "BlurCG.cginc"
                ENDCG
            }

            GrabPass {        
            	"_SpriteBlurTextureH"
            }

            // Horizontal
            Pass {
                Name "HORIZONTAL"
            
                Tags { "LightMode" = "Always" }
               
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment fragX
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
                #include "BlurCG.cginc"
                ENDCG
            }

        }

    }

}