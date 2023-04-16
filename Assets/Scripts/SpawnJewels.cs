using UnityEngine;

public class SpawnJewels : MonoBehaviour
{

    //Serialized variables
    [Header("Corner Locations")]
    //[SerializeField] Transform upperLeftCorner;
    //[SerializeField] Transform lowerLeftCorner;
    [SerializeField] Transform upperRightCorner;
    //[SerializeField] Transform lowerRightCorner;
    
    [Header("Jewels")]
    [SerializeField] GameObject spawnedJewel;
    [SerializeField] GameObject clickedJewel;

    [Header("Spawn GameObject")]
    [SerializeField] Transform location;

    [Header("Level Choice")]
    [SerializeField] int level = 0;

    //Declared variables
    Vector2 jewelPos;
    ReadJson readJson;
    float jewelX;
    float jewelY;
    float cornerPositionX;
    float cornerPositionY;
    float padding = 5f;

    void Awake()
    {
        readJson = FindObjectOfType<ReadJson>();
        cornerPositionX =
              upperRightCorner.position.x <= 0
            ? upperRightCorner.position.x + padding
            : upperRightCorner.position.x - padding;
        cornerPositionY =
              upperRightCorner.position.y <= 0
            ? upperRightCorner.position.y + padding
            : upperRightCorner.position.y - padding;
    }
    
    void Start()
    {
        LevelData[] levelData = readJson.LevelData;

        for(int i = 0; i < levelData[level].level_data.Length; i += 2)
        {
            jewelX = levelData[level].level_data[i];
            jewelY = levelData[level].level_data[i + 1];

            if(inRange(jewelX, 500f, 1000f) && inRange(jewelY, 500f, 1000f))
            {
                jewelPos = new Vector2(
                Map(jewelX, 500f, 1000f, 0f, cornerPositionX),
                Map(jewelY, 500f, 1000f, 0f, cornerPositionY)
                                );
            }
            else if(inRange(jewelX, 0f, 500f) && inRange(jewelY, 500f, 1000f))
            {
                jewelPos = new Vector2(
                Map(jewelX, 0f, -500f, 0f, cornerPositionX),
                Map(jewelY, 500f, 1000f, 0f, cornerPositionY)
                                );
            }
            else if(inRange(jewelX, 0f, 500f) && inRange(jewelY, 0f, 500f))
            {
             jewelPos = new Vector2(
             Map(jewelX, 0f, -500f, 0f, cornerPositionX),
             Map(jewelY, 0f, -500f, 0f, cornerPositionY)
                                );
            }
            else if(inRange(jewelX, 500f, 1000f) && inRange(jewelY, 0f, 500f))
            {
                jewelPos = new Vector2(
                Map(jewelX, 500f, 1000f, 0f, cornerPositionX),
                Map(jewelY, 0f, -500f, 0f, cornerPositionY)
                                );
            }

            Debug.Log($"{jewelPos} -- ({jewelX}, {jewelY})");
            Debug.Log($"{i} -- {levelData[level].level_data.Length}");

            Instantiate(spawnedJewel, jewelPos, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

/*
JSON Koordinatės X: 50 Y: 50
Canvas X: 216.7 (Width: 433.4) Y: 100 (Height: 200)
Resolution X: 2340 Y: 1080

long map(long x, long in_min, long in_max, long out_min, long out_max) {
  return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}

Xc / Xr * Xj
*/