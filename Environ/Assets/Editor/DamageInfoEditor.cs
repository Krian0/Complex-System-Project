using UnityEditor;
using UnityEngine;
using Environ.Info;
using Environ.Support.Enum.Damage;

[CustomEditor(typeof(DamageInfo))]
public class DamageInfoEditor : Editor
{
    SerializedProperty dynamicDamage;
    SerializedProperty attackGap;
    SerializedProperty delay;
    SerializedProperty refreshDelay;

    SerializedProperty limitType;
    SerializedProperty limit;
    SerializedProperty refreshLimit;
    SerializedProperty removeOnLimitReached;

    SerializedProperty debugMode;
    SerializedProperty removeEffect;

    GUIContent dynamicGUIC = new GUIContent("Dynamic Damage", "When Enabled: Damage will be calculated with Target resistance each attack. \nWhen Disabled: Damage will be calculated with Target resistance once upon the Effect being added to the Target.");
    GUIContent attackGUIC = new GUIContent("Attack Gap", "The time in seconds that should pass between each attack.");
    GUIContent delayGUIC = new GUIContent("Attack Delay", "The time in seconds to delay the initial attack.");
    GUIContent refreshDelayGUIC = new GUIContent("Refresh Delay", "When Enabled: If the Transfer Condition is met when this Effect exists on the target, the Attack Delay timer will be reset. \nWhen Disabled: No effect.");
    GUIContent limitGUIC = new GUIContent("Limit Type", "Controls the type of Limit for when the damage should stop.");
    GUIContent limitNumGUIC = new GUIContent("Limit Number", "Counts down based on Limit choice. Attacks can only be made when the Limit is above 0.");
    GUIContent refreshlimitGUIC = new GUIContent("Refresh Limit", "When Enabled: If the Transfer Condition is met when this Effect exists on the target, the Limit tracker will be reset. \nWhen Disabled: No effect.");
    GUIContent removeOnLimitGUIC = new GUIContent("Remove on Limit Reached", "When Enabled: Upon the Limit reaching 0, this Effect will be removed from the Target. \nWhen Disabled: No effect.");
    GUIContent debugGUIC = new GUIContent("Debug Mode", "Shows hidden variables in the inspector for debugging purposes.");
    GUIContent gapTimerGUIC = new GUIContent("Attack Gap Timer");
    GUIContent delayTimerGUIC = new GUIContent("Attack Delay Timer");
    GUIContent LimitTrackerGUIC = new GUIContent("Limit Tracker");


    private void OnEnable()
    {
        dynamicDamage = serializedObject.FindProperty("dynamicDamage");
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
        EditorGUILayout.PropertyField(dynamicDamage, dynamicGUIC);
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