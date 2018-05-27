using UnityEditor;
using EnvironInfo;
using EnvironEnum.DamageEnum;
using UnityEngine;

[CustomEditor(typeof(DamageInfo))]
public class DamageInfoEditor : Editor
{
    SerializedProperty attackGap;
    SerializedProperty delay;
    SerializedProperty refreshDelay;

    SerializedProperty limitType;
    SerializedProperty limit;
    SerializedProperty refreshLimit;

    SerializedProperty debugMode;
    SerializedProperty canAttack;

    static public GUIContent[] GUIC = { new GUIContent("Attack Gap", "The time in seconds that should pass between each attack"),
                                        new GUIContent("Initial Attack Delay", "The time in seconds to delay the initial attack"),
                                        new GUIContent("Refresh Delay?", "Attack delay will reset if the Output already exists as an Effect on an Environ Object. False: Disable, True: Enable"),
                                        new GUIContent("Limit Type", "Controls when the damage should stop"),
                                        new GUIContent("", "Counts down based on Limit choice. Attacks can only be made when the Limit is above 0"),
                                        new GUIContent("Refresh Limit?", "Controls whether the Limit should reset if the Effect already exists on an object"),
                                        new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"),
                                        new GUIContent("Gap Timer"),
                                        new GUIContent("Delay Timer"),
                                        new GUIContent("Limit Tracker"),
                                        new GUIContent("Can Attack")};


    private void OnEnable()
    {
        attackGap = serializedObject.FindProperty("attackGap");
        delay = serializedObject.FindProperty("delay");
        refreshDelay = serializedObject.FindProperty("refreshDelay");

        limitType = serializedObject.FindProperty("limitType");
        limit = serializedObject.FindProperty("limit");
        refreshLimit = serializedObject.FindProperty("refreshLimit");

        debugMode = serializedObject.FindProperty("debugMode");
        canAttack = serializedObject.FindProperty("canAttack");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorExtender.DrawCustomInspector(this);
        EditorGUILayout.PropertyField(attackGap.FindPropertyRelative("maxTime"), GUIC[0]);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(delay.FindPropertyRelative("maxTime"), GUIC[1]);
        EditorGUILayout.PropertyField(refreshDelay, GUIC[2]);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(limitType, GUIC[3]);
        if (limitType.enumValueIndex != (int)DLimit.NO_LIMIT)
        {
            GUIC[4].text = GetLimitName(limitType.enumNames[limitType.enumValueIndex]);
            EditorGUILayout.PropertyField(limit.FindPropertyRelative("maxTime"), GUIC[4]);
            EditorGUILayout.PropertyField(refreshLimit, GUIC[5]);
        }


        GUILayout.Space(20);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(debugMode, GUIC[6]);
        if (debugMode.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(attackGap.FindPropertyRelative("timer"), GUIC[7]);
            EditorGUILayout.PropertyField(delay.FindPropertyRelative("timer"), GUIC[8]);

            if (limitType.enumValueIndex != (int)DLimit.NO_LIMIT)
                EditorGUILayout.PropertyField(limit.FindPropertyRelative("timer"), GUIC[9]);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(canAttack, GUIC[10]);
            //script.removeOutputFlag = EditorGUILayout.Toggle("Remove OutputFlag", script.removeOutputFlag);

            if (EditorApplication.isPlaying)
                Repaint();
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();       
    }

    private string GetLimitName(string s)
    {
        s = s.Replace("_LIMIT", "").ToLower();
        return char.ToUpper(s[0]) + s.Substring(1) + " Limit";
    }
}