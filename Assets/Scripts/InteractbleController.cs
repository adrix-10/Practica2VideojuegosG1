using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractbleController : MonoBehaviour
{
    const string KEY = "key";
    
    [SerializeField]
    Character2DController character;

    [SerializeField]
    string interableObject;

    [SerializeField]
    int value = 1;

    private void Awake()
    {
        character.OnInteract.AddListener(OnInteract);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (character.ContainsCollectible(interableObject, value))
            {
                character.ReduceCollectible(interableObject, value);
            }
        } 
    }

    void OnInteract(string collectibleType, int value)
    {
        if (collectibleType.Equals(KEY, StringComparison.OrdinalIgnoreCase))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
