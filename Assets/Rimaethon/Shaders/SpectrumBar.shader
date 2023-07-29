Shader "Custom/SpectrumBar"
{
     Properties
    {
        _GridColor("Grid Color", Color) = (1, 1, 1, 1)
        _BaseColor("Base Color", Color) = (0, 0, 0, 1)
        _LineWidth("Line Width", float) = 0.05
        _GridSpacing("Grid Spacing", float) = 0.1
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Geometry" }
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        float _LineWidth;
        float _GridSpacing;
        fixed4 _GridColor;
        fixed4 _BaseColor;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 gridPosition = fmod(IN.uv_MainTex, _GridSpacing);
            bool isLine = gridPosition.x < _LineWidth || gridPosition.y < _LineWidth;
            fixed4 color = isLine ? _GridColor : _BaseColor;

            o.Albedo = color.rgb;
            o.Metallic = 0;
            o.Smoothness = 0.5;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
