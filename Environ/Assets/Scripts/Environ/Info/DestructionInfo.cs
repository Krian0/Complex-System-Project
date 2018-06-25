namespace Environ.Info
{
    using UnityEngine;
    using System.Collections.Generic;
    using Environ.Support.Base;
    using Environ.Support.Enum.Damage;
    using Environ.Support.Enum.Destruct;
    using Environ.Support.Enum.Tag;
    using Environ.Support.TagList;
    using Environ.Support.Timer;

    [CreateAssetMenu(fileName = "NewDestructionInfo.asset", menuName = "Environ/Info/New DestructionInfo", order = 3)]
    public class DestructionInfo : EnvironBase
    {
        #region Variables
        public DestroyCondition condition;
        public GameObject target;
        public bool destroy = false;

        public List<ObjectTag> searchTags;
        public PauseableTimer limit;
        public List<DamageType> damageTypes;

        public bool spawnObjectOnDestroy;
        public GameObject objectToSpawn;
        #endregion


        #region Setup and Update Functions
        ///<summary> Sets variables up for use. </summary>
        public void Setup(GameObject targetObject)
        {
            destroy = false;
            target = targetObject;

            if (condition == DestroyCondition.TIMER_ZERO)
                limit.ResetTimer();
        }

        ///<summary> Updates destroy based on the condition chosen in the inspector. </summary>
        public void UpdateInfo(float hitPoints)
        {
            if (condition == DestroyCondition.TIMER_ZERO)
            {
                limit.UpdateTimer();
                destroy = (limit.belowZero);
            }

            else if (condition == DestroyCondition.ZERO_HITPOINTS && hitPoints == 0)
                destroy = true;
        }
        #endregion


        #region Check For Match Functions
        ///<summary> Checks if DestructionInfo has a matching DestroyCondition and any matching tags. </summary>
        public void CheckTagsMatch(EnvironTagList tagList, DestroyCondition destroyCondition)
        {
            if (condition == destroyCondition && tagList.MatchingTags(searchTags))
                destroy = true;
        }

        ///<summary> Checks if the condition is for a DamageType, then whether any selected DamageTypes match the given DamageType. </summary>
        public void CheckDamageMatch(DamageType damageID)
        {
            if (condition == DestroyCondition.EFFECT_DAMAGE_TYPE && damageTypes.Contains(damageID))
                destroy = true;
        }
        #endregion


        #region Spawn Functions
        ///<summary> Instantiates objectToSpawn and sets target to inactive. </summary>
        public void Spawn()
        {
            if (spawnObjectOnDestroy && objectToSpawn)
            {
                Instantiate(objectToSpawn, new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z), Quaternion.identity);
                target.SetActive(false);
            }
        }
        #endregion
    }
}