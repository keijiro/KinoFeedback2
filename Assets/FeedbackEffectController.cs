using UnityEngine;

namespace Kino {

public sealed class FeedbackEffectController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Color _tint = Color.white;
    [SerializeField] float _offsetX = 0;
    [SerializeField] float _offsetY = 0;
    [SerializeField] float _rotation = 0;
    [SerializeField] float _scale = 1;
    [SerializeField] bool _filter = true;

    #endregion

    #region Public properties

    public Color tint
      { get => _tint; set => _tint = value; }

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
    static int OffsetID = Shader.PropertyToID("_Offset");
    static int RotationID = Shader.PropertyToID("_Rotation");
    static int ScaleID = Shader.PropertyToID("_Scale");

    MaterialPropertyBlock _props;

    // 2D rotation matrix as 4D vector
    Vector4 CalculateRotationMatrixAsVector()
    {
        var angle = -Mathf.Deg2Rad * _rotation;
        var sin = Mathf.Sin(angle);
        var cos = Mathf.Cos(angle);
        return new Vector4(cos, sin, -sin, cos);
    }

    #endregion

    #region Internal properties

    internal int PassNumber => _filter ? 1 : 0;

    internal MaterialPropertyBlock PropertyBlock => UpdatePropertyBlock();

    MaterialPropertyBlock UpdatePropertyBlock()
    {
        if (_props == null) _props = new MaterialPropertyBlock();

        _props.SetColor(TintID, _tint);
        _props.SetVector(OffsetID, new Vector2(_offsetX, _offsetY));
        _props.SetVector(RotationID, CalculateRotationMatrixAsVector());
        _props.SetFloat(ScaleID, _scale);

        return _props;
    }

    #endregion
}

} // namespace Kino
