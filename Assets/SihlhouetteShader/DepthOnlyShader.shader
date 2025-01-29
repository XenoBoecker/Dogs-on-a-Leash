Shader "Custom/DepthOnly"
{
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        Pass
        {
            ZWrite On  // Writes depth information
            ColorMask 0  // Does not write color, only depth
        }
    }
}
