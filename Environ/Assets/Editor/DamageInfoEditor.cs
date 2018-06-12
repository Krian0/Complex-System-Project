using UnityEditor;
using UnityEngine;
using Environ.Info;
using Environ.Support.Enum.Damage;

[CustomEditor(typeof(DamageInfo))]
public class DamageInfoEditor : Editor
{
    SerializedProperty attackGap;
    SerializedProperty delay;
    SerializedProperty refreshDelay;

    SerializedProperty limitType;
    SerializedProperty limit;
    SerializedProperty refreshLimit;
    SerializedProperty removeOnLimitReached;

    SerializedProperty debugMode;
    SerializedProperty removeEffect;

    GUIContent attackGUIC = new GUIContent("Attack Gap", "The time in seconds that should pass between each attack");
    GUIContent delayGUIC = new GUIContent("Initial Attack Delay", "The time in seconds to delay the initial attack");
    GUIContent refreshDelayGUIC = new GUIContent("Refresh Delay?", "Attack delay will reset if the Output already exists as an Effect on an Environ Object. False: Disable, True: Enable");
    GUIContent limitGUIC = new GUIContent("Limit Type", "Controls when the damage should stop");
    GUIContent limitNumGUIC = new GUIContent("Limit Number", "Counts down based on Limit choice. Attacks can only be made when the Limit is above 0");
    GUIContent refreshlimitGUIC = new GUIContent("Refresh Limit?", "Controls whether the Limit should reset if the Effect already exists on an object");
    GUIContent removeOnLimitGUIC = new GUIContent("Remove on Limit Reached", "If enabled, will remove the Effect this Info is contained in on Limit reaching 0");
    GUIContent debugGUIC = new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes");
    GUIContent gapTimerGUIC = new GUIContent("Gap Timer");
    GUIContent delayTimerGUIC = new GUIContent("Delay Timer");
    GUIContent LimitTrackerGUIC = new GUIContent("Limit Tracker");


    private void OnEnable()
    {
        attackGap = serializedObject.FindProperty("attackGap");
        delay = serializedObject.FindProperty("delay");
        refreshDelay = serializedObject.FindProperty("refreshDelay");

        limitType = serializedObject.FindProperty("limitType");
        limit = serializedObject.FindProperty("limit");
        refreshLimit = serializedObject.FindProperty("refreshLimit");
        removeOnLimitReached = serializedObject.FindProperty("removeOnLimitReached");

        debugMode = serializedObject.FindProperty("debugMode");
        removeEffect = serializedObject.FindProperty("removeEffect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorExtender.DrawCustomInspector(this);
        EditorGUILayout.PropertyField(attackGap.FindPropertyRelative("maxTime"), attackGUIC);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(delay.FindPropertyRelative("maxTime"), delayGUIC);
        EditorGUILayout.PropertyField(refreshDelay, refreshDelayGUIC);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(limitType, limitGUIC);
        if (limitType.enumValueIndex != (int)DLimit.NO_LIMIT)
        {
            limitNumGUIC.text = GetLimitName(limitType.enumNames[limitType.enumValueIndex]);
            EditorGUILayout.PropertyField(limit.FindPropertyRelative("maxTime"), limitNumGUIC);
            EditorGUILayout.PropertyField(refreshLimit, refreshlimitGUIC);
            EditorGUILayout.PropertyField(removeOnLimitReached, removeOnLimitGUIC);
        }

        ShowDebug();       

        serializedObject.ApplyModifiedProperties();       
    }

    public void ShowDebug()
    {
        GUILayout.Space(20);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(debugMode, debugGUIC);
        if (debugMode.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(attackGap.FindPropertyRelative("timer"), gapTimerGUIC);
            EditorGUILayout.PropertyField(delay.FindPropertyRelative("timer"), delayTimerGUIC);

            if (limitType.enumValueIndex != (int)DLimit.NO_LIMIT)
            {
                EditorGUILayout.PropertyField(limit.FindPropertyRelative("timer"), LimitTrackerGUIC);
                EditorGUILayout.PropertyField(removeEffect);
            }

            EditorGUILayout.Space();

            if (EditorApplication.isPlaying)
                Repaint();
        }

        EditorGUI.indentLevel--;
    }

    private string GetLimitName(string s)
    {
        s = s.Replace("_LIMIT", "").ToLower();
        return char.ToUpper(s[0]) + s.Substring(1) + " Limit";
    }
}