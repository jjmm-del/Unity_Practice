using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InitializeGame()
    {
        Score = 0;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        
        OnScoreChanged?.Invoke(Score);
    }
    
}
