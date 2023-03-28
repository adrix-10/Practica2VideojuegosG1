using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    Character2DController character;

    [SerializeField]
    TextMeshProUGUI starCount;

    private void Awake()
    {
        character.OnStarCountChanged.AddListener(OnStarCountChanged);
    }

    private void OnStarCountChanged(int value)
    {
        starCount.text = value.ToString(); 
    }
}
