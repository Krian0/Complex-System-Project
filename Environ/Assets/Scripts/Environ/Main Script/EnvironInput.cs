namespace Environ
{
    using UnityEngine;
    using System.Collections.Generic;
    using EnvironInfo;

    [CreateAssetMenu(fileName = "NewInput.asset", menuName = "Environ/New Input", order = 1)]
    public class EnvironInput : ScriptableObject
    {
        public List<EnvironOutput> inputList;

        public void AddToInputs(EnvironOutput output)
        {
            EnvironOutput match = null;
            foreach(EnvironOutput eo in inputList)
            {
                if (eo == output)
                    match = eo;
            }

            if (match == null)
                inputList.Add(Instantiate(output));

            else
                match.RefreshLimit();
        }
    }
}
