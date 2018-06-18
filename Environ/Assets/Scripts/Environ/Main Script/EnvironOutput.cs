namespace Environ.Main
{
    using UnityEngine;
    using Info;
    using Support.Enum.General;
    using Support.Timer;
    using Environ.Support.Enum.Damage;
    using System.Collections.Generic;
    using System.Linq;
    using Support.Enum.Resistance;

    [CreateAssetMenu(fileName = "NewOutput.asset", menuName = "Environ/New Output", order = 1)]
    public class EnvironOutput : ScriptableObject
    {
        [Space(10)]
        [HideInInspector] public TransferCondition transferCondition = TransferCondition.ON_COLLISION_ENTER;
        [HideInInspector] public bool allowTransmission = false; // set up transmission
        [HideInInspector] public Similarity similarity = Similarity.STANDARD;
        [HideInInspector] public bool selectiveCanBeSimilar;
        [HideInInspector] public DamageInfo damageSimilarity;
        [HideInInspector] public AppearanceInfo appearanceSimilarity;
        //[HideInInspector] public DestructionInfo destructionSimilarity;


        [HideInInspector] public TerminalCondition endOnCondition;

        [HideInInspector] public bool refreshTimer = false;
        //[HideInInspector] public float timeLimit;
        //[HideInInspector] public float limitTimer;
        [HideInInspector] public PauseableTimer limit; // replace above with this

        [HideInInspector] public DamageInfo damageI;
        [HideInInspector] public AppearanceInfo appearanceI;
        //public DestructionInfo destructionI;

        [HideInInspector] public EnvironObject firstSource;
        [HideInInspector] public EnvironObject lastSource;
        [HideInInspector] public EnvironObject target;

        [HideInInspector] public string uniqueID = "0";




        public void SetSourceAndUID(EnvironObject eoSource)
        {
            if (damageI)
                damageI.uniqueID = GetInstanceID().ToString();
            if (appearanceI)
                appearanceI.uniqueID = GetInstanceID().ToString();

            if (similarity == Similarity.SELECTIVE)
                uniqueID = "SELECTIVE - This EnvironObject is the same as any other EnvironObject containing Info with matching IDs";
            else
                uniqueID = GetInstanceID().ToString();
        }

        public static EnvironOutput SetSourceAndUID(EnvironOutput output)
        {
            if (output == null)
                return null;


            if (output.damageI)
                output.damageI.uniqueID = output.GetInstanceID().ToString();
            if (output.appearanceI)
                output.appearanceI.uniqueID = output.GetInstanceID().ToString();

            if (output.similarity == Similarity.SELECTIVE)
                output.uniqueID = "SELECTIVE - This EnvironObject is the same as any other EnvironObject containing Info with matching IDs";
            else
                output.uniqueID = output.GetInstanceID().ToString();

            return output;
        }

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

                if (appearanceI.hideOnResistance && targetEO.resistances)
                    if (targetEO.resistances.HasNullifyIDMatch(appearanceI.hideIDList.Distinct().ToList()))
                        appearanceI.TurnOff();
            }

            //if (destructionI)
            //    destructionI = Instantiate(destructionI);

            limit.ResetTimer();
            lastSource = lastSourceEO;
            target = targetEO;

            if (similarity == Similarity.UNIQUE)
                uniqueID = GetInstanceID().ToString();
        }

        public void Refresh()
        {
            if (refreshTimer)
                limit.ResetTimer();

            if (damageI != null)
                damageI.Refresh();
        }

        public void Update()
        {
            if (limit.AboveZero())
                limit.UpdateTimer();
        }


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