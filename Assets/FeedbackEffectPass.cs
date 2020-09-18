using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Kino {

[System.Serializable]
public sealed class FeedbackEffectPass : CustomPass
{
    #region Editable attributes

    public FeedbackEffectController _controller = null;

    #endregion

    #region Private members

    const GraphicsFormat BufferFormat = GraphicsFormat.B10G11R11_UFloatPack32;

    Material _material;
    RTHandle _buffer;

    #endregion

    #region CustomPass implementation

    protected override void Setup
      (ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        _material = CoreUtils.CreateEngineMaterial("Hidden/Kino/Feedback2");
        _buffer = RTHandles.Alloc
          (Vector2.one, colorFormat:BufferFormat, name:"Feedback Buffer");
    }

    protected override void Execute
      (ScriptableRenderContext renderContext, CommandBuffer cmd,
       HDCamera hdCamera, CullingResults cullingResult)
    {
        if (_controller == null || !_controller.enabled) return;

        var pass = _controller.PassNumber;
        var props = _controller.PropertyBlock;

        // Feedback composition
        _material.SetTexture("_FeedbackTexture", _buffer);
        CoreUtils.DrawFullScreen(cmd, _material, props, pass);

        // Do it again to the feedback buffer.
        CoreUtils.SetRenderTarget(cmd, _buffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(cmd, _material, props, pass);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(_material);
        _buffer.Release();
    }

    #endregion
}

} // namespace Kino
