using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public bool IsGameOver { get; private set; }
    public int CurrentScore { get; private set; }

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartGame();
    }

    public void StartGame()
    {
        IsGameOver = false;
        CurrentScore = 0;
        Debug.Log($"Game started. Score: {CurrentScore}");
    }

    public void EndGame()
    {
        IsGameOver = true;
        Debug.Log($"Game over. Score: {CurrentScore}");
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        Debug.Log($"Score updated by {score}: {CurrentScore}");
    }
}
