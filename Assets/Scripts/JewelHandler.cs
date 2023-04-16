using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class JewelHandler : MonoBehaviour
{
    [Header("Jewels")]
    [SerializeField] private GameObject spawnedJewel;
    [SerializeField] private GameObject clickedJewel;

    private Camera mainCamera;
    private int activeScene;
    private ReadJson readJson;

    Vector3 cornerPosition;
    float cornerPositionX;
    float cornerPositionY;
    private float padding = 5f;

    private void Awake()
    {
        readJson = FindObjectOfType<ReadJson>();
        mainCamera = Camera.main;

        cornerPositionX = Screen.currentResolution.width;
        cornerPositionY = Screen.currentResolution.height;
        cornerPosition = new Vector3(cornerPositionX, cornerPositionY, 0f);

        cornerPosition = mainCamera.ScreenToWorldPoint(cornerPosition);
        cornerPositionX = cornerPosition.x;
        cornerPositionY = cornerPosition.y;
    }

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().buildIndex;

        LevelData[] levelData = readJson.LevelData;

        Debug.Log(levelData[3].level_data[0]);

        for(int i = 0; i < levelData[activeScene].level_data.Length; i += 2)
        {
            SpawnNewJewel(
                ReturnSpawnPostion
                (
                    levelData[activeScene].level_data[i],
                    levelData[activeScene].level_data[i + 1]
                )
            );
        }
    }

    private void Update()
    {
        if(!Touchscreen.current.primaryTouch.press.isPressed) { return; }

        Vector2 palietimoKoordinates =
                Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 pasaulioKoordinates = mainCamera.ScreenToWorldPoint(palietimoKoordinates);

        Debug.Log($"Pasaulio: {pasaulioKoordinates}\n Palietimo: {palietimoKoordinates}");
    }

    private void SpawnNewJewel(Vector2 koordinates)
    {
        //koordinates.z = 0f;
        GameObject toSpawn = Instantiate(spawnedJewel, koordinates, Quaternion.identity);
    }

    private Vector2 ReturnSpawnPostion(float posFromFileX, float posFromFileY)
    {
        //float jewelX = levelData[level].level_data[i];
        //float jewelY = levelData[level].level_data[i + 1];
        Vector2 jewelPos = new Vector2();

        if(inRange(posFromFileX, 500f, 1000f) && inRange(posFromFileY, 500f, 1000f))
        {
            jewelPos = new Vector2(
            Map(posFromFileX, 500f, 1000f, 0f, cornerPositionX),
            Map(posFromFileY, 500f, 1000f, 0f, cornerPositionY)
                            );
        }
        else if(inRange(posFromFileX, 0f, 500f) && inRange(posFromFileY, 500f, 1000f))
        {
            jewelPos = new Vector2(
            Map(posFromFileX, 0f, -500f, 0f, cornerPositionX),
            Map(posFromFileY, 500f, 1000f, 0f, cornerPositionY)
                            );
        }
        else if(inRange(posFromFileX, 0f, 500f) && inRange(posFromFileY, 0f, 500f))
        {
         jewelPos = new Vector2(
         Map(posFromFileX, 0f, -500f, 0f, cornerPositionX),
         Map(posFromFileY, 0f, -500f, 0f, cornerPositionY)
                            );
        }
        else if(inRange(posFromFileX, 500f, 1000f) && inRange(posFromFileY, 0f, 500f))
        {
            jewelPos = new Vector2(
            Map(posFromFileX, 500f, 1000f, 0f, cornerPositionX),
            Map(posFromFileY, 0f, -500f, 0f, cornerPositionY)
                            );
        }

        return jewelPos;

        //Debug.Log($"{jewelPos} -- ({jewelX}, {jewelY})");
        //Debug.Log($"{i} -- {levelData[level].level_data.Length}");

        //Instantiate(spawnedJewel, jewelPos, transform.rotation);
    }

    float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - toLow) + toLow;
    }

    bool inRange (float value, float lowerRange, float upperRange)
    {
        if(lowerRange <= value && upperRange >= value) { return true; }
        else { return false; }
    }
}
