namespace EnvironInfo
{
    using UnityEngine;
    using System.Collections.Generic;
    using EnvironContainers;

    [CreateAssetMenu(fileName = "NewResistanceInfo.asset", menuName = "Environ/Info/New ResistanceInfo", order = 1)]
    public class ResistanceInfo : ScriptableObject
    {
        public List<ResistanceContainer> resistanceList;
        public float addToDamageDelay;
    }
}