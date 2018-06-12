namespace Environ.Main
{
    using UnityEngine;
    using System.Collections.Generic;
    using Info;
    using Support.Enum.General;
    using Support.EffectList;

    public class EnvironObject : MonoBehaviour
    {
        public float hitPointLimit;
        public float hitPoints;

        public ResistanceInfo resistances;
        public AppearanceInfo appearance;
        //public List<DestructionInfo> destroyConditions;

        public List<EnvironOutput> output;
        public EnvironEffectList effects;


        void Start()
        {
            for (int i = output.Count - 1; i >= 0; i--)
            {
                if (output[i] == null)
                    output.RemoveAt(i);
                else
                    if (output[i].similarity == Similarity.UNIQUE)
                    output[i] = EnvironOutput.SetSourceAndUID(Instantiate(output[i]), this);
            }

            effects = new EnvironEffectList();

            if (hitPoints == 0)
                hitPoints = hitPointLimit;
        }

        private void Update()
        {
            if (effects == null) return;

            effects.CullInputList();

            foreach (EnvironOutput e in effects.inputList)
            {
                if (e.damageI != null)
                    if (e.damageI.CanAttack())
                        hitPoints -= GetAdjustedDamage(e.damageI, e);

                if (e.appearanceI != null)
                    e.appearanceI.UpdateAppearance();
            }

            ConstrainHitpoints();
        }


        private float GetAdjustedDamage(DamageInfo di, EnvironOutput effect)
        {
            float damage = di.damage;

            if (resistances != null)
                damage = resistances.GetAdjustedDamage(di.damage, di.ID);

            di.UpdateLimit(damage);
            if (di.removeEffect)
                effects.FlagForRemoval(effect);

            return damage;
        }


        #region Hitpoint Functions
        public void ConstrainHitpoints()
        {
            if (hitPoints < 0)
                hitPoints = 0;
            if (hitPoints > hitPointLimit)
                hitPoints = hitPointLimit;
        }
        #endregion

        #region OnTrigger & OnCollision Functions
        private void OnEnterOrStay(TransferCondition transferCondition, EnvironObject targetEO)
        {
            if (output.Count == 0 || targetEO == null || targetEO.effects == null)
                return;

            foreach (EnvironOutput effect in output)
                if (effect.transferCondition == transferCondition)
                    targetEO.effects.Add(effect, targetEO, this);

            //Effect Transmission to test
            foreach (EnvironOutput tEffect in effects.inputList)
                if (tEffect.allowTransmission)
                    targetEO.effects.Add(tEffect, targetEO, this);
        }

        private void OnExit(TransferCondition exit, TerminalCondition terminalCondition, EnvironObject targetEO)
        {
            if (output.Count == 0 || targetEO == null || targetEO.effects == null)
                return;

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