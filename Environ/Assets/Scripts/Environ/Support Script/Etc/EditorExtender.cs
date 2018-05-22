#if UNITY_EDITOR
using UnityEditor;

public static class EditorExtender
{
    //Draws Default Inspector without the script field
    public static bool DrawCustomInspector(this Editor Inspector)
    {
        EditorGUI.BeginChangeCheck();
        Inspector.serializedObject.Update();

        SerializedProperty Iterator = Inspector.serializedObject.GetIterator();
        Iterator.NextVisible(true);
        while (Iterator.NextVisible(false))
            EditorGUILayout.PropertyField(Iterator, true);

        Inspector.serializedObject.ApplyModifiedProperties();
        return (EditorGUI.EndChangeCheck());
    }
}
#endif