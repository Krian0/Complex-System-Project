namespace Environ
{
    using UnityEngine;
    using System.Collections.Generic;
    using EnvironInfo;
    using EnvironEnum.DamageEnum;

    public class EnvironObject : MonoBehaviour
    {
        public float hitPoints;
        public ResistanceInfo resistances;
        public AppearanceInfo appearance;
        public List<DestructionInfo> destroyConditions;

        public EnvironOutput output;
        public EnvironInput input;


        void Start()
        {

        }

        void Update()
        {
            if (input.inputList.Count == 0) return;


            for (int i = 0; i < input.inputList.Count; i++)
            {
                input.inputList[i].damageOut.UpdateTimers();
                if (input.inputList[i].damageOut.CanAttack())
                    ApplyDamage(input.inputList[i].damageOut);

                //        ApplyDamage(input.inputList[i].damageOut);
                //ApplyAppearance(input.inputList[i].appearanceOut);
                //HandleDestructionInfo(input.inputList[i].destroyConditionOut);
            }
        }


        private void ApplyDamage(DamageInfo DI)
        {
            if (DI.IsValidDamageType())
                hitPoints -= GetResistanceAdjustedDamage(DI.damage, DI.ID);
        }

        private void ApplyAppearance(AppearanceInfo AI)
        {

        }

        private void HandleDestructionInfo(DestructionInfo DSI)
        {

        }



        private float GetResistanceAdjustedDamage(float damage, DamageType damageID)
        {
            for (int i = 0; i < resistances.resistanceList.Count; i++)
                if (resistances.resistanceList[i].ResistanceExists(damageID))
                    return resistances.resistanceList[i].GetAdjustedDamage(damage);

            return damage;
        }

        private void OnCollisionStay(Collision collision)
        {
            EnvironObject EO = collision.transform.gameObject.GetComponent<EnvironObject>();

            if (EO != null)
                EO.input.AddToInputs(output);
        }

        private void OnTriggerEnter(Collider other)
        {
            EnvironObject EO = other.transform.gameObject.GetComponent<EnvironObject>();

            if (EO != null)
                EO.input.AddToInputs(output);
        }

        private void OnTriggerExit(Collider other)
        {
            EnvironObject EO = other.transform.gameObject.GetComponent<EnvironObject>();

            if (EO != null)
                EO.input.AddToInputs(output);
        }

        private void OnTriggerStay(Collider other)
        {
            EnvironObject EO = other.transform.gameObject.GetComponent<EnvironObject>();

            if (EO != null)
                EO.input.AddToInputs(output);
        }
    }
}