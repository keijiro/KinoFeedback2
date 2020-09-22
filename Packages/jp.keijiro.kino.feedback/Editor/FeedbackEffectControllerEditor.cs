using UnityEditor;

namespace Kino {

[CanEditMultipleObjects, CustomEditor(typeof(FeedbackEffectController))]
sealed class FeedbackEffectControllerEditor : Editor
{
    SerializedProperty _tint;
    SerializedProperty _hueShift;
    SerializedProperty _offsetX;
    SerializedProperty _offsetY;
    SerializedProperty _rotation;
    SerializedProperty _scale;
    SerializedProperty _filter;

    void OnEnable()
    {
        _tint = serializedObject.FindProperty("_tint");
        _hueShift = serializedObject.FindProperty("_hueShift");
        _offsetX = serializedObject.FindProperty("_offsetX");
        _offsetY = serializedObject.FindProperty("_offsetY");
        _rotation = serializedObject.FindProperty("_rotation");
        _scale = serializedObject.FindProperty("_scale");
        _filter = serializedObject.FindProperty("_filter");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_tint);
        EditorGUILayout.PropertyField(_hueShift);
        EditorGUILayout.PropertyField(_offsetX);
        EditorGUILayout.PropertyField(_offsetY);
        EditorGUILayout.PropertyField(_rotation);
        EditorGUILayout.PropertyField(_scale);
        EditorGUILayout.PropertyField(_filter);
        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Kino
