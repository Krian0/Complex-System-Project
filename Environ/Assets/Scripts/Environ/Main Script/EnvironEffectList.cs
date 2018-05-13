namespace Environ
{
    using UnityEngine;
    using System.Collections.Generic;

    public class EnvironEffectList
    {
        public List<EnvironOutput> inputList = new List<EnvironOutput>();


        public void Add(EnvironOutput output, Transform objTransform)
        {
            int index = inputList.IndexOf(output);

            if (index >= 0)
            {
                if (inputList[index].damageOut.refreshDelay)
                    inputList[index].damageOut.ResetDelay();

                if (inputList[index].damageOut.refreshLimit)
                    inputList[index].damageOut.ResetLimit();
            }

            if (index < 0)
            {
                EnvironOutput copy = Object.Instantiate(output);

                if (copy.damageOut != null)
                {
                    copy.damageOut = Object.Instantiate(copy.damageOut);
                    copy.damageOut.Setup();
                }

                if (copy.appearanceOut != null)
                {
                    copy.appearanceOut = Object.Instantiate(copy.appearanceOut);
                    copy.appearanceOut.Setup(objTransform);
                }

                //if (copy.destroyConditionOut != null)
                //{
                //    copy.destroyConditionOut = Instantiate(copy.destroyConditionOut);
                //    copy.destroyConditionOut.Setup();
                //}

                inputList.Add(copy);
            }
        }

        public void Remove(EnvironOutput output)
        {
            int index = inputList.IndexOf(output);

            if (inputList[index].appearanceOut != null && inputList[index].appearanceOut.objectParticle != null)
            {
                inputList[index].appearanceOut.objectParticle.Stop();
                Object.Destroy(inputList[index].appearanceOut.objectParticle.gameObject, 10);
            }

            if (index >= 0)
                inputList.RemoveAt(index);
        }
    }
}
