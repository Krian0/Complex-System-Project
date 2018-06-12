using UnityEngine;
using UnityEditor;
using Environ.Main;
using Environ.Support.Enum.General;
using Environ.Info;

[CustomEditor(typeof(EnvironOutput))]
public class EnvironOutputEditor : Editor
{
    [HideInInspector] public bool debugMode;
    [HideInInspector] public GameObject firstSourceEO = null;
    [HideInInspector] public GameObject lastSourceEO = null;
    [HideInInspector] public GameObject targetEO = null;
    [HideInInspector] public bool damageIFold = false;
    [HideInInspector] public bool appearanceIFold = false;


    GUIContent transferGUIC = new GUIContent("Transfer Condition", "When this condition is met the Output will be cloned and added as an Effect to the other Environ Object's Effects list, if it doesn't already exist in the list.");
    GUIContent allowGUIC = new GUIContent("Allow Transmission?", "When True, this option allows the Effect itself to be cloned and added as an Effect on other Environ Objects. When False, only the Output can do so.");
    GUIContent terminalGUIC = new GUIContent("Terminal Condition", "When this condition is met the Effect will be removed.");
    GUIContent limitGUIC = new GUIContent("Time Limit", "The time in seconds before the Terminal Condition is met.");
    GUIContent refreshGUIC = new GUIContent("Refresh Timer?", "When True, if this Effect exists in an Effects list upon a transfer attempt, the Time Limit will be reset.");


    string similarityString =
        "Used to determine when an Effect does or does not exist (is a unique instance) in another Environ Object's Effects list. \n\n\n" +
        "Unique:     Each original Output ScriptableObject and each clone is a unique instance. \n\n" +
        "Standard:   Each original Output ScriptableObject is a unique instance that their clones share. \n\n" +
        "Selective:  Will be considered the same as any Output with Info that originates from selected Scriptable Objects.";


    public override void OnInspectorGUI()
    {
        EnvironOutput script = (EnvironOutput)target;
        EditorExtender.DrawCustomInspector(this);

        script.similarity = (Similarity)EditorGUILayout.EnumPopup(new GUIContent("Similarity Index", similarityString), script.similarity);
        if (script.similarity == Similarity.SELECTIVE)
        {
            script.selectiveCanBeSimilar = true;
            script.damageSimilarity = (DamageInfo)EditorGUILayout.ObjectField("Damage Info", script.damageI, typeof(DamageInfo), false);
            script.appearanceSimilarity = (AppearanceInfo)EditorGUILayout.ObjectField("Appearance Info", script.appearanceSimilarity, typeof(AppearanceInfo), false);
        }
        else
            script.selectiveCanBeSimilar = EditorGUILayout.Toggle("", script.selectiveCanBeSimilar);

        GUILayout.Space(20);

        script.transferCondition = (TransferCondition)EditorGUILayout.EnumPopup(transferGUIC, script.transferCondition);
        script.allowTransmission = EditorGUILayout.Toggle(allowGUIC, script.allowTransmission);

        if (script.transferCondition != TransferCondition.ON_COLLISION_STAY && script.transferCondition != TransferCondition.ON_TRIGGER_STAY)
        {
            script.endOnCondition = (TerminalCondition)EditorGUILayout.EnumPopup(terminalGUIC, script.endOnCondition);
            if (script.endOnCondition == TerminalCondition.ON_TIMER)
            {
                script.timeLimit = EditorGUILayout.FloatField(limitGUIC, script.timeLimit);
                script.refreshTimer = EditorGUILayout.Toggle(refreshGUIC, script.refreshTimer);
            }

            GUILayout.Space(20);
        }


        script.damageI = (DamageInfo)EditorGUILayout.ObjectField("Damage Info", script.damageI, typeof(DamageInfo), false);
        if (script.damageI != null)
        {
            EditorGUI.indentLevel += 2;

            damageIFold = EditorGUILayout.Foldout(damageIFold, "Extended View");
            if (damageIFold)
                CreateEditor(script.damageI).OnInspectorGUI();

            EditorGUI.indentLevel -= 2;
            GUILayout.Space(20);
        }

        script.appearanceI = (AppearanceInfo)EditorGUILayout.ObjectField("Appearance Info", script.appearanceI, typeof(AppearanceInfo), false);
        if (script.appearanceI != null)
        {
            EditorGUI.indentLevel += 2;

            appearanceIFold = EditorGUILayout.Foldout(appearanceIFold, "Extended View");
            if (appearanceIFold)
                CreateEditor(script.appearanceI).OnInspectorGUI();

            EditorGUI.indentLevel -= 2;
            GUILayout.Space(20);
        }

        //script.destructionI = (destructionInfo)EditorGUILayout.ObjectField("Destruction Info", script.destructionI, typeof(destructionInfo), false);


        GUILayout.Space(20);
        EditorGUI.indentLevel += 1;
        debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), debugMode);
        if (debugMode)
        {
            EditorGUILayout.Space();
            if (script.firstSource != null)
                firstSourceEO = (GameObject)EditorGUILayout.ObjectField("First Source", script.firstSource.gameObject, typeof(GameObject), false);
            if (script.lastSource != null)
                lastSourceEO = (GameObject)EditorGUILayout.ObjectField("Last Source", script.lastSource.gameObject, typeof(GameObject), false);
            if (script.target != null)
                targetEO = (GameObject)EditorGUILayout.ObjectField("Target", script.target.gameObject, typeof(GameObject), false);
            EditorGUILayout.Space();

            if (script.endOnCondition == TerminalCondition.ON_TIMER)
                script.limitTimer = EditorGUILayout.FloatField("Terminal Condition Timer", script.limitTimer);
            GUILayout.Space(20);

            EditorGUILayout.SelectableLabel("Unique ID: " + script.uniqueID);

            if (EditorApplication.isPlaying)
                Repaint();
        }
        EditorGUI.indentLevel -= 1;
    }
}