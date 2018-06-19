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

    GUIContent particleGUIC = new GUIContent("Particle Prefab", "The Particle System to apply. \nIf used as an Effect, the Particle will be applied to the Effect's Target. \nIf used as an Appearance on an Environ Object, the Particle will be applied to the object.");
    GUIContent materialGUIC = new GUIContent("Material Prefab", "The Material to apply. Takes up the first Material slot of the Target Mesh Renderer. \nIf used as an Effect, this Material will be applied to the Effect's Target. \nIf used as an Appearance on an Environ Object, this will be the default material for the object.");
    GUIContent priorityGUIC = new GUIContent("Material Priority", "The Priority of the Material for this Effect. The Material with a Priority closest to 0 is the Material that will be applied. \nHas no effect when used as an Environ Object's Appearance.");
    GUIContent mRendererGUIC = new GUIContent("Target Mesh Renderer", "The Mesh Renderer that the Material will be applied to.");

    GUIContent particleOnGUIC = new GUIContent("Particles On", "Works only when a Particle Prefab is set. \nWhen Enabled: Particle System will play. \nWhen Disabled: Particle System will stop.");
    GUIContent materialOnGUIC = new GUIContent("Material On", "Works only when a Material Prefab is set. \nWhen Enabled: Material will be applied to the Target Mesh Renderer on an Effect being added. \nWhen Disabled: Material will not be applied.");

    GUIContent hideOnGUIC = new GUIContent("Hide On Resistance", "When Enabled: Effect Targets with any Nullify type Resistances matching the given Damage IDs will not have Particles or Materials applied to them. \nWhen Disabled: No effect. \nHas no effect when used as an Environ Object's Appearance.");
    GUIContent hideIDListGUIC = new GUIContent("Hide ID List", "The list of Nullify type Resistance Damage IDs that will hide the Particle and Material");
     
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