using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    float maximumTime = 180.0F;

    float currentTime;

    bool timerEnabled = true;

    void Awake()
    {
        currentTime = maximumTime;
        slider.maxValue = maximumTime;
        slider.value = currentTime;
    }

    void Update()
    {
        OnTimerChanged();
    }

    void OnTimerChanged()
    {
        if (!timerEnabled)
        {
            return;
        }

        currentTime -= Time.deltaTime;
        if (currentTime > 0.0F && currentTime < maximumTime)
        {
            slider.value = currentTime; 
        }
        else
        {
            timerEnabled = false;
        }
    }
}
