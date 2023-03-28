using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCicleManager : MonoBehaviour
{
    [SerializeField]
    Light2D globalLight;

    [SerializeField]
    LightCicleType[] cicleTypes;

    [SerializeField]
    float cicleTime = 10.0F;

    float currentCicleTime = 0.0F;
    float percentageCicle = 0.0F;

    int currentCicle = 0;
    int nextCicle = 1;

    void Start()
    {
        globalLight.color = cicleTypes[currentCicle].color; 
    }

    void Update()
    {
        currentCicleTime += Time.deltaTime; 
        percentageCicle = currentCicleTime / cicleTime;

        if (currentCicleTime >= cicleTime)
        {
            currentCicleTime = 0.0F;
            currentCicle = nextCicle;

            nextCicle++;
            if (nextCicle >= cicleTypes.Length)
            {
                nextCicle = 0;
            }
        }

        ChangeLightColor(cicleTypes[currentCicle].color, cicleTypes[nextCicle].color);
    }

    void ChangeLightColor(Color currentColor, Color nextColor) 
    { 
        globalLight.color = Color.Lerp(currentColor, nextColor, percentageCicle);
    }
}
