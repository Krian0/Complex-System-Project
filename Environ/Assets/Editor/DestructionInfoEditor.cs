using UnityEditor;
using UnityEngine;
using Environ.Info;
using Environ.Support.Enum.Destruct;

[CustomEditor(typeof(DestructionInfo))]
public class DestructionInfoEditor : Editor
{
    SerializedProperty condition;
    SerializedProperty destroy;
    SerializedProperty targetObject;

    SerializedProperty spawnObjectOnDestroy;
    SerializedProperty objectToSpawn;

    SerializedProperty searchTags;
    SerializedProperty limit;
    SerializedProperty damageTypes;

    SerializedProperty debugMode;

    GUIContent conditionGUIC = new GUIContent("Condition", "The condition that should be met to destroy the DestructionInfo target.");
    GUIContent destroyGUIC = new GUIContent("Destroy", "Is set to true when the target should be destroyed.");
    GUIContent targetGUIC = new GUIContent("Target Object", "The GameObject that will be destroyed.");
    GUIContent spawnObjectGUIC = new GUIContent("Spawn Object On Destroy", "When Enabled: Spawns a GameObject at the DestructionInfo targets position after the target is destroyed. \nWhen Disabled: No effect.");
    GUIContent objectToSpawnGUIC = new GUIContent("Object To Spawn", "The GameObject that will be spawned.");
    GUIContent searchTagsGUIC = new GUIContent("Tags", "");
    GUIContent limitGUIC = new GUIContent("Time Limit", "Upon this timer reaching 0, the DestructionInfo target will be destroyed.");
    GUIContent damageTypesGUIC = new GUIContent("Damage Types", "");
    GUIContent debugGUIC = new GUIContent("Debug Mode", "Shows hidden variables in the inspector for debugging purposes.");

    private void OnEnable()
    {
        condition = serializedObject.FindProperty("condition");
        destroy = serializedObject.FindProperty("destroy");
        targetObject = serializedObject.FindProperty("target");

        spawnObjectOnDestroy = serializedObject.FindProperty("spawnObjectOnDestroy");
        objectToSpawn = serializedObject.FindProperty("objectToSpawn");

        searchTags = serializedObject.FindProperty("searchTags");
        limit = serializedObject.FindProperty("limit");
        damageTypes = serializedObject.FindProperty("damageTypes");

        debugMode = serializedObject.FindProperty("debugMode");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(condition, conditionGUIC);

        if (condition.enumValueIndex == (int)DestroyCondition.TIMER_ZERO)
            EditorGUILayout.PropertyField(limit.FindPropertyRelative("maxTime"), limitGUIC);

        else if (condition.enumValueIndex == (int)DestroyCondition.EFFECT_DAMAGE_TYPE)
            EditorGUILayout.PropertyField(damageTypes, damageTypesGUIC, true);

        else if (condition.enumValueIndex != (int)DestroyCondition.ZERO_HITPOINTS)
            EditorGUILayout.PropertyField(searchTags, searchTagsGUIC, true);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(spawnObjectOnDestroy, spawnObjectGUIC);
        if (spawnObjectOnDestroy.boolValue)
            EditorGUILayout.PropertyField(objectToSpawn, objectToSpawnGUIC);

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

            EditorGUILayout.PropertyField(targetObject, targetGUIC);
            EditorGUILayout.PropertyField(destroy, destroyGUIC);
            if (condition.enumValueIndex == (int)DestroyCondition.TIMER_ZERO)
                EditorGUILayout.PropertyField(limit.FindPropertyRelative("timer"));

            EditorGUILayout.Space();

            if (EditorApplication.isPlaying)
                Repaint();
        }

        EditorGUI.indentLevel--;
    }
}
