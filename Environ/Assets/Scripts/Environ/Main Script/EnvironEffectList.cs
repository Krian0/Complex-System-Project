namespace Environ.Support.EffectList
{
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Main;

    [Serializable]
    public class EnvironEffectList
    {
        public List<EnvironOutput> inputList = new List<EnvironOutput>();
        private List<int> cullIndexesList = new List<int>(); 


        public void Add(EnvironOutput effect, EnvironObject targetEO, EnvironObject lastSourceEO)
        {
            int index = inputList.IndexOf(effect);

            if (index >= 0)
                inputList[index].Refresh();

            if (index < 0)
            {
                EnvironOutput newEffect = UnityEngine.Object.Instantiate(effect);
                newEffect.Setup(targetEO, lastSourceEO);
                inputList.Add(newEffect);
            }
        }

        public void FlagForRemoval(EnvironOutput effect)
        {
            int index = inputList.IndexOf(effect);

            if (index < 0 || cullIndexesList.Contains(index))
                return;

            cullIndexesList.Add(index);
        }

        public void FlagForRemoval(int index)
        {
            if (index < 0 || index >= inputList.Count || cullIndexesList.Contains(index))
                return;

            cullIndexesList.Add(index);
        }

        public void CullInputList()
        {
            if (cullIndexesList.Count == 0)
                return;

            cullIndexesList.Sort();                                     //Sorts the indexes in ascending order
            cullIndexesList = cullIndexesList.Distinct().ToList();      //List now has no duplicates

            for (int i = cullIndexesList.Count - 1; i >= 0; i--)
                Remove(cullIndexesList[i]);                             //Remove in reverse order to avoid removing the wrong index

            cullIndexesList.Clear();
        }

        private void Remove(int index)
        {
            if (index >= inputList.Count)
                return;

            if (inputList[index].appearanceI && inputList[index].appearanceI.objectParticle)
            {
                inputList[index].appearanceI.objectParticle.Stop();
                UnityEngine.Object.Destroy(inputList[index].appearanceI.objectParticle.gameObject, 4);
            }

            inputList.RemoveAt(index);
        }
    }
}
