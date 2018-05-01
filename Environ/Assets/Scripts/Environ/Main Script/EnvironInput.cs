namespace Environ
{
    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "NewInput.asset", menuName = "Environ/New Input", order = 1)]
    public class EnvironInput : ScriptableObject
    {
        public List<EnvironOutput> inputList;

        public void Add(EnvironOutput output)
        {
            int index = inputList.IndexOf(output);

            if (index >= 0)
                if (inputList[index].damageOut.refreshLimits && !inputList[index].damageOut.HasNoLimit())
                    inputList[index].damageOut.RefreshLimit();

            if (index < 0)
            {
                EnvironOutput copy = Instantiate(output);

                if (copy.damageOut != null)
                    copy.damageOut = Instantiate(copy.damageOut);

                if (copy.destroyConditionOut != null)
                    copy.destroyConditionOut = Instantiate(copy.destroyConditionOut);

                if (copy.appearanceOut != null)
                    copy.appearanceOut = Instantiate(copy.appearanceOut);

                inputList.Add(copy);
            }
        }

        public void Remove(EnvironOutput output)
        {
            int index = inputList.IndexOf(output);

            if (index >= 0)
                inputList.RemoveAt(index);
        }
    }
}
