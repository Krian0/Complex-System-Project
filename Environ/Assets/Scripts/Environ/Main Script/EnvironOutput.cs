namespace Environ
{
    using UnityEngine;
    using EnvironInfo;

    [CreateAssetMenu(fileName = "NewOutput.asset", menuName = "Environ/New Output", order = 1)]
    public class EnvironOutput : ScriptableObject
    {
        public DamageInfo damageOut;
        public DestructionInfo destroyConditionOut;
        public AppearanceInfo appearanceOut;
    }
}