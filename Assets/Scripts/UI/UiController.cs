using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statsText;
        [SerializeField] private TMP_Text _bestScoreText;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private Button _playAgainButton;
        
        private string HealthString => ((int)PlayerData.Instance.Health).ToString();
        private string PoisoningString => ((int)PlayerData.Instance.Poisoning).ToString();
        private string ScoreString => ((int)(PlayerData.Instance.Score * 10)).ToString();
        private string BestScoreString => ((int)(PlayerData.Instance.BestScore * 10)).ToString();

        private void Start()
        {
            PlayerData.Instance.HealthChanged += UpdateStats;
            PlayerData.Instance.PoisoningChanged += UpdateStats;
            PlayerData.Instance.ScoreChanged += UpdateStats;
            PlayerData.Instance.GameOver += OnGameOver;
            _playAgainButton.onClick.AddListener(RestartGame);

            if (PlayerData.Instance.BestScore > 0)
            {
                _bestScoreText.text = $"<color=\"yellow\">BEST SCORE\n{BestScoreString}";
            }
        }

        private void OnDisable()
        {
            PlayerData.Instance.HealthChanged -= UpdateStats;
            PlayerData.Instance.PoisoningChanged -= UpdateStats;
            PlayerData.Instance.ScoreChanged -= UpdateStats;
            PlayerData.Instance.GameOver += OnGameOver;
            _playAgainButton.onClick.RemoveListener(RestartGame);
        }

        void UpdateStats(float f) => UpdateStats();
        private void UpdateStats()
        {
            _statsText.text =
                $"<color=\"red\">HP: {HealthString}</color>\n<color=\"green\">POIS: {PoisoningString}</color>\n<color=\"yellow\">SCR: {ScoreString}</color>";
        }

        private void OnGameOver()
        {
            _gameOverPanel.SetActive(true);
            _gameOverText.text = $"GG\n";
            if (PlayerData.Instance.Score > PlayerData.Instance.BestScore)
            {
                _gameOverText.text += $"NEW BEST SCORE: {ScoreString}";
            }
            else
            {
                _gameOverText.text += $"YOUR SCORE: {ScoreString}\nBEST SCORE: {BestScoreString}";
            }
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
    }
}