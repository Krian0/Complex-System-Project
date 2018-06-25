namespace Environ.Support.Base
{
    using UnityEngine;

    public class EnvironBase : ScriptableObject
    {
        [HideInInspector] public string uniqueID;
        #if UNITY_EDITOR
        [HideInInspector] public bool debugMode;
        #endif
    }
}