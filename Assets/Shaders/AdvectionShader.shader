Shader "Unlit/AdvectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushTexture ("Brush Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float2 _MainTex_TexelSize;
            sampler2D _BrushTexture;
            sampler2D _VelocityTexture;
            float deltaTime = 0.05;
            float4 _BrushColor;
            float2 _BrushCenterUV;
            float2 _BrushScale;
            bool _BrushOn = false;

            bool hasBrush(float2 uv) {
                return _BrushOn && abs(uv.x - _BrushCenterUV.x) < _BrushScale.x / 2 && abs(uv.y - _BrushCenterUV.y) < _BrushScale.y / 2;
            }

            float getBrushAlpha(float2 uv) {
                float2 brushOffset = float2(0.5, 0.5) * _BrushScale;
                float2 brushUV = ((brushOffset + _BrushCenterUV - uv) / _BrushScale);
                float brushAlpha = tex2D(_BrushTexture, brushUV).a;
                return brushAlpha * _BrushColor.a;
            }

            float4 bilerp(sampler2D inputSampler, float2 inputUV, float2 texelSize) {
                float2 st = inputUV / texelSize - float2(0.5, 0.5);
                float2 uvInt = floor(st);
                float2 uvFrac = frac(st);

                float4 a = tex2D(inputSampler, (uvInt + float2(0.5, 0.5)) * texelSize);
                float4 b = tex2D(inputSampler, (uvInt + float2(1.5, 0.5)) * texelSize);
                float4 c = tex2D(inputSampler, (uvInt + float2(0.5, 1.5)) * texelSize);
                float4 d = tex2D(inputSampler, (uvInt + float2(1.5, 1.5)) * texelSize);
                float4 abMix = lerp(a, b, uvFrac.x);
                float4 cdMix = lerp(c, d, uvFrac.x);
                return lerp(abMix, cdMix, uvFrac.y);
            }

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                float2 coord = i.uv - deltaTime * bilerp(_VelocityTexture, i.uv, _ScreenParams.zw).xy;
                half4 finalColor = bilerp(_MainTex, coord, _MainTex_TexelSize);
                if (hasBrush(i.uv)) {
                    finalColor = lerp(finalColor, _BrushColor, getBrushAlpha(i.uv));
                }

                return finalColor;
            }
            ENDCG
        }
    }
}
