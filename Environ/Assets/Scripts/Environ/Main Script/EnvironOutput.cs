namespace Environ
{
    using UnityEngine;
    using EnvironInfo;
    using EnvironEnum.GeneralEnum;

    [CreateAssetMenu(fileName = "NewOutput.asset", menuName = "Environ/New Output", order = 1)]
    public class EnvironOutput : ScriptableObject
    {
        [Tooltip("Determines when to transfer the damage effect to another object")] public TransferCondition transferOnCondition;
        [Tooltip("Determines if the EnvironOutput effect is allowed to spread from a non-original source")] public bool allowTransmission = false;


        public DamageInfo damageOut;
        public AppearanceInfo appearanceOut;
        //public DestructionInfo destroyConditionOut;

        public EnvironObject source;

        public static bool operator ==(EnvironOutput a, EnvironOutput b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            //What the **** is happening to the enums? Why can't I compare them as Enums? What changed to make them decide they can't do it any more?

            if (a.damageOut == b.damageOut /*&& a.appearanceOut == b.appearanceOut*/)
                return (a.source == b.source) && ((int)a.transferOnCondition == (int)b.transferOnCondition);

            return false;
        }

        public static bool operator !=(EnvironOutput a, EnvironOutput b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            EnvironOutput eo = (EnvironOutput)o;

            return this == eo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}