using UnityEditor;
using UnityEngine;
using Environ.Support.Containers;
using Environ.Support.Enum.Resistance;

//------------------------------------------------------------------------------//
//
//  For reasons beyond me, this Editor doesn't want to actually work, meaning 
//  resistPercent will show up no matter the resistType. It won't effect the 
//  Nullify Damage type when calculating damage, just a heads up. 
//  If you have any idea what might be wrong, please let me know.
//
//------------------------------------------------------------------------------//

[CustomEditor(typeof(Resistance))]
public class ResistanceEditor : Editor
{
    SerializedProperty resistanceID;
    SerializedProperty resistType;
    SerializedProperty resistPercent;

    GUIContent idGUIC = new GUIContent("Resistance ID", "The identifier for the type of damage resistance.");
    GUIContent typeGUIC = new GUIContent("Resist Type", "The identifier for the type of resistance effect.");
    GUIContent percentGUIC = new GUIContent("Resist Percent", "The percentage of resistance to the damage.");

    private void OnEnable()
    {
        resistanceID = serializedObject.FindProperty("resistanceID");
        resistType = serializedObject.FindProperty("resistType");
        resistPercent = serializedObject.FindProperty("resistPercent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(resistanceID, idGUIC);
        EditorGUILayout.PropertyField(resistType, typeGUIC);

        if (resistType.enumValueIndex != (int)ResistanceType.NULLIFY_DAMAGE)
            EditorGUILayout.PropertyField(resistPercent, percentGUIC);

        serializedObject.ApplyModifiedProperties();
    }
}