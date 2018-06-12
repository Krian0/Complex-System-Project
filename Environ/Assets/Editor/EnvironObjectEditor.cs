using UnityEngine;
using UnityEditor;
using Environ.Main;

[CustomEditor(typeof(EnvironObject))]
public class EnvironObjectEditor : Editor
{
    [HideInInspector]
    public bool debugMode;

    public override void OnInspectorGUI()
    {
        EnvironObject script = (EnvironObject)target;
        EditorExtender.DrawCustomInspector(this);

        GUILayout.Space(20);
        EditorGUI.indentLevel += 1;
        debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), debugMode);
        GUILayout.Space(20);
        if (debugMode)
        {
            int i = 0;
            foreach (EnvironOutput eo in script.output)
            {
                i++;
                EditorGUILayout.LabelField("Output " + i + " Unique ID: " + eo.uniqueID);
                eo.uniqueID = EditorGUILayout.TextField(eo.uniqueID);
            }

            if (EditorApplication.isPlaying)
                Repaint();
        }
        EditorGUI.indentLevel -= 1;
    }
}