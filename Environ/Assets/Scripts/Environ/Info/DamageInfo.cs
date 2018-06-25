namespace Environ.Info
{
    using UnityEngine;
    using Environ.Support.Base;
    using Environ.Support.Containers;
    using Environ.Support.Enum.Damage;
    using Environ.Support.Timer;

    [CreateAssetMenu(fileName = "NewDamageInfo.asset", menuName = "Environ/Info/New Damage Info", order = 1)]
    public class DamageInfo : EnvironBase
    {
        #region Variables
        public DamageType ID;
        public float damage;
        public bool dynamicDamage;
        public float adjustedDamage;

        public PauseableTimer attackGap = new PauseableTimer();
        public PauseableTimer delay = new PauseableTimer();
        public bool refreshDelay = false;

        public DamageLimit limitType;
        public PauseableTimer limit = new PauseableTimer();
        public bool refreshLimit = false;
        public bool removeOnLimitReached;

        public bool removeEffect;
        #endregion


        #region Setup Functions
        ///<summary> Sets variables up for use, sets adjustedDamage to resistance adjusted damage if dynamicDamage is not chosen. </summary>
        public void Setup(ResistanceInfo targetR)
        {
            delay.ResetTimer();
            attackGap.timer = 0;

            if (limitType == DamageLimit.NONE)
                limit.timer = 1;
            else
                limit.ResetTimer();

            if (!dynamicDamage)
                adjustedDamage = (targetR != null) ? GetAdjustedDamage(targetR) : damage;
        }
        #endregion


        #region Timer and Limit Functions
        ///<summary> Update the limit tracker with the appropriate value for the Limit type. </summary>
        public void UpdateLimit(float damageValue)
        {
            if (limitType == DamageLimit.NONE)
                return;

            if (limitType == DamageLimit.DAMAGE_LIMIT && damageValue < 0)
                limit.UpdateTimer(damageValue);

            else if (limitType == DamageLimit.ATTACK_LIMIT)
                limit.UpdateTimer(1);

            if (removeOnLimitReached && limit.belowZero)
                removeEffect = true;
        }

        ///<summary> Resets any timers marked for refreshing to their maxTime. </summary>
        public void Refresh()
        {
            if (refreshDelay)
                delay.ResetTimer();

            if (refreshLimit)
                limit.ResetTimer();
        }
        
        ///<summary> Updates several timers, returns true if an attack can be made. </summary>
        private bool CanAttack()
        {
            delay.UpdateTimer();                        //Attacks can only be made when delay has timed out, attack gap has timed out,
            if (delay.belowZero)                        //and limit is still above zero
            {
                attackGap.UpdateTimer();
                if (limitType == DamageLimit.TIME_LIMIT)
                    limit.UpdateTimer();
            }

            if (attackGap.belowZero)
            {
                attackGap.ResetTimer();
                if (limit.aboveZero)
                    return true;
            }

            return false;
        }
        #endregion


        #region Attack and Damage Functions
        /// <summary> Checks if an attack can be made, and if so subtracts resistance adjusted damage from the given hitPoints. </summary>
        public void Attack(ref float hitPoints, ResistanceInfo resistances)
        {
            if (CanAttack())
                hitPoints -= GetDamage(resistances);
        }

        ///<summary> Calculates damage based on DamageInfo options chosen in the inspector. </summary>
        public float GetDamage(ResistanceInfo resistances)
        {
            float finalDamage = (!dynamicDamage) ? ((!resistances) ? damage : adjustedDamage) : GetAdjustedDamage(resistances);
            UpdateLimit(finalDamage);

            return finalDamage;
        }

        ///<summary> Searches given ResistanceInfo for resistances matching the damage ID. If a resistance is found, it will be used to calculate and return damage, otherwise damage is returned. </summary>
        private float GetAdjustedDamage(ResistanceInfo resistances)
        {
            Resistance res = resistances.resistanceList.Find(r => r.resistanceID == ID);
            return (res == null) ? damage : res.GetAdjustedDamage(damage);
        }
        #endregion


        #region Operator Overrides
        public static bool operator ==(DamageInfo a, DamageInfo b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.uniqueID == b.uniqueID;
        }

        public static bool operator !=(DamageInfo a, DamageInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            return this == (DamageInfo)o;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}