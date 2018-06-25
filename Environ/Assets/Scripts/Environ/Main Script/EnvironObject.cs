namespace Environ.Main
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Environ.Info;
    using Environ.Support.EffectList;
    using Environ.Support.Enum.Destruct;
    using Environ.Support.Enum.General;
    using Environ.Support.TagList;

    public class EnvironObject : MonoBehaviour
    {
        #region Variables
        public float hitPointLimit;
        public float hitPoints = 0;

        public ResistanceInfo resistances;
        public AppearanceInfo appearance;
        public DestructionInfo destruction;
        public EnvironTagList tags = new EnvironTagList();

        public List<EnvironOutput> output;
        public EnvironEffectList effects = new EnvironEffectList();
        #endregion


        #region Start and Update
        ///<summary> Sets up Outputs, Info and variables for use. </summary>
        void Start()
        {
            output.RemoveAll(eOut => !eOut);

            appearance = (appearance) ? Instantiate(appearance) : ScriptableObject.CreateInstance(typeof(AppearanceInfo)) as AppearanceInfo;
            appearance.Setup(gameObject);
            appearance.SetupRenderer();

            if (destruction)
            {
                destruction = Instantiate(destruction);
                destruction.Setup(gameObject);
            }


            for (int i = 0; i < output.Count; i++)
            {
                output[i].firstSource = this;

                if (output[i].similarity == Similarity.UNIQUE)
                {
                    output[i] = Instantiate(output[i]);
                    output[i].SetID();
                }
            }

            SetHitpointsToMax(hitPoints == 0);      //Set hitpoints to hitpoint limit if they were not given a non-zero value
        }

        ///<summary> Updates Appearance and Destruction, removes effects flagged for removal, updates each Effect, contstrains hitPoints. </summary>
        private void Update()
        {
            appearance.UpdateInfo();
            if (destruction)
            {
                destruction.UpdateInfo(hitPoints);
                if (destruction.destroy)
                    StartCoroutine(DestroyThis(0.4f));
            }
          
            effects.CullInputList(this);

            foreach (EnvironOutput eOut in effects.inputList)
                eOut.UpdateOutput(effects, ref hitPoints, resistances);

            ConstrainHitpoints();
        }
        #endregion


        #region Helpers
        ///<summary> Stops all Effect particles, waits the given waitTime, then Spawns the onDestruction object and destroys this. </summary>
        IEnumerator DestroyThis(float waitTime = 3f)
        {
            effects.StopAllParticles(waitTime);
            yield return new WaitForSeconds(waitTime);
            destruction.Spawn();
            Destroy(gameObject);
        }
        #endregion


        #region Hitpoint Functions
        ///<summary> Clamps hitPoints between 0 and hitPointLimit. </summary>
        public void ConstrainHitpoints()
        {
            hitPoints = Mathf.Clamp(hitPoints, 0, hitPointLimit);
        }

        ///<summary> Sets hitPoints to hitPointLimit if given Boolean is true. </summary>
        public void SetHitpointsToMax(bool conditionalValue = true)
        {
            if (conditionalValue)
                hitPoints = hitPointLimit;
        }
        #endregion


        #region OnTrigger & OnCollision Functions
        ///<summary> Adds On_Enter Outputs as an Effect to the TargetEO, as well as Effects with allowTransmission. </summary>
        private void OnEnter(TransferCondition enterTC, DestroyCondition enterDSC, EnvironObject targetEO)
        {
            if (targetEO == null)
                return;

            foreach (EnvironOutput effect in output)
                if (effect.transferCondition == enterTC)
                    CheckAndAdd(effect, targetEO, enterDSC);

            foreach (EnvironOutput tEffect in effects.inputList)
                if (tEffect.allowTransmission && tEffect.transferCondition == enterTC)
                    CheckAndAdd(tEffect, targetEO, enterDSC);
        }

        ///<summary> Adds On_Stay Outputs as an Effect to the TargetEO, as well as Effects with allowTransmission. </summary>
        private void OnStay(TransferCondition stay, EnvironObject targetEO)
        {
            if (targetEO == null)
                return;

            foreach (EnvironOutput effect in output)
                if (effect.transferCondition == stay)
                    CheckAndAdd(effect, targetEO);

            foreach (EnvironOutput tEffect in effects.inputList)
                if (tEffect.allowTransmission && tEffect.transferCondition == stay)
                    CheckAndAdd(tEffect, targetEO);
        }

        ///<summary> Adds On_Exit Outputs as an Effect to the TargetEO, as well as Effects with allowTransmission. Also flags Effects meeting the On_Exit TerminalCondition for removal. </summary>
        private void OnExit(TransferCondition exitTC, TerminalCondition terminalCondition, DestroyCondition exitDSC, EnvironObject targetEO)
        {
            if (targetEO == null)
                return;

            foreach (EnvironOutput effect in effects.inputList)
                if (effect.allowTransmission && effect.transferCondition == exitTC)
                    CheckAndAdd(effect, targetEO, exitDSC);

            foreach (EnvironOutput eOut in output)
            {
                if (eOut.transferCondition == exitTC)
                    CheckAndAdd(eOut, targetEO, exitDSC);

                if (eOut.endOnCondition == terminalCondition)
                    targetEO.effects.FlagForRemoval(eOut);
            }
        }

        ///<summary> Checks targetEO DestructionInfo on matching DestroyConditions, adds effect to targetEO's Effects list. </summary>
        private void CheckAndAdd(EnvironOutput effect, EnvironObject targetEO, DestroyCondition destructCondition)
        {
            if (targetEO.destruction)
            {
                targetEO.destruction.CheckTagsMatch(tags, destructCondition);
                if (effect.damageI)
                    targetEO.destruction.CheckDamageMatch(effect.damageI.ID);
            }

            targetEO.effects.Add(effect, targetEO, this);
        }

        ///<summary> Checks targetEO DestructionInfo on matching DestroyConditions, adds effect to targetEO's Effects list. </summary>
        private void CheckAndAdd(EnvironOutput effect, EnvironObject targetEO)
        {
            if (targetEO.destruction && effect.damageI)
                targetEO.destruction.CheckDamageMatch(effect.damageI.ID);

            targetEO.effects.Add(effect, targetEO, this);
        }


        private void OnCollisionEnter(Collision collision)
        {
            OnEnter(TransferCondition.COLLISION_ENTER, DestroyCondition.COLLISION_ENTER, collision.gameObject.GetComponent<EnvironObject>());
        }

        private void OnCollisionStay(Collision collision)
        {
            OnStay(TransferCondition.COLLISION_STAY, collision.gameObject.GetComponent<EnvironObject>());
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(TransferCondition.COLLISION_EXIT, TerminalCondition.COLLISION_EXIT, DestroyCondition.COLLISION_EXIT, collision.gameObject.GetComponent<EnvironObject>());
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEnter(TransferCondition.TRIGGER_ENTER, DestroyCondition.TRIGGER_ENTER, other.GetComponent<EnvironObject>());
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay(TransferCondition.TRIGGER_STAY, other.GetComponent<EnvironObject>());
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(TransferCondition.TRIGGER_EXIT, TerminalCondition.TRIGGER_EXIT, DestroyCondition.TRIGGER_EXIT, other.GetComponent<EnvironObject>());
        }
        #endregion
    }
}