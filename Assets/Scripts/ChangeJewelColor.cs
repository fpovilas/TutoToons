using UnityEngine;

public class ChangeJewelColor : MonoBehaviour
{
    SpawnJewels spawnJewels;

    void Start()
    {
        spawnJewels = FindObjectOfType<SpawnJewels>();
    }

    //void OnClick(InputValue value) { Debug.Log("Click"); }
}