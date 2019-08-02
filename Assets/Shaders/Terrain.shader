Shader "RiceTools/Terrain"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "" {}
		_UVSET ("UV", int) = 0
		_Intensity ("Intensity", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

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
                float2 uv_Map : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv_Map : TEXCOORD1;
				float3 normal : NORMAL;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			int _UVSET;
			float _Intensity;

            v2f vert (appdata_full v)
            {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv_Map = TRANSFORM_TEX(v.texcoord2, _MainTex);
				o.normal = mul(unity_ObjectToWorld, v.normal);
				return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
				fixed4 tex1 = tex2D(_MainTex, i.uv);
				fixed4 tex2 = tex2D(_MainTex, i.uv_Map);

				if(_UVSET == 0)
				{
					if(i.uv.x == 0.5 && i.uv.y == 0.5)
					tex1.a = 0;

					return tex1;
				}
				else
				{
					return tex2;
				}
            }

            ENDCG
        }
    }
}
