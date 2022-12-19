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

    RTHandle NewBuffer(string name)
      => RTHandles.Alloc(Vector2.one, useDynamicScale: true,
                         name: name, colorFormat: BufferFormat);

    Material _material;
    (RTHandle last, RTHandle next) _buffer;

    #endregion

    #region CustomPass implementation

    protected override void Setup
      (ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        _material = CoreUtils.CreateEngineMaterial("Hidden/Kino/Feedback2");
        _buffer = (NewBuffer("Feedback1"), NewBuffer("Feedback2"));
        CoreUtils.SetRenderTarget(cmd, _buffer.last, ClearFlag.Color);
        CoreUtils.SetRenderTarget(cmd, _buffer.next, ClearFlag.Color);
    }

    protected override void Execute(CustomPassContext ctx)
    {
        if (_controller == null || !_controller.enabled) return;

        var pass = _controller.PassNumber;
        var props = _controller.PropertyBlock;

        // Feedback composition (to the render target)
        _material.SetTexture("_FeedbackTexture", _buffer.last);
        CoreUtils.DrawFullScreen(ctx.cmd, _material, props, pass);

        // Feedback composition (to the feedback buffer)
        CoreUtils.SetRenderTarget(ctx.cmd, _buffer.next, ClearFlag.Color);
        CoreUtils.DrawFullScreen(ctx.cmd, _material, props, pass);

        // Buffer swap
        _buffer = (_buffer.Item2, _buffer.Item1);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(_material);
        _buffer.last.Release();
        _buffer.next.Release();
    }

    #endregion
}

} // namespace Kino
