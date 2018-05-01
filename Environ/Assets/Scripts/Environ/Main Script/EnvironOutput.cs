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
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            if (ReferenceEquals(a.damageOut, null) && ReferenceEquals(b.damageOut, null))
                return (a.source == b.source);

            if (a.damageOut == null || b.damageOut == null)
                return false;

            return (a.damageOut.ID == b.damageOut.ID) && (a.damageOut.damage == b.damageOut.damage) && (a.damageOut.transferOnCondition == b.damageOut.transferOnCondition) && (a.source == b.source);
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

            //if (eo == null)
            //    return false;
            //else
            //{
            //    if ((this.damageOut == null && eo.damageOut == null) && (this.source == eo.source) && (this.name == eo.name))
            //        return true;

            //    return (this.damageOut.ID == eo.damageOut.ID) && (this.source == eo.source) && (this.name == eo.name);
            //}
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}