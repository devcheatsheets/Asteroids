using UnityEditor;

namespace Asteroids.Editor
{
    /// <summary>
    /// Custom editor for the Rotateable component
    /// </summary>
    [CustomEditor(typeof(Rotateable))]
    [CanEditMultipleObjects]
    public class RotateableEditor : UnityEditor.Editor
    {
        SerializedProperty rotationSpeed;
        SerializedProperty lookAxis;
        SerializedProperty targetMode;
        SerializedProperty lookTarget;

        void OnEnable()
        {
            rotationSpeed = serializedObject.FindProperty("rotationSpeed");
            lookAxis = serializedObject.FindProperty("lookAxis");
            targetMode = serializedObject.FindProperty("targetMode");
            lookTarget = serializedObject.FindProperty("target");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(rotationSpeed);
            EditorGUILayout.PropertyField(lookAxis);
            EditorGUILayout.PropertyField(targetMode);
            if(targetMode.enumValueIndex == 1)
                EditorGUILayout.PropertyField(lookTarget);
            serializedObject.ApplyModifiedProperties();
        }
    }
}