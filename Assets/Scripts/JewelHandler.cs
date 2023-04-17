using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class JewelHandler : MonoBehaviour
{
    #region "Variables"
    [Header("Jewels")]
    [SerializeField] private GameObject spawnedJewelPrefab;
    [SerializeField] private GameObject clickedJewelPrefab;

    //Camera variables
    private Camera mainCamera;
    private int activeScene;

    //Reading level spawn positions;
    private ReadJson readJson;

    //Getting device resolution
    Vector3 cornerPosition;
    float cornerPositionX;
    float cornerPositionY;

    //Jewels variables
    private int number = 1;
    private List<GameObject> jewelObjects;
    private bool[] isPressed;
    private int currentJewelToTouch = 0;
    private List<TextMeshProUGUI> jewelNumber;
    private bool isLast = false;

    //Linehandler
    [Header("Line Handler")]
    [SerializeField] private GameObject lineHandlerPrefab;
    private List<GameObject> lineHandler;
    private int lineHandlerCounter = 0;

    #endregion

    private void Awake()
    {
        readJson = FindObjectOfType<ReadJson>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        jewelObjects = new List<GameObject>();
        jewelNumber = new List<TextMeshProUGUI>();
        lineHandler = new List<GameObject>();

        SetCornerPositions();

        activeScene = 3;//SceneManager.GetActiveScene().buildIndex;

        int[] levelData = readJson.GetCurrentLevelLevelData(activeScene);

        for(int i = 0; i < levelData.Length; i += 2)
        {
            SpawnNewJewel(
                ReturnSpawnPostion
                (
                    levelData[i],
                    levelData[i + 1]
                )
            );
            number++;
            //Debug.Log(FindObjectsOfType<TextMeshProUGUI>().Length);
            lineHandler.Add(
                Instantiate(
                    lineHandlerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity));
        }
        isPressed = new bool[levelData.Length];
    }

    private void Update()
    {
        if(!Touchscreen.current.primaryTouch.press.isPressed) { return; }

        Vector2 palietimoKoordinates =
                Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 pasaulioKoordinates =
                mainCamera.ScreenToWorldPoint(palietimoKoordinates);

        if((currentJewelToTouch == PressedJewelIndex(pasaulioKoordinates))
            && (isPressed[currentJewelToTouch] == false))
          {
            jewelObjects[currentJewelToTouch].GetComponent<SpriteRenderer>().sprite =
                clickedJewelPrefab.GetComponent<SpriteRenderer>().sprite;
            isPressed[currentJewelToTouch] = true;

            StartCoroutine(Fade(jewelNumber[currentJewelToTouch]));

            if(currentJewelToTouch > 0 && currentJewelToTouch < lineHandler.Count)
            {
                lineHandler[lineHandlerCounter].GetComponent<LineHandler>().DrawLine(
                    jewelObjects[lineHandlerCounter].transform.position,
                    jewelObjects[currentJewelToTouch].transform.position);
                    lineHandlerCounter++;
            }

            if(lineHandlerCounter + 1 == jewelObjects.Count && isLast)
            {
                lineHandler[lineHandlerCounter].GetComponent<LineHandler>().DrawLine(
                    jewelObjects[lineHandlerCounter].transform.position,
                    jewelObjects[0].transform.position);
                    lineHandlerCounter++;
            }

            //Debug.Log($"{jewelObjects[currentJewelToTouch].transform.position} ?? {jewelObjects[nextJewelToTouch].transform.position}");

            if(currentJewelToTouch + 1 < jewelObjects.Count)
            {
                currentJewelToTouch++;
                isLast = true;
            }
            //Debug.Log($"{currentJewelToTouch} {nextJewelToTouch}");
          }
        //Debug.Log($"Pasaulio: {pasaulioKoordinates}\n Palietimo: {palietimoKoordinates}");
        //Debug.Log($"{distance}");
        //if(distance < jewelObjects[0].GetComponent<CircleCollider2D>().radius) {Debug.Log("Touched");}
    }

    #region "Jewel methods"

    private void SpawnNewJewel(Vector2 koordinates)
    {
        //koordinates.z = 0f;
        GameObject toSpawn = Instantiate(spawnedJewelPrefab, koordinates, Quaternion.identity);
        FindObjectOfType<TextMeshProUGUI>().text = number.ToString();
        jewelObjects.Add(toSpawn);
        jewelNumber.Add(FindObjectOfType<TextMeshProUGUI>());
    }

    private Vector2 ReturnSpawnPostion(float posFromFileX, float posFromFileY)
    {
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
    }

    private int PressedJewelIndex(Vector2 touchCoordinates)
    {
        float distance;
        int index = 0;
        foreach(var jewel in jewelObjects)
        {
            distance = Vector2.Distance(touchCoordinates, jewel.transform.position);
            if(distance < jewel.GetComponent<CircleCollider2D>().radius) { return index;}
            else { index++; }
        }
        return -1;
    }

    #endregion

    #region "Generic methods"
    
    private void SetCornerPositions()
    {
        cornerPositionX = Screen.currentResolution.width;
        cornerPositionY = Screen.currentResolution.height;
        cornerPosition = new Vector3(cornerPositionX, cornerPositionY, 0f);

        cornerPosition = mainCamera.ScreenToWorldPoint(cornerPosition);
        cornerPositionX = cornerPosition.x;
        cornerPositionY = cornerPosition.y;
    }

    private float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - toLow) + toLow;
    }

    private bool inRange (float value, float lowerRange, float upperRange)
    {
        if(lowerRange <= value && upperRange >= value) { return true; }
        else { return false; }
    }

    IEnumerator Fade(TextMeshProUGUI textToFade)
    {
        for(float alpha = 1f; alpha >= 0; alpha -= 0.1f)        
        {
            textToFade.alpha = alpha;
            yield return new WaitForSeconds(.1f);
        }
    }

    #endregion
}