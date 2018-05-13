namespace EnvironInfo
{
    using UnityEngine;
    using System.Collections.Generic;
    using EnvironContainers;
    using EnvironEnum.DamageEnum;
    using EnvironEnum.ResistanceEnum;

    [CreateAssetMenu(fileName = "NewResistanceInfo.asset", menuName = "Environ/Info/New ResistanceInfo", order = 1)]
    public class ResistanceInfo : ScriptableObject
    {
        public List<ResistanceContainer> resistanceList;
        public float addToDamageDelay;

        public float GetAdjustedDamage(float damage, DamageType damageID)
        {
            ResistanceContainer rc = resistanceList.Find(r => r.resistanceID == damageID);

            if (rc == null)
                return damage;

            float decimalPercent = (rc.resistPercent / 100);
            switch (rc.resistType)
            {
                case ResistanceType.HEAL:
                    return -(decimalPercent * damage);

                case ResistanceType.MULTIPLY_DAMAGE:
                    return damage + (damage * decimalPercent);

                case ResistanceType.REDUCE_DAMAGE:
                    return decimalPercent * damage;

                case ResistanceType.NULLIFY_DAMAGE:
                    return 0;

                default:
                    return damage;
            }
        }
    }
}