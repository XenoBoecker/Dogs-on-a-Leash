using UnityEngine;

public class ScoreShowObject : MonoBehaviour
{
    ObjectiveCollector objectiveCollector;


    [SerializeField] TMPro.TextMeshProUGUI scoreText;

    private void Start()
    {
        objectiveCollector.OnScoreChanged += UpdateScoreUI;
    }

    private void UpdateScoreUI()
    {
        scoreText.text = objectiveCollector.ScoreCount.ToString();
    }
}