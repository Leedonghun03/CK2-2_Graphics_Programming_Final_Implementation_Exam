Shader "Custom/GS_WireframeCube"
{
    Properties
    {
        _Color ("Line Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Cull Off
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma target 4.0
            #pragma vertex   vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2g
            {
                float4 pos : SV_POSITION;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
            };

            fixed4 _Color;

            v2g vert (appdata v)
            {
                v2g o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(6)]
            void geom(triangle v2g IN[3], inout LineStream<g2f> lineStream)
            {
                g2f o;

                o.pos = IN[0].pos;
                lineStream.Append(o);
                o.pos = IN[1].pos;
                lineStream.Append(o);

                o.pos = IN[1].pos;
                lineStream.Append(o);
                o.pos = IN[2].pos;
                lineStream.Append(o);

                o.pos = IN[2].pos;
                lineStream.Append(o);
                o.pos = IN[0].pos;
                lineStream.Append(o);
            }

            fixed4 frag (g2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
    FallBack Off
}
