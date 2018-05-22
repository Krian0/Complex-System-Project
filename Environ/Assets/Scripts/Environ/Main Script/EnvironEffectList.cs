namespace Environ
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    [Serializable]
    public class EnvironEffectList
    {
        public List<EnvironOutput> inputList = new List<EnvironOutput>();


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

        public void Remove(EnvironOutput effect)
        {
            int index = inputList.IndexOf(effect);

            if (inputList[index].appearanceI != null && inputList[index].appearanceI.objectParticle != null)
            {
                inputList[index].appearanceI.objectParticle.Stop();
                UnityEngine.Object.Destroy(inputList[index].appearanceI.objectParticle.gameObject, 10);
            }

            if (index >= 0)
                inputList.RemoveAt(index);
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= inputList.Count)
                return;

            inputList.RemoveAt(index);
        }
    }
}
