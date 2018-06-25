using UnityEngine;
using UnityEditor;
using Environ.Main;
using Environ.Support.Enum.General;

[CustomEditor(typeof(EnvironOutput))]
public class EnvironOutputEditor : Editor
{
    [HideInInspector] public bool debugMode;
    [HideInInspector] public bool damageIFold = false;
    [HideInInspector] public bool appearanceIFold = false;


    SerializedProperty similarity;
    SerializedProperty selectiveCanBeSimilar;
    SerializedProperty damageSimilarity;
    SerializedProperty appearanceSimilarity;

    SerializedProperty transferCondition;
    SerializedProperty allowTransmission;
    SerializedProperty endOnCondition;
    SerializedProperty limit;
    SerializedProperty refreshTimer;


    SerializedProperty firstSourceEO;
    SerializedProperty lastSourceEO;
    SerializedProperty targetEO;

    SerializedProperty damageI;
    SerializedProperty appearanceI;

    SerializedProperty ID;

    static string similarityString =
        "Used to determine when an Effect does or does not exist (is a unique instance) in another Environ Object's Effects list. \n\n" +
        "Unique:      Each original Output ScriptableObject and each clone is a unique instance. \n\n" +
        "Standard:   Each original Output ScriptableObject is a unique instance that their clones share. \n\n" +
        "Selective:   Will be considered the same as any Output with Info that originates from selected Scriptable Objects.";

    static GUIContent dSimilarityGUIC = new GUIContent("Damage Info", "Input the exact Damage Info ScriptableObject you want this Output to be similar to.");
    static GUIContent aSimilarityGUIC = new GUIContent("Appearance Info", "Input the exact Appearance Info ScriptableObject you want this Output to be similar to.");
    static GUIContent sCanBeSimilarGUIC = new GUIContent("Selective Can Be Similar?", "If enabled, this Environ Output will be considered the same as any with the given Info. If disabled, Selective Outputs cannot be similar to this one.");
    static GUIContent transferGUIC = new GUIContent("Transfer Condition", "When this condition is met the Output will be cloned and added as an Effect to the other Environ Object's Effects list, if it doesn't already exist in the list.");
    static GUIContent allowGUIC = new GUIContent("Allow Transmission?", "When True, this option allows the Effect itself to be cloned and added as an Effect on other Environ Objects. When False, only the Output can do so.");
    static GUIContent terminalGUIC = new GUIContent("Terminal Condition", "When this condition is met the Effect will be removed.");
    static GUIContent limitGUIC = new GUIContent("Time Limit", "The time in seconds before the Terminal Condition is met.");
    static GUIContent refreshGUIC = new GUIContent("Refresh Timer?", "When True, if this Effect exists in an Effects list upon a transfer attempt, the Time Limit will be reset.");
    static GUIContent similarityGUIC = new GUIContent("Similarity Index", similarityString);
    static GUIContent damageIGUIC = new GUIContent("Damage Info");
    static GUIContent appearanceIGUIC = new GUIContent("Appearance Info");
    static GUIContent debugGUIC = new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes");
    static GUIContent firstSourceGUIC = new GUIContent("First Source EO", "The original source for this Effect");
    static GUIContent lastSourceGUIC = new GUIContent("Last Source EO", "The most recent source for this Effect");
    static GUIContent targetGUIC = new GUIContent("Target EO", "The Environ Object this Effect is on");
    static GUIContent limitTimer = new GUIContent("Limit Timer");





