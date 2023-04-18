using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] JewelHandler jewelHandlerPrefab;

    JewelHandler jewelHandler;
    SelectedLevel selectedLevel;

    private void Awake()
    {
        jewelHandler = FindObjectOfType<JewelHandler>();
        selectedLevel = FindObjectOfType<SelectedLevel>();
    }

    private void Start()
    {
        jewelHandler.gameObject.SetActive(false);
    }

    void Update()
    {
        if(selectedLevel.isSelected)
        {
            jewelHandler.gameObject.SetActive(true);
            selectedLevel.gameObject.SetActive(false);
            selectedLevel.isSelected = false;
        }
    }

    public void ResetSession()
    {
        selectedLevel.gameObject.SetActive(true);
        foreach(var jewel in jewelHandler.jewelObjects)
        {
            Destroy(jewel);
        }
        foreach(var line in jewelHandler.lineHandler)
        {
            Destroy(line);
        }
        Destroy(jewelHandler.gameObject);
        jewelHandler = Instantiate(jewelHandlerPrefab, Vector3.zero, Quaternion.identity);
        jewelHandler.gameObject.SetActive(false);
    }
}
