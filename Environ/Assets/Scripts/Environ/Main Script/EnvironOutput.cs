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

        public EnvironObject source;

        public static bool operator ==(EnvironOutput a, EnvironOutput b)
        {
            return a.damageOut.ID == b.damageOut.ID && a.source == b.source;
        }

        public static bool operator !=(EnvironOutput a, EnvironOutput b)
        {
            return !(a == b);
        }


        public void RefreshLimit()
        {
            damageOut.limitTracker = damageOut.limit;
        }
    }
}