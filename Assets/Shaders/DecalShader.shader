// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DecalShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        //Tags { "Queue" = "Geometry+1" "RenderType" = "Transparent" }
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        //Blend Zero SrcColor
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off Lighting On ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            struct v2f {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                //half4 prev = _Color * tex2D(_MainTex, i.uv);
                //fixed4 col = lerp(half4(1,1,1,1), prev, prev.a);
                //return col;

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
