using UnityEngine;
using UnityEditor;
using Environ.Main;

[CustomEditor(typeof(EnvironObject))]
public class EnvironObjectEditor : Editor
{
    SerializedProperty hitPointLimit;
    SerializedProperty hitPoints;

    SerializedProperty resistances;
    SerializedProperty appearance;
    SerializedProperty destruction;
    SerializedProperty tags;

    SerializedProperty output;
    SerializedProperty effects;

    bool debugMode;

    GUIContent debugGUIC = new GUIContent("Debug Mode", "Shows hidden variables in the inspector for debugging purposes.");

    private void OnEnable()
    {
        hitPointLimit = serializedObject.FindProperty("hitPointLimit");
        hitPoints = serializedObject.FindProperty("hitPoints");

        resistances = serializedObject.FindProperty("resistances");
        appearance = serializedObject.FindProperty("appearance");
        destruction = serializedObject.FindProperty("destruction");
        tags = serializedObject.FindProperty("tags");

        output = serializedObject.FindProperty("output");
        effects = serializedObject.FindProperty("effects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(hitPointLimit);
        EditorGUILayout.PropertyField(hitPoints);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(resistances);
        EditorGUILayout.PropertyField(appearance);
        EditorGUILayout.PropertyField(destruction);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(tags.FindPropertyRelative("objectTags"), true);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(output, true);

        ShowDebug();

        serializedObject.ApplyModifiedProperties();
    }

    public void ShowDebug()
    {
        GUILayout.Space(20);
        EditorGUI.indentLevel += 1;

        debugMode = EditorGUILayout.Toggle(debugGUIC, debugMode);
        if (debugMode)
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(effects, true);

            EditorGUILayout.Space();

            if (EditorApplication.isPlaying)
                Repaint();
        }
        EditorGUI.indentLevel -= 1;
    }
}