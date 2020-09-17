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
    RTHandle _buffer1;
    RTHandle _buffer2;

    #endregion

    #region CustomPass implementation

    protected override void Setup
      (ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        _material = CoreUtils.CreateEngineMaterial("Hidden/Kino/Feedback2");
        _buffer1 = RTHandles.Alloc(Vector2.one, colorFormat:BufferFormat, name:"Feedback Buffer 1");
        _buffer2 = RTHandles.Alloc(Vector2.one, colorFormat:BufferFormat, name:"Feedback Buffer 2");
    }

    protected override void Execute
      (ScriptableRenderContext renderContext, CommandBuffer cmd,
       HDCamera hdCamera, CullingResults cullingResult)
    {
        if (_controller == null || !_controller.enabled) return;

        var pass = _controller.PassNumber;
        var props = _controller.PropertyBlock;

        // Feedback composition
        _material.SetTexture("_FeedbackTexture", _buffer1);
        CoreUtils.DrawFullScreen(cmd, _material, props, pass);

        // Do it again to the feedback buffer.
        CoreUtils.SetRenderTarget(cmd, _buffer2, ClearFlag.None);
        CoreUtils.DrawFullScreen(cmd, _material, props, pass);

        // Buffer swap
        (_buffer1, _buffer2) = (_buffer2, _buffer1);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(_material);
        _buffer1.Release();
        _buffer2.Release();
    }

    #endregion
}

} // namespace Kino
