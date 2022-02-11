Shader "HOBODOG/BreakableShineShader"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (1,1,1,1)
        _Emission("Emission", Color) = (0,0,0,1)
        _ShadowColor("Shadow color", Color) = (1,1,1,1)
        _Brightness("Shadow Brightness", Range(0,1)) = 0.3
        _Strength("Light Strength", Range(0,1)) = 0.5
        _Detail("Detail/Banding", Range(0,1)) = 0.3
        _ShineDuration("Shine Duration", Range(0,5)) = 1.3
        _ShineInterval("Shine Interval", Range(1, 10)) = 3.14
        _ShineSmooth("Shine Detail/Banding", Range(0,1)) = 0.8

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
            #pragma multi_compile_fwdbase
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #include "AutoLight.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 worldPos : TEXCOORD2;
                SHADOW_COORDS(3)
                float3 viewDir : TEXCOORD4;
                float4 pos : SV_POSITION;
                half3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Brightness;
            float _Strength;
            float4 _Color;
            float4  _Emission;
            float4 _ShadowColor;
            float _Detail;
            float _ShineDuration;
            float _ShineInterval;
            float _ShineSmooth;

            
            
            float Toon(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));

                return floor(NdotL / _Detail);                
            }

            float invLerp(float a, float b, float v)
            {
                return (v - a)/(b - a);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 tex = tex2D(_MainTex, i.uv);
                
                UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);

                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                float3 halfVector = normalize(_WorldSpaceLightPos0 * viewDir);

                float shineProg = invLerp(1-(_ShineDuration*(1/_ShineInterval)), 1, frac(_Time.y * (1/_ShineInterval)));
                
                float speed = saturate(shineProg);
                
                //speed *= shineON;

                
                
                float shine = floor(frac((normalize(normal) + normalize(halfVector) + speed) )/_ShineSmooth) ;
                
                fixed4 light = Toon(i.worldNormal, _WorldSpaceLightPos0.xyz ) * _LightColor0;
                
                fixed4 shadow = saturate(_Brightness * _ShadowColor);
                
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return _Color * tex *_Strength * (light* attenuation + shadow + shine  ) + _Emission;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
