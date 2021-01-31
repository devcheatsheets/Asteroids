using UnityEngine;
using UnityEditor;

namespace Asteroids.Editor
{
    /// <summary>
    /// Custom editor for the Displayable component
    /// </summary>
    [CustomEditor(typeof(Displayable))]
    [CanEditMultipleObjects]
    public class DisplayableEditor : UnityEditor.Editor
    {
        SerializedProperty meshType;
        SerializedProperty width;
        SerializedProperty height;
        SerializedProperty radius;
        SerializedProperty numVertices;
        SerializedProperty messUp;
        SerializedProperty messUpRadius;
        SerializedProperty generateCollider;
        SerializedProperty material;
        SerializedProperty initOnStart;

        void OnEnable()
        {
            meshType = serializedObject.FindProperty("meshType");
            width = serializedObject.FindProperty("width");
            height = serializedObject.FindProperty("height");
            radius = serializedObject.FindProperty("radius");
            numVertices = serializedObject.FindProperty("numVertices");
            messUp = serializedObject.FindProperty("messUp");
            messUpRadius = serializedObject.FindProperty("messUpRadius");
            generateCollider = serializedObject.FindProperty("generateCollider");
            material = serializedObject.FindProperty("material");
            initOnStart = serializedObject.FindProperty("initOnStart");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            EditorGUILayout.PropertyField(meshType);
            if (meshType.enumValueIndex == 0 || meshType.enumValueIndex == 2)
            {
                EditorGUILayout.PropertyField(radius);
                if(meshType.enumValueIndex == 2)
                    EditorGUILayout.PropertyField(numVertices);
            }
            else if (meshType.enumValueIndex == 1)
            {
                EditorGUILayout.PropertyField(width);
                EditorGUILayout.PropertyField(height);
            }
            
            EditorGUILayout.PropertyField(messUp);
            if(messUp.boolValue)
                EditorGUILayout.PropertyField(messUpRadius);

            EditorGUILayout.PropertyField(generateCollider);
            EditorGUILayout.PropertyField(material);
            EditorGUILayout.PropertyField(initOnStart);

            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}