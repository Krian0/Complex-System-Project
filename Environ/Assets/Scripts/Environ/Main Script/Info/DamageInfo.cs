namespace Environ.Info
{
    using UnityEngine;
    using Environ.Support.Enum.Damage;
    using Environ.Support.Timer;

    [CreateAssetMenu(fileName = "NewDamageInfo.asset", menuName = "Environ/Info/New Damage Info", order = 1)]
    public class DamageInfo : EnvironInfoBase
    {
        public DType ID;
        public float damage;
        public bool dynamicDamage;
        public float adjustedDamage;

        public PauseableTimer attackGap = new PauseableTimer();
        public PauseableTimer delay = new PauseableTimer();
        public bool refreshDelay = false;

        public DLimit limitType;
        public PauseableTimer limit = new PauseableTimer();
        public bool refreshLimit = false;
        public bool removeOnLimitReached;

        public bool removeEffect;

        public void Setup(ResistanceInfo targetR)
        {
            delay.ResetTimer();
            attackGap.timer = 0;

            if (limitType == DLimit.NO_LIMIT)
                limit.timer = 1;
            else
                limit.ResetTimer();

            if (!dynamicDamage)
                adjustedDamage = targetR != null ? targetR.GetAdjustedDamage(damage, ID) : damage;
        }

        public void Refresh()
        {
            if (refreshDelay)
                delay.ResetTimer();

            if (refreshLimit)
                limit.ResetTimer();
        }

        public bool CanAttack()
        {
            delay.UpdateTimer();
            if (!delay.AboveZero())
            {
                attackGap.UpdateTimer();
                if (limitType == DLimit.TIME_LIMIT)
                    limit.UpdateTimer();
            }

            if (!attackGap.AboveZero())
            {
                attackGap.ResetTimer();
                if (limit.AboveZero())
                    return true;
            }

            return false;
        }

        public void UpdateLimit(float damageValue)
        {
            if (limitType == DLimit.NO_LIMIT)
                return;

            if (limitType == DLimit.DAMAGE_LIMIT && damageValue > 0)        //Won't update the damage limit if it heals (damage has negative value)
                limit.UpdateTimer(damageValue);

            else if (limitType == DLimit.ATTACK_LIMIT)
                limit.UpdateTimer(1.00f);

            if (removeOnLimitReached && !limit.AboveZero())
                removeEffect = true;
        }


        #region Operator Overrides
        public static bool operator ==(DamageInfo a, DamageInfo b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.uniqueID == b.uniqueID;
        }

        public static bool operator !=(DamageInfo a, DamageInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            return this == (DamageInfo)o;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}