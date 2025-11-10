Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "red" {}
        _Color("Tint", Color) = (1,1,1,1)
        _Fill("Fill", Range(0.0,1.0)) = 0.5
        _Speed("Speed", float) = 5
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
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
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Fill;
            float _Speed;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

                return color;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                

                // get scroll value
                float2 scroll = float2(0, frac(_Time.x * _Speed));

                // sample texture
                fixed4 col = tex2D(_MainTex, i.uv * - scroll);

                // discard if uv.y is below below cut value
                clip(step(i.uv.y, _Fill* _MainTex_ST.y) - 0.1);

                //colors
                fixed4 c = SampleSpriteTexture(i.uv) * i.color;
                c.rgb *= c.a;

                return col * _Color;

                // make un-animated part black
                //return col*step(i.uv.y, _Cut * _MainTex_ST.y);
            }
            ENDCG
        }
    }
}
