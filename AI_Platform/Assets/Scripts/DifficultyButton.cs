using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    public int difficultyLevel; // Assign the difficulty level for each button in the Inspector

    public void OnClick()
    {
        GameManager.instance.SetDifficultyLevel(difficultyLevel);
    }
}