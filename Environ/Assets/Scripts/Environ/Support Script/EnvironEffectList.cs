namespace Environ.Support.EffectList
{
    using System;
    using System.Collections.Generic;
    using Environ.Info;
    using Environ.Main;

    [Serializable]
    public class EnvironEffectList
    {
        #region Variables
        public List<EnvironOutput> inputList = new List<EnvironOutput>();
        private List<EnvironOutput> cullList = new List<EnvironOutput>();
        #endregion


        #region Add and Remove Functions
        ///<summary> Finds all Effects in the inputList matching the given Output. If there are none, the Effect will be added to inputList, otherwise the matching Effects are refreshed. </summary>
        public void Add(EnvironOutput effect, EnvironObject targetEO, EnvironObject lastSourceEO)
        {
            List<EnvironOutput> effectList = inputList.FindAll(e => e == effect);

            if (effectList.Count > 0)
            {
                foreach (EnvironOutput eOut in effectList)
                    eOut.Refresh();
                return;
            }

            EnvironOutput newEffect = UnityEngine.Object.Instantiate(effect);
            newEffect.Setup(targetEO, lastSourceEO);

            if (inputList.Count == 0 || IsPriority(newEffect.appearanceI))      //If the given Effect has a valid material Appearance that takes priority
                newEffect.appearanceI.SetRendererMaterial();                    //The material will be added to the Effect's set AppearanceI MeshRenderer.

            inputList.Add(newEffect);
        }

        ///<summary> Adds the given Effect to cullList if inputList contains it and the given Boolean is true. </summary>
        public void ConditionalFlagForRemoval(bool value, EnvironOutput effect)
        {
            if (value && inputList.Contains(effect))
                cullList.Add(effect);
        }

        ///<summary> Adds the given Effect to cullList if inputList contains it. </summary>
        public void FlagForRemoval(EnvironOutput effect)
        {
            if (inputList.Contains(effect))
                cullList.Add(effect);
        }

        ///<summary> Stops and destroys the particles of Effects in the cullList, then removes them from inputList and sets targetEO's appearance. </summary>
        public void CullInputList(EnvironObject targetEO)
        {
            if (cullList.Count == 0)
                return;

            var setToRemove = new HashSet<EnvironOutput>(cullList);
            foreach(EnvironOutput eOut in setToRemove)
                if (eOut.appearanceI)
                    eOut.appearanceI.StopAndDestroy();

            inputList.RemoveAll(x => setToRemove.Contains(x));
            cullList.Clear();
            SetMaterialAppearance(targetEO);
        }
        #endregion


        #region Helper Functions
        ///<summary> Stops all existing particles in Effects in the inputList. </summary>
        public void StopAllParticles(float waitTime = 3f)
        {
            foreach (EnvironOutput eOut in inputList)
                if (eOut.appearanceI && eOut.appearanceI.particle)
                    eOut.appearanceI.particle.Stop();
        }

        ///<summary> Determines the priority material AppearanceInfo in the inputList and applies it to the target MeshRenderer. </summary>
        private void SetMaterialAppearance(EnvironObject targetEO)
        {
            if (inputList.Count > 0)
            {
                AppearanceInfo apInfo = inputList.Find(eOut => eOut.appearanceI && eOut.appearanceI.canUseMaterial).appearanceI;

                if (apInfo)                                         //Sets the meshRenderer material of targetEO to:
                    apInfo.SetRendererMaterial();                   //apInfo material (from Effect in inputList)
                else
                    targetEO.appearance.SetRendererMaterial();      //TargetEO's appearance material (reset)
            }

            else
                targetEO.appearance.SetRendererMaterial();
        }

        ///<summary> Checks if there's a different priority Appearance in the inputList. Returns true if the given AppearanceInfo is the priority. </summary>
        private bool IsPriority(AppearanceInfo apInfo)
        {
            if (apInfo && apInfo.canUseMaterial)
                return !inputList.Exists(eOut => eOut.appearanceI && eOut.appearanceI.IsPriority(apInfo));

            return false;
        }
        #endregion
    }
}
