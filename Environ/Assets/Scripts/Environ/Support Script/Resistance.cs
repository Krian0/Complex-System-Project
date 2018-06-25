namespace Environ.Support.Containers
{
    using System;
    using UnityEngine;
    using Environ.Support.Enum.Damage;
    using Environ.Support.Enum.Resistance;

    [Serializable]
    public class Resistance
    {
        #region Variables
        [Tooltip("The identifier for the type of damage resistance.")] public DamageType resistanceID;
        [Tooltip("The identifier for the type of resistance effect.")] public ResistanceType resistType;
        [Tooltip("The percentage of resistance to the damage.")] [Range(0.00f, 100.00f)] public float resistPercent;

        private delegate float TypeDelegate(float damage, float decimalPercent);
        private static TypeDelegate[] typeFunctions = new TypeDelegate[] { new TypeDelegate(Nullify), new TypeDelegate(Reduce), new TypeDelegate(Multiply), new TypeDelegate(Heal) };
        #endregion


        #region Get Adjusted Damage Functions
        ///<summary> Returns the damage adjusted by resistance. </summary>
        public float GetAdjustedDamage(float damage)
        {
            return typeFunctions[(int)resistType](damage, resistPercent / 100);     //Calls the matching typeFunction for the resistType
        }

        ///<summary> Returns the damage adjusted by resistance. Nullify always returns 0. </summary>
        private static float Nullify(float damage, float decimalPercent)
        {
            return 0f;
        }

        ///<summary> Returns the damage adjusted by resistance. Reduces the damage by the resist percent. </summary>
        private static float Reduce(float damage, float decimalPercent)
        {
            return damage - (damage * decimalPercent);
        }

        ///<summary> Returns the damage adjusted by resistance. Multiplies the damage by the resist percent. </summary>
        private static float Multiply(float damage, float decimalPercent)
        {
            return damage + (damage * decimalPercent);
        }

        ///<summary> Returns the damage adjusted by resistance. Heals the damage by the resist percent. </summary>
        private static float Heal(float damage, float decimalPercent)
        {
            return -(decimalPercent * damage);
        }
        #endregion
    }
}