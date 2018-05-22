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
        //[HideInInspector] public float attackGap;
        //[HideInInspector] public float gapTimer;
        //[HideInInspector] public float delay;
        //[HideInInspector] public float delayTimer;
        [HideInInspector] public PauseableTimer attackGap = new PauseableTimer();
        [HideInInspector] public PauseableTimer delay = new PauseableTimer();
        [ShowIf("ID", ShowIfAttribute.Compare.NOT_EQUALS, (int)DType.ELECTRICITY)]
         public bool refreshDelay = false;

        [Space(40)]
        [HideInInspector] public DLimit limitType;

        [HideInInspector] public PauseableTimer limit = new PauseableTimer();
        //[HideInInspector] public float limit = 0;
        //[HideInInspector] public float limitTracker;

        //[HideInInspector] public bool removeOnLimitReached;

        [HideInInspector] public bool refreshLimit = false;

        [HideInInspector] public bool canAttackFlag;
        //[HideInInspector] public bool removeOutputFlag = false;


        public void Setup()
        {
            delay.ResetTimer();
            attackGap.timer = 0;
            limit.ResetTimer();
            //delayTimer = delay;
            //gapTimer = 0;
            //limitTracker = limit;
        }

        public bool CanAttack()
        {
            canAttackFlag = false;

            delay.UpdateTimer();

            if (delay.AtOrBelowZero())
                attackGap.UpdateTimer();

            if (attackGap.AtOrBelowZero())
            {
                attackGap.ResetTimer();

                switch (limitType)
                {
                    case DLimit.NO_LIMIT:
                        canAttackFlag = true;
                        break;

                    case DLimit.TIME_LIMIT:
                        limit.UpdateTimer();
                        if (limit.AboveZero())
                            canAttackFlag = true;
                        break;

                    case DLimit.ATTACK_LIMIT:
                        if (limit.AboveZero())
                        {
                            canAttackFlag = true;
                            UpdateLimit(1);
                        }
                        break;

                    case DLimit.DAMAGE_LIMIT:
                        if (limit.AboveZero())
                            canAttackFlag = true;
                        break;

                    default:
                        break;
                }
            }


            //if (limit.AtOrBelowZero())
            //    removeOutputFlag = true;

            return canAttackFlag;
        }

        public float UpdateLimit(float damageValue)
        {
            if (limitType == DLimit.DAMAGE_LIMIT && damageValue < 0)
                limit.UpdateTimer(damageValue);

            return damageValue;
        }

        public void Refresh()
        {
            if (refreshDelay)
                delay.ResetTimer();

            if (refreshLimit)
                limit.ResetTimer();
        }



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
    }
}