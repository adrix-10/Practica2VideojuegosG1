using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField]
    string collectibleType;

    [SerializeField]
    int value;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character2DController controller = other.GetComponent<Character2DController>();

            if (controller != null)
            {
                controller.IncreaseCollectible(collectibleType, value);
            }

            //El objeto al ser tocado por el jugar se destruira (estrella, moneda, etc...)
            Destroy(gameObject);
        } 
    }
}
