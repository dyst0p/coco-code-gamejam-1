using System;
using Services;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public int Score { get; private set; }
    public float Health { get; private set; }
    public float Poisoning { get; private set; }

    public event Action<float> ScoreChanged;
    public event Action<float> HealthChanged;
    public event Action<float> PoisoningChanged;

    public void AddScore(int score)
    {
        Score += score;
        ScoreChanged?.Invoke(Score);
    }

    public void AddDamage(float damage)
    {
        Health -= damage;
    }
}