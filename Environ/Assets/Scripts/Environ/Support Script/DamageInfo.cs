namespace EnvironInfo
{
    using UnityEngine;
    using EnvironEnum.DamageEnum;

    [CreateAssetMenu(fileName = "NewDamageInfo.asset", menuName = "Environ/Info/New DamageInfo", order = 1)]
    public class DamageInfo : ScriptableObject
    {
        public DamageType ID;
        public float damage;
        public DamageCondition condition;
        public DamageRegularity regularity;

        public float limit;
        public float limitTracker;

        public float delay;

        public float timeBetweenDamage;
        public float damageTimer;

        private bool canAttack;
        private bool meetsCondition;


        public void UpdateTimers()
        {
            if (delay > 0)
                delay -= Time.deltaTime;

            if (regularity == DamageRegularity.TIME_LIMIT)
                limitTracker -= Time.deltaTime;

            if (damageTimer > 0)
                damageTimer -= Time.deltaTime;


            if (delay <= 0 && damageTimer <= 0 && LimitReached())
            {
                damageTimer = timeBetweenDamage;
                limitTracker = limit;
                canAttack = true;
            }
        }

        public bool IsValidDamageType()
        {
            return ID != DamageType.NULL;
        }

        public bool CanAttack()
        {
            return canAttack;
        }

        public void SetAttack(bool value)
        {
            canAttack = value;
        }

        public bool MeetsDamageCondition()
        {
            return meetsCondition;
        }

        public void setMeetsDamageCondition(bool value)
        {
            meetsCondition = value;
        }

        private bool LimitReached()
        {
            switch (regularity)
            {
                case DamageRegularity.UNTIL_DESTROYED:
                    return true;

                case DamageRegularity.TIME_LIMIT:
                    return (limitTracker <= 0);

                case DamageRegularity.DAMAGE_LIMIT:
                    return (limitTracker > 0);

                case DamageRegularity.ATTACK_LIMIT:
                    return (limitTracker > 0);

                default:
                    return false;
            }
        }
    }
}