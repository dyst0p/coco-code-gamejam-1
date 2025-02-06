using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UiController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _poisonBar;
        [SerializeField] private Image _hpBar;
        [SerializeField] private Image _scoreBar;
        [SerializeField] private Image _bestScoreBar;
        [SerializeField] private float _poisoningScale = 2f;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private Button _playAgainButton;
        private float _barHeight;
        private float _poisonHeight;
        private float _heightOfHpUnit;
        
        private PlayerInput _playerInput;
        
        private string ScoreString => ((int)(PlayerData.Instance.Score * 10)).ToString();
        private string BestScoreString => ((int)(PlayerData.Instance.BestScore * 10)).ToString();

        private void Start()
        {
            _playerInput = PlayerData.Instance.GetComponent<PlayerInput>();
            PlayerData.Instance.HealthChanged += UpdateHp;
            PlayerData.Instance.PoisoningChanged += UpdatePoisoning;
            PlayerData.Instance.ScoreChanged += UpdateScore;
            PlayerData.Instance.GameOver += OnGameOver;
            _playerInput.actions["Navigate"].performed += ReturnFocus;
            
            _playAgainButton.onClick.AddListener(RestartGame);
            
            _barHeight = _poisonBar.gameObject.transform.parent.GetComponent<RectTransform>().rect.height;
            _heightOfHpUnit = _barHeight / PlayerData.MaxHealth;
            _poisonHeight = PlayerData.Instance.Poisoning * _heightOfHpUnit;
            
            UpdateBestScore();
            UpdateHp(PlayerData.Instance.Health);
        }

        private void OnDisable()
        {
            PlayerData.Instance.HealthChanged -= UpdateHp;
            PlayerData.Instance.PoisoningChanged -= UpdatePoisoning;
            PlayerData.Instance.ScoreChanged -= UpdateScore;
            PlayerData.Instance.GameOver += OnGameOver;
            if (_playerInput != null) 
                _playerInput.actions["Navigate"].performed -= ReturnFocus;
            _playAgainButton.onClick.RemoveListener(RestartGame);
        }

        private void UpdateHp(float hp)
        {
            UpdateBar(_poisonBar, hp * _heightOfHpUnit);
            UpdateBar(_hpBar, _poisonBar.rectTransform.sizeDelta.y - _poisonHeight);
        }

        private void UpdatePoisoning(float poisoning)
        {
            _poisonHeight = poisoning * _heightOfHpUnit * _poisoningScale;
            UpdateBar(_hpBar, _poisonBar.rectTransform.sizeDelta.y - _poisonHeight);
        }

        private void UpdateScore(float score)
        {
            UpdateBar(_scoreBar, Mathf.Clamp01(score / PlayerData.MaxScore) * _barHeight);
        }
        
        private void UpdateBestScore()
        {
            UpdateBar(_bestScoreBar, Mathf.Clamp01(PlayerData.Instance.BestScore / PlayerData.MaxScore) * _barHeight);
        }

        private void UpdateBar(Image bar, float height)
        {
            var delta = bar.rectTransform.sizeDelta;
            delta.y = height;
            bar.rectTransform.sizeDelta = delta;
        }
        
        private void OnGameOver()
        {
            _gameOverPanel.SetActive(true);
            _gameOverText.text = $"JESTER IS DEAD\n\n";
            if (PlayerData.Instance.Score > PlayerData.Instance.BestScore)
            {
                _gameOverText.text += $"NEW GREATEST OVATIONS: {ScoreString}";
            }
            else
            {
                _gameOverText.text += $"YOUR OVATION: {ScoreString}\nTHE GREATEST OVATION: {BestScoreString}";
            }
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PlayerData.IsGamePaused = false;
        }
        
        private void ReturnFocus(InputAction.CallbackContext callbackContext)
        {
            if (!EventSystem.current.currentSelectedGameObject)
            {
                EventSystem.current.SetSelectedGameObject(_playAgainButton.gameObject);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(_playAgainButton.gameObject);
        }
    }
}