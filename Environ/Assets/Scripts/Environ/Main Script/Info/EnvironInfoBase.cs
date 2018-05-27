namespace EnvironInfo
{
    using UnityEngine;

    public class EnvironInfoBase : ScriptableObject
    {
        [HideInInspector] public string uniqueID;
#if UNITY_EDITOR
        [HideInInspector] public bool debugMode;
#endif
    }
}