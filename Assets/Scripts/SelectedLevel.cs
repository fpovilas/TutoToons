using UnityEngine;

public class SelectedLevel : MonoBehaviour
{
    private int sceneToLoad;
    public bool isSelected = false;

    public int SceneToLoad
    {
        get { return sceneToLoad; }
        set { sceneToLoad = value; }
    }

    public void OnAnswerSelected(int index)
    {
        if(isSelected == false)
        {
            sceneToLoad = index;
            isSelected = true;
        }
    }
}
