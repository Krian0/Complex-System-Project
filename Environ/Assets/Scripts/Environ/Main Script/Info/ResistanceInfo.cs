namespace Environ.Info
{
    using UnityEngine;
    using System.Collections.Generic;
    using Support.Containers;
    using Support.Enum.Damage;
    using Support.Enum.Resistance;
    using System.Linq;

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

            float decimalPercent = rc.resistPercent / 100;
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

        //public bool HasResistanceTo(DType ID)
        //{
        //    return resistanceList.Any(r => r.resistanceID == ID);
        //}

        //public bool HasNullifyResistanceTo(DType ID)
        //{
        //    return resistanceList.Any(r => r.resistanceID == ID);
        //}

        //public List<DType> GetNullifyIDs()
        //{
        //    List<ResistanceContainer> rc = resistanceList.FindAll(r => r.resistType == RType.NULLIFY_DAMAGE);
        //    return rc.Select(r => r.resistanceID).ToList();
        //}

        public bool ContainsNullifyResistance()
        {
            return resistanceList.Any(r => r.resistType == RType.NULLIFY_DAMAGE);
        }

        public bool ContainsResistanceToID(List<DType> matchList)
        {
            foreach (ResistanceContainer rc in resistanceList.FindAll(r => r.resistType == RType.NULLIFY_DAMAGE))
                if (matchList.Contains(rc.resistanceID))
                    return true;

            return false;
        }
    }
}