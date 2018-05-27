namespace Environ
{
    using UnityEngine;
    using EnvironEnum.GeneralEnum;
    using EnvironInfo;
    using System.Collections.Generic;

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
                    if (SimilarityCheck.IsUnique(output[i].similarity))
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
        private void OnEnterOrStay(TransferCondition transferCondition, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output.Count == 0 || otherEO.effects == null)
                return;

            foreach (EnvironOutput eo in output)
                if (eo.transferCondition == transferCondition)
                    otherEO.effects.Add(eo, obj.transform);
        }

        private void OnExit(TransferCondition exit, TerminalCondition terminalCondition, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output.Count == 0 || otherEO.effects == null)
                return;

            foreach (EnvironOutput eo in output)
            {
                if (eo.transferCondition == exit)
                    otherEO.effects.Add(eo, obj.transform);

                if (eo.endOnCondition == terminalCondition)
                    otherEO.effects.FlagForRemoval(eo);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            OnEnterOrStay(TransferCondition.ON_TRIGGER_ENTER, other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            OnEnterOrStay(TransferCondition.ON_TRIGGER_STAY, other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(TransferCondition.ON_TRIGGER_EXIT, TerminalCondition.ON_TRIGGER_EXIT, other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnEnterOrStay(TransferCondition.ON_COLLISION_ENTER, collision.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            OnEnterOrStay(TransferCondition.ON_COLLISION_STAY, collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(TransferCondition.ON_COLLISION_EXIT, TerminalCondition.ON_COLLISION_EXIT, collision.gameObject);
        }
        #endregion
    }
}