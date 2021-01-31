using UnityEditor;

namespace Asteroids.Editor
{
    /// <summary>
    /// Custom editor for the Movable component
    /// </summary>
    [CustomEditor(typeof(Movable))]
    [CanEditMultipleObjects]
    public class MovableEditor : UnityEditor.Editor
    {
        SerializedProperty lookAxis;
        SerializedProperty speed;
        SerializedProperty motionMode;
        SerializedProperty bordersBehaviour;
        SerializedProperty bordersOffset;
        SerializedProperty thrustParticles;

        void OnEnable()
        {
            speed = serializedObject.FindProperty("speed");
            lookAxis = serializedObject.FindProperty("lookAxis");
            motionMode = serializedObject.FindProperty("motionMode");
            bordersBehaviour = serializedObject.FindProperty("bordersBehaviour");
            bordersOffset = serializedObject.FindProperty("bordersOffset");
            thrustParticles = serializedObject.FindProperty("thrustParticles");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(motionMode);
            switch (motionMode.enumValueIndex)
            {
                case 0: // Automatic
                {
                    EditorGUILayout.PropertyField(speed);
                    EditorGUILayout.PropertyField(lookAxis);
                    break;
                }
                case 1: // InputBased
                {
                    EditorGUILayout.PropertyField(speed);
                    EditorGUILayout.PropertyField(lookAxis);
                    EditorGUILayout.PropertyField(thrustParticles);
                    break;
                }
                case 2: // None
                {
                    break;
                }
            }
            
            EditorGUILayout.PropertyField(bordersBehaviour);
            EditorGUILayout.PropertyField(bordersOffset);

            serializedObject.ApplyModifiedProperties();
        }
    }
}