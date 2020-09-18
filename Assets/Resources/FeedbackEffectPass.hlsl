//
// Feedback custom pass shader
//
// This is not the best implementation of a feedback effect. There are some
// points to be improved.
//
// - The current implementation invokes FullScreenPass twice with the camera
//   buffer and the feedback buffer. Is it possible to use MRT to join them?
// - Should I use Z-Test instead of branching-by-depth?
//

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

Texture2D _FeedbackTexture;

float3 _Tint;
float4 _Xform;

float3x3 ConstructTransformMatrix()
{
    return float3x3(_Xform.y, -_Xform.x, _Xform.z,
                    _Xform.x,  _Xform.y, _Xform.w,
                           0,         0,        1);
}

float3 SampleFeedbackTexture(float2 uv)
{
#ifdef KINO_POINT_SAMPLER
    return SAMPLE_TEXTURE2D_X_LOD(_FeedbackTexture, s_point_clamp_sampler, uv, 0).rgb;
#else
    return SAMPLE_TEXTURE2D_X_LOD(_FeedbackTexture, s_trilinear_clamp_sampler, uv, 0).rgb;
#endif
}

float4 FullScreenPass(Varyings varyings) : SV_Target
{
    // Screen/UV coordinates
    uint2 pcs = varyings.positionCS.xy;
    float2 uv = (pcs + 0.5) / _ScreenSize.xy;

    // Feedback transform
    uv = mul(ConstructTransformMatrix(), float3(uv - 0.5, 1)).xy + 0.5;

    // Composition
    float3 c1 = LoadCameraColor(pcs);
    float3 c2 = SampleFeedbackTexture(uv) * _Tint;
    return float4(LoadCameraDepth(pcs) == 0 ? c2 : c1, 1);
}
