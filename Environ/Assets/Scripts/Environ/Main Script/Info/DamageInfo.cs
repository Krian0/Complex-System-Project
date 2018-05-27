namespace EnvironInfo
{
    using UnityEngine;
    using EnvironEnum.DamageEnum;
    using Environ.Support.Timer;



    [CreateAssetMenu(fileName = "NewDamageInfo.asset", menuName = "Environ/Info/New DamageInfo", order = 1)]
    public class DamageInfo : EnvironInfoBase
    {
        [Space(10)]
        public DType ID;
        public float damage;

        [HideInInspector] public PauseableTimer attackGap = new PauseableTimer();
        [HideInInspector] public PauseableTimer delay = new PauseableTimer();
        [HideInInspector] public bool refreshDelay = false;

        [HideInInspector] public DLimit limitType;
        [HideInInspector] public PauseableTimer limit = new PauseableTimer();
        [HideInInspector] public bool refreshLimit = false;
        [HideInInspector] public bool removeOnLimitReached;

        [HideInInspector] public bool canAttack;
        [HideInInspector] public bool removeEffect;


        public void Setup()
        {
            delay.ResetTimer();
            attackGap.timer = 0;

            if (limitType == DLimit.NO_LIMIT)
                limit.timer = 1;
            else
                limit.ResetTimer();
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
            canAttack = false;

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
                    canAttack = true;
            }

            return canAttack;
        }

        public void UpdateLimit(float damageValue)
        {
            if (limitType == DLimit.NO_LIMIT)
                return;

            if (limitType == DLimit.DAMAGE_LIMIT && damageValue < 0)
                limit.UpdateTimer(damageValue);

            else if (limitType == DLimit.ATTACK_LIMIT)
                limit.UpdateTimer(1);

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