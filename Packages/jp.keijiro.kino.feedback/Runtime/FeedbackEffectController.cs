using UnityEngine;

namespace Kino {

public sealed class FeedbackEffectController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField, ColorUsage(false)] Color _tint = Color.white;
    [SerializeField] float _hueShift = 0;
    [SerializeField] float _offsetX = 0;
    [SerializeField] float _offsetY = 0;
    [SerializeField] float _rotation = 0;
    [SerializeField] float _scale = 1;
    [SerializeField] bool _filter = true;

    #endregion

    #region Public properties

    public Color tint
      { get => _tint; set => _tint = value; }

    public float hueShift
      { get => _hueShift; set => _hueShift = value; }

    public float offsetX
      { get => _offsetX; set => _offsetX = value; }

    public float offsetY
      { get => _offsetY; set => _offsetY = value; }

    public float rotation
      { get => _rotation; set => _rotation = value; }

    public float scale
      { get => _scale; set => _scale = value; }

    public bool filter
      { get => _filter; set => _filter = value; }

    #endregion

    #region Private members

    static int TintID = Shader.PropertyToID("_Tint");
    static int XformID = Shader.PropertyToID("_Xform");

    MaterialPropertyBlock _props;

    Color GetTintColor()
      => new Color(_tint.r, _tint.g, _tint.b, _hueShift);

    Vector4 GetTransformVector()
    {
        var angle = Mathf.Deg2Rad * -_rotation;
        return new Vector4
          (Mathf.Sin(angle), Mathf.Cos(angle), -offsetX, -_offsetY) / _scale;
    }

    #endregion

    #region Internal properties

    internal int PassNumber => _filter ? 1 : 0;

    internal MaterialPropertyBlock PropertyBlock => UpdatePropertyBlock();

    MaterialPropertyBlock UpdatePropertyBlock()
    {
        if (_props == null) _props = new MaterialPropertyBlock();
        _props.SetColor(TintID, GetTintColor());
        _props.SetVector(XformID, GetTransformVector());
        return _props;
    }

    #endregion
}

} // namespace Kino
