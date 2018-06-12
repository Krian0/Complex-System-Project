using System;
using UnityEngine;

namespace Environ.Support.Timer
{
    [Serializable]
    public class PauseableTimer
    {
        public float maxTime;
        public float timer;
        public bool pause;

        public void UpdateTimer()
        {
            if (!pause)
                if (timer > 0)
                    timer -= Time.deltaTime;
        }

        public void UpdateTimer(float value)
        {
            if (!pause && timer > 0)
                timer -= value;
        }

        public void ResetTimer()
        {
            timer = maxTime;
        }

        public bool AboveZero()
        {
            return timer > 0;
        }
    }
}