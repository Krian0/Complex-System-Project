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
    }
}