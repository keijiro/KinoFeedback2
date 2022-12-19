// Feedback custom pass shader

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

TEXTURE2D_X(_FeedbackTexture);

float4 _Tint; // R, G, B, Hue shift
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
    return SAMPLE_TEXTURE2D_X_LOD(_FeedbackTexture, s_linear_clamp_sampler, uv, 0).rgb;
#endif
}

float3 ApplyTint(float3 rgb)
{
    rgb = RgbToHsv(rgb);
    rgb.x = frac(rgb.x + _Tint.a);
    rgb = HsvToRgb(rgb);
    rgb *= _Tint.rgb;
    return rgb;
}

float4 FullScreenPass(Varyings varyings) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);

    float3 c1 = LoadCameraColor(varyings.positionCS.xy);
    float depth = LoadCameraDepth(varyings.positionCS.xy);

    PositionInputs posInput =
      GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw,
                       depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);

    float2 uv = posInput.positionNDC.xy;

    // Feedback transform
    uv = mul(ConstructTransformMatrix(), float3(uv - 0.5, 1)).xy + 0.5;
    uv = saturate(uv) * _RTHandleScale.xy;

    // Composition
    float3 c2 = ApplyTint(SampleFeedbackTexture(uv));
    return float4(depth == 0 ? c2 : c1, 1);
}
