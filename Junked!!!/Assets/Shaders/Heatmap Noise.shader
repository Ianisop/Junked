Shader "Unlit/Heatmap Noise"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Deadzone ("Deadzone", float) = 0.25
        _Scale ("Scale", float) = 10.0
        _Offset ("Offset", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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

            float4 _Color;
            float _Scale;
            float4 _Offset;
            float _Deadzone;

            float fract(float f)
            {
                return f - floor(f);
            }

            float2 fract2(float2 f2)
            {
                return float2(fract(f2.x),fract(f2.y));
            }

            inline float unity_noise_randomValue (float2 uv)
            {
                return fract(sin(dot(uv, float2(12, 78))) * 430);
            }

            inline float unity_noise_interpolate (float a, float b, float t)
            {
                return (1.0f-t)*a + (t*b);
            }

            inline float unity_valueNoise (float2 uv)
            {
                float2 i = floor(uv);
                float2 fractUV = fract2(uv);
                float2 f = fract2(uv);
                f = f * f * float2(3 - (2 * f).x, 3 - (2 * f).y);

                float2 c0 = i + float2(0, 0);
                float2 c1 = i + float2(1, 0);
                float2 c2 = i + float2(0, 1);
                float2 c3 = i + float2(1, 1);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            float Unity_SimpleNoise_float(float2 UV, float Scale)
            {
                float t = 0.0f;

                float freq = 1;
                float amp = 0.125f;
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = 2;
                amp = 0.25f;
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = 4;
                amp = 0.5f;
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;


                float Out = t;
                return Out;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                i.uv = 1 - i.uv;

                float deadzone = saturate(distance(float2(0,0),(i.uv * 2) - 1) - _Deadzone);
                float f = 0;
                float2 youvee = float2(i.uv * 2) - 1;
                f = Unity_SimpleNoise_float(float2(_Offset.x + youvee.x, _Offset.y + youvee.y), _Scale);
                f -= 0.5f;
                f *= 10;
                float heat = (deadzone * f);
                return _Color * heat;
            }
            ENDCG
        }
    }
}
