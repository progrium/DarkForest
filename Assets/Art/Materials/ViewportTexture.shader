// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ViewportTexture" {
 
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0.5)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
   
    Category {
        /* Upgrade NOTE: commented out, possibly part of old style per-pixel lighting: Blend AppSrcAdd AppDstAdd */
        Fog { Color [_AddFog] }
       
        SubShader {
            Pass {
                Tags { "LightMode" = "Always" }
               
                CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uvproj)
#pragma exclude_renderers d3d11
                    #pragma vertex vert
                    #pragma fragment frag
                   
                    #include "UnityCG.cginc"
                   
                    struct v2f {
                        float4 pos : POSITION;
                        float4 uvproj:TEXCOORD0;
                    };
                   
                    float4 _MainTex_ST;
 
                    v2f vert(appdata_base v) {
                        v2f o;
                        o.pos = UnityObjectToClipPos( v.vertex );
                        o.uvproj.xy = TRANSFORM_TEX(o.pos, _MainTex);
                        o.uvproj.zw = o.pos.zw;
                        return o;
                    }
                   
                    uniform sampler2D _MainTex;
                    uniform float4 _Color;
                   
                    float4 frag(v2f i) : COLOR {
                        i.uvproj /= i.uvproj.w;
                        i.uvproj = (i.uvproj + 1) * 0.5;
                       
                        half4 color = tex2D(_MainTex,i.uvproj.xy);
                        return color*_Color;
                    }
                ENDCG
            }
        }
    }
}