namespace Environ.Main
{
    using UnityEngine;
    using System.Collections.Generic;
    using Info;
    using Support.Enum.General;
    using Support.EffectList;

    public class EnvironObject : MonoBehaviour
    {
        #region Variables
        public float hitPointLimit;
        public float hitPoints;

        public ResistanceInfo resistances;
        public AppearanceInfo appearance; // implement this
        //public List<DestructionInfo> destroyConditions;

        public List<EnvironOutput> output;
        public EnvironEffectList effects;
        #endregion


        #region Start and Update
        void Start()
        {
            output.RemoveAll(o => o == null);
            effects = new EnvironEffectList();
            if (!appearance)
                appearance = ScriptableObject.CreateInstance(typeof(AppearanceInfo)) as AppearanceInfo;
            else
                appearance = Instantiate(appearance);
            appearance.Setup(gameObject);
            appearance.SetupRenderer();


            for (int i = 0; i < output.Count; i++)
            {
                output[i].firstSource = this;

                if (output[i].similarity == Similarity.UNIQUE)
                    output[i] = EnvironOutput.SetSourceAndUID(Instantiate(output[i]));
            }

            SetHitpointsToMax(hitPoints == 0);      //Set hitpoints to hitpoint limit if they were not given a non-zero value
        }

        private void Update()
        {
            appearance.UpdateAppearance();

            if (effects.inputList.Count == 0)
                return;

            effects.CullInputList(this);

            foreach (EnvironOutput e in effects.inputList)
            {
                if (e.endOnCondition == TerminalCondition.ON_TIMER)
                {
                    e.limit.UpdateTimer();
                    effects.ConditionalFlagForRemoval(!e.limit.AboveZero(), e);
                }

                if (e.damageI != null && e.damageI.CanAttack())
                {
                    hitPoints -= GetAdjustedDamage(e.damageI);
                    effects.ConditionalFlagForRemoval(e.damageI.removeEffect, e);
                }

                if (e.appearanceI != null)
                    e.appearanceI.UpdateAppearance();
            }

            ConstrainHitpoints();
        }
        #endregion


        #region Damage Functions
        private float GetAdjustedDamage(DamageInfo di)
        {
            //If dynamicDamage, damage = (if resistances exist) resistance adjusted damage, or di.damage. Else, damage = di.adjustedDamage.
            float damage = di.dynamicDamage ? (resistances != null ? resistances.GetAdjustedDamage(di.damage, di.ID) : di.damage) : di.adjustedDamage;

            di.UpdateLimit(damage);
            return damage;
        }
        #endregion


        #region Hitpoint Functions
        public void ConstrainHitpoints()
        {
            hitPoints = Mathf.Clamp(hitPoints, 0, hitPointLimit);
        }

        public void SetHitpointsToMax(bool conditionalValue = true)
        {
            if (conditionalValue)
                hitPoints = hitPointLimit;
        }
        #endregion


        #region OnTrigger & OnCollision Functions
        private void OnEnterOrStay(TransferCondition transferCondition, EnvironObject targetEO)
        {
            if (targetEO == null || targetEO.effects == null)
                return;

            foreach (EnvironOutput effect in output)
                if (effect.transferCondition == transferCondition)
                    targetEO.effects.Add(effect, targetEO, this);

            foreach (EnvironOutput tEffect in effects.inputList)
                if (tEffect.allowTransmission && tEffect.transferCondition == transferCondition)
                    targetEO.effects.Add(tEffect, targetEO, this);
        }

        private void OnExit(TransferCondition exit, TerminalCondition terminalCondition, EnvironObject targetEO)
        {
            if (targetEO == null || targetEO.effects == null)
                return;

            foreach (EnvironOutput tEffect in effects.inputList)
                if (tEffect.allowTransmission && tEffect.transferCondition == exit)
                    targetEO.effects.Add(tEffect, targetEO, this);

            foreach (EnvironOutput effect in output)
            {
                if (effect.transferCondition == exit)
                    targetEO.effects.Add(effect, targetEO, this);

                if (effect.endOnCondition == terminalCondition)
                    targetEO.effects.FlagForRemoval(effect);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            OnEnterOrStay(TransferCondition.ON_TRIGGER_ENTER, other.GetComponent<EnvironObject>());
        }

        private void OnTriggerStay(Collider other)
        {
            OnEnterOrStay(TransferCondition.ON_TRIGGER_STAY, other.GetComponent<EnvironObject>());
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(TransferCondition.ON_TRIGGER_EXIT, TerminalCondition.ON_TRIGGER_EXIT, other.gameObject.GetComponent<EnvironObject>());
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnEnterOrStay(TransferCondition.ON_COLLISION_ENTER, collision.gameObject.GetComponent<EnvironObject>());
        }

        private void OnCollisionStay(Collision collision)
        {
            OnEnterOrStay(TransferCondition.ON_COLLISION_STAY, collision.gameObject.GetComponent<EnvironObject>());
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(TransferCondition.ON_COLLISION_EXIT, TerminalCondition.ON_COLLISION_EXIT, collision.gameObject.GetComponent<EnvironObject>());
        }
        #endregion
    }
}