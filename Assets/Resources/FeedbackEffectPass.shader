Shader "Hidden/Kino/Feedback2"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            #define KINO_POINT_SAMPLER
            #include "FeedbackEffectPass.hlsl"
            ENDHLSL
        }
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            #define KINO_TRILINEAR_SAMPLER
            #include "FeedbackEffectPass.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}
