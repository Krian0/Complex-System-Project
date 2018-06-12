namespace Environ.Info
{
    using UnityEngine;
    using System.Collections.Generic;
    using Support.Containers;
    using Support.Enum.Damage;
    using Support.Enum.Resistance;

    [CreateAssetMenu(fileName = "NewResistanceInfo.asset", menuName = "Environ/Info/New ResistanceInfo", order = 1)]
    public class ResistanceInfo : ScriptableObject
    {
        public List<ResistanceContainer> resistanceList;
        public float addToDamageDelay;

        public float GetAdjustedDamage(float damage, DType damageID)
        {
            ResistanceContainer rc = resistanceList.Find(r => r.resistanceID == damageID);

            if (rc == null)
                return damage;

            float decimalPercent = (rc.resistPercent / 100);
            switch (rc.resistType)
            {
                case RType.HEAL:
                    return -(decimalPercent * damage);

                case RType.MULTIPLY_DAMAGE:
                    return damage + (damage * decimalPercent);

                case RType.REDUCE_DAMAGE:
                    return decimalPercent * damage;

                case RType.NULLIFY_DAMAGE:
                    return 0;

                default:
                    return damage;
            }
        }
    }
}