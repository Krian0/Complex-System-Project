namespace Environ.Main
{
    using System.Linq;
    using UnityEngine;
    using Environ.Info;
    using Environ.Support.Base;
    using Environ.Support.EffectList;
    using Environ.Support.Enum.General;
    using Environ.Support.Enum.Resistance;
    using Environ.Support.Timer;

    [CreateAssetMenu(fileName = "NewOutput.asset", menuName = "Environ/New Output", order = 1)]
    public class EnvironOutput : EnvironBase
    {
        #region Variables
        public Similarity similarity;
        public DamageInfo damageSimilarity;
        public AppearanceInfo appearanceSimilarity;
        public bool selectiveCanBeSimilar;

        public TransferCondition transferCondition;
        public TerminalCondition endOnCondition;
        public PauseableTimer limit;
        public bool allowTransmission;
        public bool refreshTimer;

        public EnvironObject firstSource;
        public EnvironObject lastSource;
        public EnvironObject target;

        public DamageInfo damageI;
        public AppearanceInfo appearanceI;

        private static string selectiveDescription = "SELECTIVE - This EnvironObject is the same as any other EnvironObject containing Info with matching IDs";
        #endregion


        #region Setup and Update
        ///<summary> Sets Info and variables up for use. </summary>
        public void Setup(EnvironObject targetEO, EnvironObject lastSourceEO)
        {
            if (damageI)
            {
                damageI = Instantiate(damageI);
                damageI.Setup(targetEO.resistances);
            }

            if (appearanceI)
            {
                appearanceI = Instantiate(appearanceI);
                appearanceI.Setup(targetEO.gameObject);

                if (appearanceI.hideOnResistance && targetEO.resistances)   //Turns off Appearance particles and materials if hideOnResistance has been set up
                    if (targetEO.resistances.HasTypeIDMatch(ResistanceType.NULLIFY_DAMAGE, appearanceI.hideIDList.Distinct()))
                        appearanceI.TurnOff();
            }

            limit.ResetTimer();
            lastSource = lastSourceEO;
            target = targetEO;

            if (similarity == Similarity.SELECTIVE)
                uniqueID = selectiveDescription;
            if (similarity == Similarity.UNIQUE)
            {
                uniqueID = GetInstanceID().ToString();
                firstSource = lastSourceEO;
            }
        }

        ///<summary> Sets Info and Output IDs. </summary>
        public void SetID()
        {
            if (damageI)
                damageI.uniqueID = damageI.GetInstanceID().ToString();
            if (appearanceI)
                appearanceI.uniqueID = appearanceI.GetInstanceID().ToString();

            uniqueID = GetInstanceID().ToString();
        }

        ///<summary> Updates Effect limit, checks to see if DamageInfo can attack, updates AppearanceInfo. Flags effect for removal when the conditions for it are met. </summary>
        public void UpdateOutput(EnvironEffectList effects, ref float hitPoints, ResistanceInfo resistances)
        {
            if (endOnCondition == TerminalCondition.TIMER_ZERO)
            {
                limit.UpdateTimer();
                effects.ConditionalFlagForRemoval(limit.belowZero, this);
            }

            if (damageI)
            {
                damageI.Attack(ref hitPoints, resistances);
                effects.ConditionalFlagForRemoval(damageI.removeEffect, this);
            }

            if (appearanceI)
                appearanceI.UpdateInfo();
        }
        #endregion


        #region
        ///<summary> Resets any timers marked for refreshing to their maxTime. </summary>
        public void Refresh()
        {
            if (refreshTimer)
                limit.ResetTimer();

            if (damageI)
                damageI.Refresh();
        }
        #endregion


        #region Operator Overloads
        public static bool operator ==(EnvironOutput a, EnvironOutput b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;


            if (a.similarity == Similarity.SELECTIVE || b.similarity == Similarity.SELECTIVE)
            {
                if (a.selectiveCanBeSimilar && b.selectiveCanBeSimilar)
                    return (a.damageI == b.damageI && a.appearanceI == b.appearanceI);

                else
                    return false;
            }

            return a.uniqueID == b.uniqueID;
        }

        public static bool operator !=(EnvironOutput a, EnvironOutput b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            return this == (EnvironOutput)o;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}