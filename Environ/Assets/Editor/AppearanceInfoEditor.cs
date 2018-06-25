using Environ.Info;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AppearanceInfo))]
public class AppearanceInfoEditor : Editor
{

    SerializedProperty particle;
    SerializedProperty material;
    SerializedProperty priority;
    SerializedProperty mRenderer;

    SerializedProperty particlesOn;
    SerializedProperty materialOn;

    SerializedProperty hideOnResistance;
    SerializedProperty hideIDList;

    SerializedProperty debugMode;

    GUIContent particleGUIC = new GUIContent("Particle Prefab", "The particle system to apply. \nIf used as an Effect, the particle will be applied to the Effect's target. \nIf used as an appearance on an Environ Object, the particle will be applied to the object.");
    GUIContent materialGUIC = new GUIContent("Material Prefab", "The material to apply. Takes up the first material slot of the target Mesh Renderer. \nIf used as an Effect, this material will be applied to the Effect's target. \nIf used as an appearance on an Environ Object, this will be the default material for the object.");
    GUIContent priorityGUIC = new GUIContent("Material Priority", "The priority of the material for this Effect. The material with a priority closest to 0 is the material that will be applied. \nHas no effect when used as an Environ Object's appearance.");
    GUIContent mRendererGUIC = new GUIContent("Target Mesh Renderer", "The Mesh Renderer that the material will be applied to.");

    GUIContent particleOnGUIC = new GUIContent("Particles On", "Works only when a particle prefab is set. \nWhen Enabled: particle system will play. \nWhen Disabled: Particle system will stop.");
    GUIContent materialOnGUIC = new GUIContent("Material On", "Works only when a material prefab is set. \nWhen Enabled: material will be applied to the target Mesh Renderer on an Effect being added. \nWhen Disabled: Material will not be applied.");

    GUIContent hideOnGUIC = new GUIContent("Hide On Resistance", "When Enabled: Effect targets with any Nullify type resistances matching the given damage IDs will not have particles or materials applied to them. \nWhen Disabled: No effect. \nHas no effect when used as an Environ Object's appearance.");
    GUIContent hideIDListGUIC = new GUIContent("Hide ID List", "The list of Nullify type resistance damage IDs that will hide the particle and material.");
     
    GUIContent debugGUIC = new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes");

    private void OnEnable()
    {
        particle = serializedObject.FindProperty("particle");
        material = serializedObject.FindProperty("material");
        priority = serializedObject.FindProperty("priority");
        mRenderer = serializedObject.FindProperty("mRenderer");

        particlesOn = serializedObject.FindProperty("particlesOn");
        materialOn = serializedObject.FindProperty("materialOn");

        hideOnResistance = serializedObject.FindProperty("hideOnResistance");
        hideIDList = serializedObject.FindProperty("hideIDList");

        debugMode = serializedObject.FindProperty("debugMode");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(particle, particleGUIC);
        EditorGUILayout.PropertyField(material, materialGUIC);
        EditorGUILayout.PropertyField(priority, priorityGUIC);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(hideOnResistance, hideOnGUIC);

        if (hideOnResistance.boolValue)
            EditorGUILayout.PropertyField(hideIDList, hideIDListGUIC, true);

        GUILayout.Space(20);

        ShowDebug();

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowDebug()
    {
        EditorGUI.indentLevel += 1;

        EditorGUILayout.PropertyField(debugMode, debugGUIC);
        if (debugMode.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(mRenderer, mRendererGUIC);
            EditorGUILayout.PropertyField(particlesOn, particleOnGUIC);
            EditorGUILayout.PropertyField(materialOn, materialOnGUIC);
        }

        EditorGUI.indentLevel -= 1;
    }
}