using UnityEngine;

namespace EnvironGuiContent
{
    struct EGC
    {
        static public GUIContent[] eOut = {
            new GUIContent("Transfer Condition", "When this condition is met the Output will be cloned and added as an Effect to the other Environ Object's Effects list, if it doesn't already exist in the list."),
            new GUIContent("Allow Transmission?", "When True, this option allows the Effect itself to be cloned and added as an Effect on other Environ Objects. When False, only the Output can do so."),
            new GUIContent("Terminal Condition", "When this condition is met the Effect will be removed."),
            new GUIContent("Time Limit", "The time in seconds before the Terminal Condition is met."),
            new GUIContent("Refresh Timer?", "When True, if this Effect exists in an Effects list upon a transfer attempt, the Time Limit will be reset.")};

        static public string eUSSU =
            "Used to determine when an Effect does or does not exist (is a unique instance) in another Environ Object's Effects list. \n\n\n" +
            "Unique_A:    Each original Output ScriptableObject and each clone is a unique instance. \n\n" +
            "Unique_B:    Each original Output ScriptableObject and each clone is a unique instance, except when matching a Selective Index. \n\n" +
            "Standard_A:  Each original Output ScriptableObject is a unique instance that their clones share. \n\n" +
            "Standard_B:  Each original Output ScriptableObject is a unique instance that their clones and any matching Selective Index share. \n\n" +
            "Selective_A: Will be considered the same as any Output with Info that originates from selected Scriptable Objects. \n\n" +
            "Selective_B: Shares a unique instance with any Effect that has matching Info";
    }
}