using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<ScenePersist>().Length;

        if(numGameSessions > 1) { Destroy(gameObject); }
        else { DontDestroyOnLoad(gameObject); }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}