namespace Environ.Support.Timer
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PauseableTimer
    {
        #region Variables
        public float maxTime;
        public float timer;
        public bool pause;

        public bool aboveZero { get { return timer > 0; } }
        public bool belowZero { get { return timer <= 0; } }
        #endregion


        #region Update and Reset Functions
        ///<summary> Updates the timer if it isn't paused by subtracting Time.deltaTime. </summary>
        public void UpdateTimer()
        {
            if (!pause && timer > 0)
                timer -= Time.deltaTime;
        }

        ///<summary> Updates the timer if it isn't paused by subtracting the given float value. </summary>
        public void UpdateTimer(float value)
        {
            if (!pause && timer > 0)
                timer -= value;
        }

        ///<summary> Resets timer to maxTime. </summary>
        public void ResetTimer()
        {
            timer = maxTime;
        }
        #endregion
    }
}