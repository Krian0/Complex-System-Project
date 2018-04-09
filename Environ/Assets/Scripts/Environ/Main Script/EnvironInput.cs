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
            if (!inputList.Contains(output))
                inputList.Add(output);
        }
    }
}
