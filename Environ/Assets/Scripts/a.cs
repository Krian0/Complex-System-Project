//#if UNITY_EDITOR
//using UnityEditor;
//using EnvironInfo;
//using EnvironEnum.DamageEnum;
//using UnityEngine;

//[CustomEditor(typeof(DamageInfo))]
//public class DamageInfoEditor : Editor
//{
//    static public GUIContent[] GUIC = { new GUIContent("Attack Gap", "The time in seconds that should pass between each attack"),
//                                            new GUIContent("Initial Attack Delay", "The time in seconds to delay the initial attack"),
//                                            new GUIContent("Refresh Delay?", "Attack delay will reset if the Output already exists as an Effect on an Environ Object. False: Disable, True: Enable"),
//                                            new GUIContent("Limit Type", "Controls when the damage should stop"),
//                                            new GUIContent("Refresh Limit?", "Controls whether the Limit should reset if the Effect already exists on an object") };


//    public override void OnInspectorGUI()
//    {
//        DamageInfo script = (DamageInfo)target;
//        EditorExtender.DrawCustomInspector(this);

//        script.attackGap.maxTime = EditorGUILayout.FloatField(GUIC[0], script.attackGap.maxTime);
//        EditorGUILayout.Space();

//        script.delay.maxTime = EditorGUILayout.FloatField(GUIC[1], script.delay.maxTime);
//        script.refreshDelay = EditorGUILayout.Toggle(GUIC[2], script.refreshDelay);
//        EditorGUILayout.Space();

//        script.limitType = (DLimit)EditorGUILayout.EnumPopup(GUIC[3], script.limitType);
//        if (script.limitType != DLimit.NO_LIMIT)
//        {
//            string limitName = script.limitType.ToString();
//            limitName = limitName[0] + limitName.Substring(1, limitName.IndexOf("_") - 1).ToLower() + " Limit";
//            script.limit.maxTime = EditorGUILayout.FloatField(new GUIContent(limitName, "Counts down based on Limit choice. Attacks can only be made when the Limit is above 0"), script.limit.maxTime);
//            script.refreshLimit = EditorGUILayout.Toggle(GUIC[4], script.refreshLimit);
//        }

//        GUILayout.Space(20);
//        EditorGUI.indentLevel += 1;
//        script.debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), script.debugMode);
//        if (script.debugMode)
//        {
//            EditorGUILayout.Space();
//            script.delay.timer = EditorGUILayout.FloatField("Delay Timer", script.delay.timer);
//            script.attackGap.timer = EditorGUILayout.FloatField("Gap Timer", script.attackGap.timer);

//            if (script.limitType != DLimit.NO_LIMIT)
//                script.limit.timer = EditorGUILayout.FloatField("Limit Tracker", script.limit.timer);
//            EditorGUILayout.Space();

//            script.canAttackFlag = EditorGUILayout.Toggle("Can Attack Flag", script.canAttackFlag);
//            //script.removeOutputFlag = EditorGUILayout.Toggle("Remove OutputFlag", script.removeOutputFlag);

//            if (EditorApplication.isPlaying)
//                Repaint();
//        }
//        EditorGUI.indentLevel -= 1;
//    }
//}
//#endif