using UnityEngine;

namespace Environ
{
    namespace Support
    {
        namespace Timer
        {
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
                    if (!pause)
                        if (timer > 0)
                            timer -= value;
                }

                public void ResetTimer()
                {
                    timer = maxTime;
                }

                public bool AtOrBelowZero()
                {
                    return timer <= 0;
                }

                public bool AboveZero()
                {
                    return timer > 0;
                }
            }
        }
    }
}