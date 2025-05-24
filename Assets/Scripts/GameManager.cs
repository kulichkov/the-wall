using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public bool IsGameOver { get; private set; }
    public int CurrentScore { get; private set; }
    private PlayerController playerController;
    public TextMeshProUGUI ScoreText;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        StartGame();
    }

    private void UpdateScoreText()
    {
        ScoreText.text = $"Score: {CurrentScore}";
    }

    public void StartGame()
    {
        IsGameOver = false;
        CurrentScore = 0;
        UpdateScoreText();
        Debug.Log($"Game started. Score: {CurrentScore}");
    }

    public void EndGame()
    {
        playerController.StopMoving();
        IsGameOver = true;
        Debug.Log($"Game over. Score: {CurrentScore}");
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        UpdateScoreText();
        Debug.Log($"Score updated by {score}: {CurrentScore}");
    }
}
