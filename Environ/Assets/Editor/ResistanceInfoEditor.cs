using UnityEditor;
using UnityEngine;
using Environ.Info;

[CustomEditor(typeof(ResistanceInfo))]
public class ResistanceInfoEditor : Editor
{
    SerializedProperty addToDamageDelay;
    SerializedProperty resistanceList;

    //GUIContent addToGUIC = new GUIContent("Add To Damage Delay", "The amount of time in seconds (if any) to add to the Damage Info attack delay.");   //Yet to be implemented
    GUIContent resistanceListGUIC = new GUIContent("Resistance List", "");

    private void OnEnable()
    {
        //addToDamageDelay = serializedObject.FindProperty("addToDamageDelay");
        resistanceList = serializedObject.FindProperty("resistanceList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();

        //EditorGUILayout.PropertyField(addToDamageDelay, addToGUIC);
        EditorGUILayout.PropertyField(resistanceList, resistanceListGUIC, true);

        serializedObject.ApplyModifiedProperties();
    }
}
