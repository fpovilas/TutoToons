using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private int jewelToTouch = 0;
    internal List<GameObject> jewelObjects;
    private List<TextMeshProUGUI> jewelNumber;
    private bool[] isPressed;
    private bool isLast = false;

    //Linehandler
    [Header("Line Handler")]
    [SerializeField] private GameObject lineHandlerPrefab;
    internal List<GameObject> lineHandler;
    private int lineHandlerCounter = 0;

    //SelectedLevel
    SelectedLevel selectedLevel;

    //Game Manager
    GameManager gameManager;
    bool isLoaded = false;

    #endregion

    private void Awake()
    {
        readJson = FindObjectOfType<ReadJson>();
        mainCamera = Camera.main;
        selectedLevel = FindObjectOfType<SelectedLevel>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        jewelObjects = new List<GameObject>();
        jewelNumber = new List<TextMeshProUGUI>();
        lineHandler = new List<GameObject>();
    }

    private void Update()
    {
        if(selectedLevel.SceneToLoad != -1 && !isLoaded)
        {
            LoadLevel();
        }
        if(!Touchscreen.current.primaryTouch.press.isPressed) { return; }

        Vector2 touchCoordinates =
                Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldSpaceCoordinates =
                mainCamera.ScreenToWorldPoint(touchCoordinates);

        if((jewelToTouch == PressedJewelIndex(worldSpaceCoordinates))
            && (isPressed[jewelToTouch] == false))
          {
            ChangeJewelSprite();
            isPressed[jewelToTouch] = true;

            StartCoroutine(Fade(jewelNumber[jewelToTouch]));

            CheckHowToDrawAndDraw();
          }
    }

    public int ActiveScene {get; set;}

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

        if(inRange(posFromFileX, 500f, 1000f) && inRange(posFromFileY, 0f, 500f))
        {
            jewelPos = new Vector2(
                Map(posFromFileX, 500f, 1000f, 0f, cornerPositionX),
                Map(posFromFileY, 0f, 500f, cornerPositionY, 0f)
                            );
        }
        else if(inRange(posFromFileX, 0f, 500f) && inRange(posFromFileY, 0f, 500f))
        {
            jewelPos = new Vector2(
                Map(posFromFileX, 0f, 500f, -cornerPositionX, 0f),
                Map(posFromFileY, 0f, 500f, cornerPositionY, 0f)
                            );            
        }
        else if(inRange(posFromFileX, 0f, 500f) && inRange(posFromFileY, 500f, 1000f))
        {
            jewelPos = new Vector2(
                Map(posFromFileX, 0f, 500f, -cornerPositionX, 0f),
                Map(posFromFileY, 500f, 1000f, 0f, -cornerPositionY)
                            );            
        }
        else if(inRange(posFromFileX, 500f, 1000f) && inRange(posFromFileY, 500f, 1000f))
        {
            jewelPos = new Vector2(
                Map(posFromFileX, 500f, 1000f, 0f, cornerPositionX),
                Map(posFromFileY, 500f, 1000f, 0f, -cornerPositionY)
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

    private void ChangeJewelSprite()
    {
        jewelObjects[jewelToTouch].GetComponent<SpriteRenderer>().sprite =
                clickedJewelPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    #endregion

    #region "Generic methods"
    
    private void SetCornerPositions()
    {
        cornerPositionX = Screen.currentResolution.width;
        cornerPositionY = Screen.currentResolution.height;
        cornerPosition = new Vector3(cornerPositionX, cornerPositionY, 0f);

        cornerPosition = mainCamera.ScreenToWorldPoint(cornerPosition);
        cornerPositionX = cornerPosition.x - 1f;
        cornerPositionY = cornerPosition.y - 1f;
    }

    private float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
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

    private void CheckHowToDrawAndDraw()
    {
        if(jewelToTouch > 0 && jewelToTouch < lineHandler.Count)
        {
            StartCoroutine(DrawJewels());
        }

        if(lineHandlerCounter + 1 == jewelObjects.Count && isLast)
        {
            StartCoroutine(DrawLastJewel());
        }

        if(jewelToTouch + 1 < jewelObjects.Count)
        {
            jewelToTouch++;
            isLast = true;
        }
    }

    IEnumerator DrawJewels()
    {
        DrawLineFromPointToPoint(lineHandlerCounter, jewelToTouch);
        yield return new WaitForSecondsRealtime(1f);
    }

    IEnumerator DrawLastJewel()
    {
        yield return new WaitForSecondsRealtime(1f);
        DrawLineFromPointToPoint(lineHandlerCounter, 0);
        yield return new WaitForSecondsRealtime(1f);
        gameManager.ResetSession();
    }

    private void DrawLineFromPointToPoint(int from, int to)
    {
        lineHandler[from].GetComponent<LineHandler>().DrawLine(
            jewelObjects[from].transform.position,
            jewelObjects[to].transform.position);
        lineHandlerCounter++;
    }

    private void LoadLevel()
    {
        SetCornerPositions();
        activeScene = selectedLevel.SceneToLoad;
        int[] levelData = readJson.GetCurrentLevelLevelData(activeScene);

        for(int i = 0; i < levelData.Length; i += 2)
        {
            SpawnNewJewel(
                ReturnSpawnPostion
                (
                    levelData[i],
                    levelData[i + 1]
                ));

            number++;
            lineHandler.Add(
                        Instantiate(
                                    lineHandlerPrefab,
                                    new Vector3(0f, 0f, 0f),
                                    Quaternion.identity));
        }

        isPressed = new bool[levelData.Length];
        isLoaded = true;
    }

    #endregion
}