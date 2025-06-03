using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // ----------------------------------------------------------------
    // 1) Static singleton reference so other scripts can call ScoreManager.Instance.AddPoint()
    // ----------------------------------------------------------------
    public static ScoreManager Instance { get; private set; }

    [Tooltip("Drag the ScoreText (UI Text) here.")]
    public Text scoreText;

    private int _score = 0;

    void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[ScoreManager] Another instance already exists! Destroying this one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Optional: DontDestroyOnLoad(gameObject); // if you want it to persist across scenes
    }

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("[ScoreManager] No scoreText assigned in Inspector!");
            return;
        }
        UpdateText();
    }

    /// <summary>
    /// Call ScoreManager.Instance.AddPoint(...) to increase the score.
    /// </summary>
    public void AddPoint(int amount = 1)
    {
        _score += amount;
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = "Score: " + _score;
    }
}
