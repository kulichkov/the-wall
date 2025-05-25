using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public bool IsGameOver { get; private set; }
    public int CurrentScore { get; private set; }
    [SerializeField] private PlayerController Player;
    [SerializeField] private EnemySpawnManager EnemySpawnManager;
    
    public TextMeshProUGUI ScoreText;
    public MenuPanel MenuPanel;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetInitialState();
    }

    private void UpdateScoreText()
    {
        ScoreText.text = $"Score: {CurrentScore}";
    }

    private void SetInitialState()
    {
        IsGameOver = true;
        ScoreText.text = "";

        MenuPanel.TitleText.text = "THE WALL";
        MenuPanel.ButtonText.text = "START GAME";
        MenuPanel.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        MenuPanel.gameObject.SetActive(false);
        CurrentScore = 0;
        UpdateScoreText();
        Player.Reset();
        EnemySpawnManager.Clear();
        IsGameOver = false;
        Debug.Log($"Game started. Score: {CurrentScore}");
    }

    public void EndGame()
    {
        Player.StopMoving();
        IsGameOver = true;
        MenuPanel.TitleText.text = "GAME OVER";
        MenuPanel.ButtonText.text = "RESTART";
        MenuPanel.gameObject.SetActive(true);
        Debug.Log($"Game over. Score: {CurrentScore}");
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        UpdateScoreText();
        Debug.Log($"Score updated by {score}: {CurrentScore}");
    }
}
