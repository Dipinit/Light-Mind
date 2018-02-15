#if UNITY_EDITOR

using UnityEditor;

namespace Assets.Scripts.Utilities
{
    [CustomEditor(typeof(RayColor))]
    [CanEditMultipleObjects]
    public class RayColorEditor : Editor
    {
        SerializedProperty r;
        SerializedProperty g;
        SerializedProperty b;
    
        void OnEnable()
        {
            r = serializedObject.FindProperty("R");
            g = serializedObject.FindProperty("G");
            b = serializedObject.FindProperty("B");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(r);
            EditorGUILayout.PropertyField(g);
            EditorGUILayout.PropertyField(b);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif