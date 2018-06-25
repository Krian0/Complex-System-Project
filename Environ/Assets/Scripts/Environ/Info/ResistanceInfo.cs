namespace Environ.Info
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using Support.Containers;
    using Support.Enum.Damage;
    using Support.Enum.Resistance;

    [Serializable]
    [CreateAssetMenu(fileName = "NewResistanceInfo.asset", menuName = "Environ/Info/New ResistanceInfo", order = 4)]
    public class ResistanceInfo : ScriptableObject
    {
        public List<Resistance> resistanceList = new List<Resistance>();

        ///<summary> Checks resistanceList for any entry with a ResistanceType and DamageType matching the parameters. Returns true if a match is found. </summary> 
        public bool HasTypeIDMatch(ResistanceType type, IEnumerable<DamageType> idList)
        {
            return resistanceList.Exists(r => r.resistType == type && idList.Contains(r.resistanceID));
        }

        ///<summary> Checks resistanceList for any DamageType that matches with the given idList. Returns true if a match is found. </summary> 
        public bool HasIDMatch(IEnumerable<DamageType> idList)
        {
            return resistanceList.Select(r => r.resistanceID).Intersect(idList).Any();
        }

        ///<summary> Checks idList_A for any DamageType that matches with the given idList_B. Returns true if a match is found. </summary> 
        public static bool HasIDMatch(IEnumerable<DamageType> idList_A, IEnumerable<DamageType> idList_B)
        {
            return idList_A.Intersect(idList_B).Any();
        }
    }
}