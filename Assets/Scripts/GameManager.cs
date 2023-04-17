using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    JewelHandler jewelHandler;
    SelectedLevel selectedLevel;

    public int Find { get; private set; }

    private void Awake()
    {
        jewelHandler = FindObjectOfType<JewelHandler>();
        selectedLevel = FindObjectOfType<SelectedLevel>();
        int numGameSessions = FindObjectsOfType<GameManager>().Length;

        if(numGameSessions > 1)
        { 
            Destroy(gameObject);
            selectedLevel.gameObject.SetActive(false);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            jewelHandler.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        jewelHandler.gameObject.SetActive(false);
    }

    void Update()
    {
        if(selectedLevel.isSelected)
        {
            SceneManager.LoadScene(selectedLevel.SceneToLoad);
            selectedLevel.isSelected = false;
        }
    }

    public void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        Destroy(gameObject);
    }
}
