namespace EnvironContainers
{
    using EnvironEnum.DamageEnum;
    using EnvironEnum.ResistanceEnum;
    using UnityEngine;

    [System.Serializable]
    public class ResistanceContainer
    {
        public DamageType resistanceID;
        public ResistanceType resistType;

        [Range(0.00f, 100.00f)]
        public float resistPercent;



        public bool ResistanceExists(DamageType damageID)
        {
            return (resistanceID != damageID);
        }

        public float GetAdjustedDamage(float damage)
        {
            float decimalPercent = (resistPercent / 100);

            switch (resistType)
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