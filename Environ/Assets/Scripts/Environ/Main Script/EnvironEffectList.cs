namespace Environ.Support.EffectList
{
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Main;
    using Info;
    using UnityEngine;

    [Serializable]
    public class EnvironEffectList
    {
        public List<EnvironOutput> inputList = new List<EnvironOutput>();
        private List<EnvironOutput> cullList = new List<EnvironOutput>(); 


        public void Add(EnvironOutput effect, EnvironObject targetEO, EnvironObject lastSourceEO)
        {
            List<EnvironOutput> similarEffectList = inputList.FindAll(sEffect => sEffect == effect);

            if (similarEffectList.Count == 0)
            {
                EnvironOutput newEffect = UnityEngine.Object.Instantiate(effect);
                newEffect.Setup(targetEO, lastSourceEO);

                if (HasHighestMaterialPriority(newEffect))
                    newEffect.appearanceI.SetRendererMaterial();

                inputList.Add(newEffect);
            }

            else
                foreach (EnvironOutput eo in similarEffectList)
                    eo.Refresh();
        }

        public void ConditionalFlagForRemoval(bool value, EnvironOutput effect)
        {
            if (value)
                cullList.Add(effect);
        }

        public void FlagForRemoval(EnvironOutput effect)
        {
            cullList.Add(effect);
        }

        public void CullInputList(EnvironObject targetEO)
        {
            if (cullList.Count == 0)
                return;

            foreach (EnvironOutput eOut in cullList)
                if (eOut.appearanceI)
                    eOut.appearanceI.StopAndDestroy();

            var setToRemove = new HashSet<EnvironOutput>(cullList);
            inputList.RemoveAll(x => setToRemove.Contains(x));

            cullList.Clear();

            SetMaterialAppearance(targetEO);
        }

        private void SetMaterialAppearance(EnvironObject targetEO)
        {
            if (inputList.Count == 0)
            {
                targetEO.appearance.SetRendererMaterial();
                return;
            }

            List<EnvironOutput> copy = inputList.OrderBy(eOut => eOut.appearanceI.priority).ToList();
            //AppearanceInfo newAppearance = copy.Where(eOut => eOut.appearanceI && eOut.appearanceI.material && 
            //                                          eOut.appearanceI.materialOn).Select(eOut => eOut.appearanceI).First();

            AppearanceInfo newAppearance = copy.Where(eOut => HasUseableMaterial(eOut)).Select(eOut => eOut.appearanceI).First();


            if (newAppearance)                                  //Sets the meshRenderer material of targetEO to:
                newAppearance.SetRendererMaterial();            //newAppearance material (from Effect in inputList)
            else
                targetEO.appearance.SetRendererMaterial();      //TargetEO's appearance material (reset)
        }

        private bool HasUseableMaterial(EnvironOutput effect)
        {
            return effect.appearanceI && effect.appearanceI.material && effect.appearanceI.materialOn;
        }

        private bool HasHighestMaterialPriority(EnvironOutput effect)
        {
            if (!HasUseableMaterial(effect))
                return false;

            if (inputList.Count == 0)
                return true;

            return !inputList.Exists(eOut => HasUseableMaterial(eOut) && eOut.appearanceI.priority < effect.appearanceI.priority);
        }
    }
}
