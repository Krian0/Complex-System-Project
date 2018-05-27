namespace Environ
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;


    [Serializable]
    public class EnvironEffectList
    {
        public List<EnvironOutput> inputList = new List<EnvironOutput>();
        private List<int> cullIndexesList = new List<int>(); 


        public void Add(EnvironOutput effect, Transform objTransform)
        {
            int index = inputList.IndexOf(effect);

            if (index >= 0)
                inputList[index].Refresh();

            if (index < 0)
            {
                EnvironOutput newEffect = UnityEngine.Object.Instantiate(effect);
                newEffect.Setup(objTransform);
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

            cullIndexesList.Sort();            //Sorts the indexes in ascending order
            cullIndexesList.Reverse();         //List is now in descending order

            for (int i = 0; i < cullIndexesList.Count; i++)
                Remove(cullIndexesList[i]);
        }

        private void Remove(int index)
        {
            if (index >= inputList.Count)
                return;

            if (inputList[index].appearanceI != null && inputList[index].appearanceI.objectParticle != null)
            {
                inputList[index].appearanceI.objectParticle.Stop();
                UnityEngine.Object.Destroy(inputList[index].appearanceI.objectParticle.gameObject, 10);
            }

            inputList.RemoveAt(index);
        }
    }
}
