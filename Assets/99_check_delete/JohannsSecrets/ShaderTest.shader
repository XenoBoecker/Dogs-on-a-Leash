Shader "Fix/ShaderTest"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Speed("Speed", Range(0, 5.0)) = 1
        _Frequency("Frequency", Range(0, 1.3)) = 1
        _Amplitude("Amplitude", Range(0, 5.0)) = 1
        _Rotation("Rotation", Range(0, 360)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Speed;
            float _Frequency;
            float _Amplitude;
            float _Rotation;

            v2f vert(appdata v)
            {
                v2f o;
                float waveY = cos(v.vertex.x * 10.0 + _Time.y * _Speed) * _Amplitude;
                v.vertex.y += waveY;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Apply rotation to UV coordinates
                float rad = radians(_Rotation);
                float2x2 rotationMatrix = float2x2(cos(rad), -sin(rad), sin(rad), cos(rad));
                o.uv = mul(rotationMatrix, v.uv);
                o.uv = TRANSFORM_TEX(o.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}