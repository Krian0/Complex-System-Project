namespace Environ.Support.EffectList
{
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Main;
    using Timer;

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
                inputList.Add(UnityEngine.Object.Instantiate(effect));
                inputList[inputList.Count - 1].Setup(targetEO, lastSourceEO);
            }

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

        public void CullInputList()
        {
            foreach (EnvironOutput eo in cullList)
                if (eo.appearanceI)
                    eo.appearanceI.StopAndDestroy();

            var setToRemove = new HashSet<EnvironOutput>(cullList);
            inputList.RemoveAll(x => setToRemove.Contains(x));
            

            cullList.Clear();
        }
    }
}
