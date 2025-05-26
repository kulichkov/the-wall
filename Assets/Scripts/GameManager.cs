using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public bool IsGameOver { get; private set; }
    public int CurrentScore { get; private set; }
    [SerializeField] private PlayerController Player;
    [SerializeField] private EnemySpawnManager EnemySpawnManager;
    
    public TextMeshProUGUI ScoreText;
    public MenuPanel MenuPanel;

    void Awake()
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
        IsGameOver = false;
        CurrentScore = 0;
        UpdateScoreText();
        MenuPanel.gameObject.SetActive(false);
        Player.Reset();
        EnemySpawnManager.Clear();
        EnemySpawnManager.StartSpawning();
    }

    public void EndGame()
    {
        Player.StopMoving();
        EnemySpawnManager.StopSpawning();
        IsGameOver = true;
        MenuPanel.TitleText.text = "GAME OVER";
        MenuPanel.ButtonText.text = "RESTART";
        MenuPanel.gameObject.SetActive(true);
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        UpdateScoreText();
    }
}
