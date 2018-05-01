namespace EnvironInfo
{
    using UnityEngine;
    using EnvironEnum.DamageEnum;

    [CreateAssetMenu(fileName = "NewDamageInfo.asset", menuName = "Environ/Info/New DamageInfo", order = 1)]
    public class DamageInfo : ScriptableObject
    {
        public DamageType ID;
        public float damage;
        public DamageCondition transferOnCondition;
        public DamageRegularity regularity;

        public float limit;
        [HideInInspector]
        public float limitTracker;

        public bool refreshLimits;

        public float delay;

        public float timeBetweenDamage;
        public float tbdTimer;

        private bool canAttack;
        //private bool meetsCondition;


        public void UpdateLimits()
        {
            canAttack = false;

            if (delay > 0)
                delay -= Time.deltaTime;

            if (delay <= 0)
            {
                if (tbdTimer > 0)
                    tbdTimer -= Time.deltaTime;

                if (tbdTimer <= 0)
                {
                    tbdTimer = timeBetweenDamage;

                    switch (regularity)
                    {
                        case DamageRegularity.UNTIL_DESTROYED:
                            canAttack = true;
                            return;

                        case DamageRegularity.TIME_LIMIT:
                            if (limitTracker > 0)
                                limitTracker -= Time.deltaTime;
                            if (limitTracker <= 0)
                                canAttack = true;
                            break;

                        case DamageRegularity.ATTACK_LIMIT:
                            if (limitTracker > 0)
                                limitTracker--;
                            break;
                    }
                }
            }

        }


        //public void UpdateTimers()
        //{
        //    if (delay > 0)
        //    {
        //        delay -= Time.deltaTime;
        //        return;
        //    }

        //    if (regularity == DamageRegularity.TIME_LIMIT)
        //        limitTracker -= Time.deltaTime;

        //    if (damageTimer > 0)
        //        damageTimer -= Time.deltaTime;


        //    if (delay <= 0 && damageTimer <= 0 && LimitReached())
        //    {
        //        damageTimer = timeBetweenDamage;
        //        limitTracker = limit;
        //        canAttack = true;
        //    }
        //}

        //public bool IsValidDamageType()
        //{
        //    return ID != DamageType.NULL;
        //}

        public bool CanAttack()
        {
            return canAttack;
        }

        //public void SetAttack(bool value)
        //{
        //    canAttack = value;
        //}

        //public bool MeetsDamageCondition()
        //{
        //    return meetsCondition;
        //}

        //public void setMeetsDamageCondition(bool value)
        //{
        //    meetsCondition = value;
        //}

        public void RefreshLimit()
        {
            limitTracker = limit;
        }

        public bool HasNoLimit()
        {
            return regularity == DamageRegularity.UNTIL_DESTROYED;
        }

        //private bool LimitReached()
        //{
        //    switch (regularity)
        //    {
        //        case DamageRegularity.UNTIL_DESTROYED:
        //            return true;

        //        case DamageRegularity.TIME_LIMIT:
        //            return (limitTracker <= 0);

        //        case DamageRegularity.DAMAGE_LIMIT:
        //            return (limitTracker > 0);

        //        case DamageRegularity.ATTACK_LIMIT:
        //            return (limitTracker > 0);

        //        default:
        //            return false;
        //    }
        //}
    }
}