using System;
using Services;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    private const float MaxHealth = 100f;
    private const int SaveVersion = 1;
    [SerializeField] private float _health = MaxHealth;
    [SerializeField] private float _poisoning;
    private static float _bestScore;
    private bool _isGameOver;

    public float Health
    {
        get => _health;
        private set => _health = Mathf.Clamp(value, 0f, MaxHealth);
    }

    public float Poisoning
    {
        get => _poisoning;
        set => _poisoning = value < 0 ? 0 : value;
    }
    [field: SerializeField] public float Score { get; private set; }
    public float BestScore => _bestScore;

    public event Action<float> HealthChanged;
    public event Action<float> PoisoningChanged;
    public event Action<float> ScoreChanged;
    public event Action GameOver;

    protected override void Awake()
    {
        base.Awake();
        void ClearSaves()
        {
            PlayerPrefs.SetInt("SaveVersion", SaveVersion);
            PlayerPrefs.SetFloat("BestScore", 0);
        }
        
        if (PlayerPrefs.HasKey("SaveVersion"))
        {
            int saveVersion = PlayerPrefs.GetInt("SaveVersion");
            if (saveVersion != SaveVersion)
            {
                ClearSaves();
            }
            else
            {
                _bestScore = PlayerPrefs.GetFloat("BestScore");
            }
        }
        else
        {
            ClearSaves();
        }
        
    }

    private void OnDestroy()
    {
        if (_bestScore < Score)
        {
            _bestScore = Score;
            PlayerPrefs.SetFloat("BestScore", 0);
        }
    }

    private void FixedUpdate()
    {
        if (Poisoning != 0 && Health > 0)
        {
            ChangeHealth(-Poisoning * Time.fixedDeltaTime);
        }

        if (Health <= 0 && !_isGameOver)
        {
            _isGameOver = true;
            Time.timeScale = 0;
            GameOver?.Invoke();
        }
    }
    
    public void AddScore(float score)
    {
        int oldScoreInt = (int) (Score * 10);
        Score += score;
        int newScoreInt = (int) (Score * 10);
        if (newScoreInt != oldScoreInt)
        {
            ScoreChanged?.Invoke(Score);
        }
    }

    public void ChangeHealth(float change)
    {
        int oldHealthInt = (int)Health;
        Health += change;
        int newHealthInt = (int)Health;
        if (newHealthInt != oldHealthInt)
        {
            HealthChanged?.Invoke(Health);
        }
    }
    
    public void ChangePoisoning(float change)
    {
        int oldPoisoningInt = (int)Poisoning;
        Poisoning += change;
        int newPoisoningInt = (int)Poisoning;
        if (newPoisoningInt != oldPoisoningInt)
        {
            PoisoningChanged?.Invoke(Poisoning);
        }
    }
}