    private void OnEnable()
    {
        similarity = serializedObject.FindProperty("similarity");
        selectiveCanBeSimilar = serializedObject.FindProperty("selectiveCanBeSimilar");
        damageSimilarity = serializedObject.FindProperty("damageSimilarity");
        appearanceSimilarity = serializedObject.FindProperty("appearanceSimilarity");

        transferCondition = serializedObject.FindProperty("transferCondition");
        allowTransmission = serializedObject.FindProperty("allowTransmission");
        endOnCondition = serializedObject.FindProperty("endOnCondition");
        limit = serializedObject.FindProperty("limit");

        refreshTimer = serializedObject.FindProperty("refreshTimer");

        firstSourceEO = serializedObject.FindProperty("firstSource"); 
        lastSourceEO = serializedObject.FindProperty("lastSource"); 
        targetEO = serializedObject.FindProperty("target");
    
        damageI = serializedObject.FindProperty("damageI");
        appearanceI = serializedObject.FindProperty("appearanceI");

        ID = serializedObject.FindProperty("uniqueID");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();


        EditorGUILayout.PropertyField(similarity, similarityGUIC);

        if (similarity.enumValueIndex == (int)Similarity.SELECTIVE)
        {
            selectiveCanBeSimilar.boolValue = true;
            EditorGUILayout.PropertyField(damageSimilarity, dSimilarityGUIC);
            EditorGUILayout.PropertyField(appearanceSimilarity, aSimilarityGUIC);
        }

        else
            EditorGUILayout.PropertyField(selectiveCanBeSimilar, sCanBeSimilarGUIC);

        GUILayout.Space(20);
        
        EditorGUILayout.PropertyField(transferCondition, transferGUIC);
        EditorGUILayout.PropertyField(allowTransmission, allowGUIC);

        EditorGUILayout.PropertyField(endOnCondition, terminalGUIC);
        if (endOnCondition.enumValueIndex == (int)TerminalCondition.TIMER_ZERO)
        {
            EditorGUILayout.PropertyField(limit.FindPropertyRelative("maxTime"), limitGUIC);
            EditorGUILayout.PropertyField(refreshTimer, refreshGUIC);
        }

        GUILayout.Space(20);


        EditorGUILayout.PropertyField(damageI, damageIGUIC);
        if (damageI.objectReferenceValue != null)
        {
            EditorGUI.indentLevel += 2;

            damageIFold = EditorGUILayout.Foldout(damageIFold, "Extended View");
            if (damageIFold)
                CreateEditor(damageI.objectReferenceValue).OnInspectorGUI();

            EditorGUI.indentLevel -= 2;
            GUILayout.Space(20);
        }

        EditorGUILayout.PropertyField(appearanceI, appearanceIGUIC);
        if (appearanceI.objectReferenceValue != null)
        {
            EditorGUI.indentLevel += 2;

            appearanceIFold = EditorGUILayout.Foldout(appearanceIFold, "Extended View");
            if (appearanceIFold)
                CreateEditor(appearanceI.objectReferenceValue).OnInspectorGUI();

            EditorGUI.indentLevel -= 2;
            GUILayout.Space(20);
        }

        //script.destructionI = (destructionInfo)EditorGUILayout.ObjectField("Destruction Info", script.destructionI, typeof(destructionInfo), false);

        ShowDebug();


        serializedObject.ApplyModifiedProperties();
    }

    public void ShowDebug()
    {
        GUILayout.Space(20);
        EditorGUI.indentLevel += 1;

        debugMode = EditorGUILayout.Toggle(debugGUIC, debugMode);
        if (debugMode == true)
        {
            EditorGUILayout.Space();
            if (firstSourceEO.objectReferenceValue != null)
                EditorGUILayout.PropertyField(firstSourceEO, firstSourceGUIC);
            if (lastSourceEO.objectReferenceValue != null)
                EditorGUILayout.PropertyField(lastSourceEO, lastSourceGUIC);
            if (targetEO.objectReferenceValue != null)
                EditorGUILayout.PropertyField(targetEO, targetGUIC);
            EditorGUILayout.Space();

            if (endOnCondition.enumValueIndex == (int)TerminalCondition.TIMER_ZERO)
                EditorGUILayout.PropertyField(limit.FindPropertyRelative("timer"), limitTimer);
            GUILayout.Space(20);


            EditorGUILayout.SelectableLabel("Unique ID: " + ID.stringValue);

            if (EditorApplication.isPlaying)
                Repaint();
        }

        EditorGUI.indentLevel -= 1;
    }
}