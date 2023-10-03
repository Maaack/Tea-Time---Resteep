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
                float2 coord = i.uv - tex2D(_MainTex, i.uv).rg;
                half4 finalColor = tex2D(_MainTex, coord);
                if (hasBrush(i.uv)) {
                    finalColor = lerp(finalColor, _BrushColor, getBrushAlpha(i.uv));
                }
                return finalColor;
            }
            ENDCG
        }
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
            float viscosityScale = 64.0;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            float2 Jacobi(float2 uv, float2 texelSize, sampler2D velocityTexture, sampler2D b) {
                float2 xOffset = float2(texelSize.x, 0.0);
                float2 yOffset = float2(0.0, texelSize.y);

                float2 xl = tex2D(velocityTexture, uv - xOffset).xy;
                float2 xr = tex2D(velocityTexture, uv + xOffset).xy;
                float2 xb = tex2D(velocityTexture, uv - yOffset).xy;
                float2 xt = tex2D(velocityTexture, uv + yOffset).xy;

                float2 bc = tex2D(b, uv).xy;

                return (xl + xr + xb + xt + viscosityScale * bc) / (4.0 + viscosityScale);
            }

            sampler2D _MainTex;
            float2 _MainTex_TexelSize;

            half4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                float2 texelSize = 1.0 / _ScreenParams.zw; // Texel size

                float2 velocity;
                if (viscosityScale > 0.0) {
                    velocity = Jacobi(uv, texelSize, _MainTex, _MainTex);
                } else {
                    velocity = tex2D(_MainTex, uv);
                }

                return float4(velocity, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
