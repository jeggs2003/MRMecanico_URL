Shader "Custom/ClippingShader"
{
    Properties
    {
        _Color      ("Color", Color) = (1,1,1,1)
        _MainTex    ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic   ("Metallic", Range(0,1)) = 0.0
        _ClipPlane  ("Clip Plane", Vector) = (0,1,0,0)
    }

    SubShader
    {
        // Desactiva el culling para ver el interior al cortar
        Cull Off

        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;    // Posición en el mundo para el corte
        };

        half  _Glossiness;
        half  _Metallic;
        fixed4 _Color;
        float4 _ClipPlane;      // XYZ = normal del plano, W = distancia

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // ── Clipping: descarta fragmentos debajo del plano ──
            float dist = dot(_ClipPlane.xyz, IN.worldPos) + _ClipPlane.w;
            clip(dist);         // clip() descarta si dist < 0

            // ── Renderizado normal ──
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo      = c.rgb;
            o.Metallic    = _Metallic;
            o.Smoothness  = _Glossiness;
            o.Alpha       = c.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}