namespace Environ.Support.Containers
{
    using UnityEngine;
    using Enum.Damage;
    using Enum.Resistance;

    [System.Serializable]
    public class ResistanceContainer
    {
        public DType resistanceID;
        public RType resistType;

        [Range(0.00f, 100.00f)]
        public float resistPercent;
    }
}