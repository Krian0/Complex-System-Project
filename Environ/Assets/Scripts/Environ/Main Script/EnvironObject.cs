namespace Environ
{
    using UnityEngine;
    using EnvironEnum.GeneralEnum;
    using EnvironEnum.DamageEnum;
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
            if (output.Count > 0)
                foreach (EnvironOutput eo in output)
                    eo.source = this;

            effects = new EnvironEffectList();

            if (hitPoints == 0)
                hitPoints = hitPointLimit;
        }

        private void Update()
        {
            if (effects == null) return;

            foreach (EnvironOutput e in effects.inputList)
            {
                if (e.damageOut != null)
                    if (e.damageOut.CanAttack())                       //Passes adjusted damage through UpdateLimit
                        hitPoints -= e.damageOut.UpdateLimit(GetAdjustedDamage(e.damageOut.damage, e.damageOut.ID));

                if (e.appearanceOut != null)
                    e.appearanceOut.UpdateAppearance(); 
            }

            if (hitPoints < 0)
                hitPoints = 0;
            if (hitPoints > hitPointLimit)
                hitPoints = hitPointLimit;
        }

        private float GetAdjustedDamage(float damage, DamageType damageID)
        {
            if (resistances == null)
                return damage;

            return resistances.GetAdjustedDamage(damage, damageID);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEnter(TransferCondition.ON_TRIGGER_ENTER, TransferCondition.ON_TRIGGER_STAY, other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(TransferCondition.ON_TRIGGER_EXIT, TransferCondition.ON_TRIGGER_STAY, other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnEnter(TransferCondition.ON_COLLISION_ENTER, TransferCondition.ON_COLLISION_STAY, collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(TransferCondition.ON_COLLISION_EXIT, TransferCondition.ON_COLLISION_STAY, collision.gameObject);
        }

        private void OnEnter(TransferCondition enter, TransferCondition stay, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output.Count == 0 || otherEO.effects == null)
                return;

            foreach (EnvironOutput eo in output)
                if (eo.transferOnCondition == enter || eo.transferOnCondition == stay)
                    otherEO.effects.Add(eo, obj.transform);
        }
    

    private void OnExit(TransferCondition exit, TransferCondition stay, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output.Count == 0 || otherEO.effects == null)
                return;

            foreach (EnvironOutput eo in output)
            {
                if (eo.transferOnCondition == exit)
                    otherEO.effects.Add(eo, obj.transform);

                else if (eo.transferOnCondition == stay)
                    otherEO.effects.Remove(eo);
            }
        }
    }
}