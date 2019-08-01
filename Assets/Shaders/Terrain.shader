Shader "RiceTools/Terrain"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "" {}
        _OverlayTex ("Surface Texture", 2D) = "" {}
		_Direction ("Direction", Vector) = (0, 0, 0)
		_Intensity ("Intensity", Range(0, 1)) = 1
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv_Overlay : TEXCOORD1;
				float3 normal : NORMAL;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _OverlayTex;
            float4 _OverlayTex_ST;

            v2f vert (appdata_full v)
            {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv_Overlay = TRANSFORM_TEX(v.texcoord, _OverlayTex);
				o.normal = mul(unity_ObjectToWorld, v.normal);
				return o;
            }

			float3 _Direction;
			fixed _Intensity;

            fixed4 frag (v2f i) : COLOR
            {
				fixed dir = dot(normalize(i.normal), _Direction);

				if(dir < 1 - _Intensity)
				{
					dir = 0;
				}

				fixed4 tex1 = tex2D(_MainTex, i.uv);
				fixed4 tex2 = tex2D(_OverlayTex, i.uv_Overlay);

				if(i.uv.y < 0.5)
				return tex2;

				return tex1;
            }

            ENDCG
        }
    }
}
