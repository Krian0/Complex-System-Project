namespace Environ
{
    using UnityEngine;
    using EnvironEnum.DamageEnum;
    using EnvironInfo;
    using EnvironContainers;

    public class EnvironObject : MonoBehaviour
    {
        public float hitPointLimit;
        public float hitPoints;
        public ResistanceInfo resistances;
        public AppearanceInfo appearance;
        //public List<DestructionInfo> destroyConditions;

        public EnvironOutput output;
        public EnvironInput effects;


        void Start()
        {
            if (output != null)
                output.source = this;

            if (effects != null)
                effects = Instantiate(effects);

            if (hitPoints == 0)
                hitPoints = hitPointLimit;
        }

        private void Update()
        {
            if (effects == null) return;

            foreach (EnvironOutput effect in effects.inputList)
            {
                DamageInfo damageOut = effect.damageOut;

                damageOut.UpdateLimits();
                if (damageOut.CanAttack())
                {
                    float damageValue = GetAdjustedDamage(damageOut.damage, damageOut.ID);
                    hitPoints -= damageValue;
                    if (damageOut.regularity == DamageRegularity.DAMAGE_LIMIT)
                        damageOut.limitTracker -= damageValue;
                }
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
            OnEnter(DamageCondition.ON_TRIGGER_ENTER, DamageCondition.ON_TRIGGER_STAY, other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(DamageCondition.ON_TRIGGER_EXIT, DamageCondition.ON_TRIGGER_STAY, other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnEnter(DamageCondition.ON_COLLISION_ENTER, DamageCondition.ON_COLLISION_STAY, collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(DamageCondition.ON_COLLISION_EXIT, DamageCondition.ON_COLLISION_STAY, collision.gameObject);
        }

        private void OnEnter(DamageCondition enter, DamageCondition stay, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output == null || output.damageOut == null || otherEO.effects == null)
                return;

            if (output.damageOut.transferOnCondition == enter || output.damageOut.transferOnCondition == stay)
                otherEO.effects.Add(output);
        }

        private void OnExit(DamageCondition exit, DamageCondition stay, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output == null || output.damageOut == null || otherEO.effects == null)
                return;

            if (output.damageOut.transferOnCondition == exit)
                otherEO.effects.Add(output);

            else if (output.damageOut.transferOnCondition == stay)
                otherEO.effects.Remove(output);
        }


        //void Update()
        //{
        //    if (input.inputList.Count == 0) return;


        //    for (int i = 0; i < input.inputList.Count; i++)
        //    {
        //        input.inputList[i].damageOut.UpdateTimers();
        //        if (input.inputList[i].damageOut.CanAttack())
        //            ApplyDamage(input.inputList[i].damageOut);

        //        //        ApplyDamage(input.inputList[i].damageOut);
        //        //ApplyAppearance(input.inputList[i].appearanceOut);
        //        //HandleDestructionInfo(input.inputList[i].destroyConditionOut);
        //    }
        //}


        //private void ApplyDamage(DamageInfo DI)
        //{
        //    if (DI.IsValidDamageType())
        //        hitPoints -= GetResistanceAdjustedDamage(DI.damage, DI.ID);
        //}

        //private void ApplyAppearance(AppearanceInfo AI)
        //{

        //}

        //private void HandleDestructionInfo(DestructionInfo DSI)
        //{

        //}


        //private void OnCollisionStay(Collision collision)
        //{
        //    EnvironObject EO = collision.transform.gameObject.GetComponent<EnvironObject>();

        //    if (EO != null)
        //        EO.input.AddToInputs(output);
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    EnvironObject EO = other.transform.gameObject.GetComponent<EnvironObject>();

        //    if (EO != null)
        //        EO.input.AddToInputs(output);
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    EnvironObject EO = other.transform.gameObject.GetComponent<EnvironObject>();

        //    if (EO != null)
        //        EO.input.AddToInputs(output);
        //}

        //private void OnTriggerStay(Collider other)
        //{
        //    EnvironObject EO = other.transform.gameObject.GetComponent<EnvironObject>();

        //    if (EO != null)
        //        EO.input.AddToInputs(output);
        //}
    }
}