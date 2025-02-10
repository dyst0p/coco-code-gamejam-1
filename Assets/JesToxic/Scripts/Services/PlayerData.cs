using System;
using JesToxic.FX;
using JesToxic.Tools;
using UnityEngine;

namespace JesToxic.Services
{
    public class PlayerData : Singleton<PlayerData>
    {
        public const float MaxHealth = 100f;
        public const float MaxScore = 120f;
        private const int SaveVersion = 2;
        public static bool IsFirstStart = true;
        private static float _bestScore;
        private float _health = MaxHealth;
        private float _poisoning;
        private bool _isGameOver;
        private float _ovationThreshold = 4;
        private float _bigOvationThreshold = 8;

        public float Health
        {
            get => _health;
            private set => _health = Mathf.Clamp(value, 0f, MaxHealth);
        }

        public float Poisoning
        {
            get => _poisoning;
            private set => _poisoning = value < 0 ? 0 : value;
        }

        [field: SerializeField] public float Score { get; private set; }
        public float BestScore => _bestScore;
        public string ScoreString => ((int)(Score * 10)).ToString();
        public string BestScoreString => ((int)(BestScore * 10)).ToString();
        public bool IsLucky { get; private set; }

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
                    print("Best score loaded from saves: " + _bestScore);
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
                PlayerPrefs.SetFloat("BestScore", _bestScore);
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
            if (score > _ovationThreshold)
            {
                var ovationType = score > _bigOvationThreshold 
                    ? SoundFxType.OvationBig 
                    : SoundFxType.Ovation;
                this.CreateSoundFx(ovationType);
            }
        
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

        public void GetLucky()
        {
            IsLucky = true;
        }
    }
}