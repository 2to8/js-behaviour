Shader "Custom/Mask3D"
{
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent+1"
        }
        Pass
        {
            Blend Zero One
        }
    }
}