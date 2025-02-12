using JesToxic.Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JesToxic.UI
{
    public class UiController : MonoBehaviour, IPointerClickHandler
    {
        [Header("Bars")]
        [SerializeField] private Image _poisonBar;
        [SerializeField] private Image _hpBar;
        [SerializeField] private Image _scoreBar;
        [SerializeField] private Image _bestScoreBar;
        [SerializeField] private float _poisoningScale = 2f;
        [Header("Panels")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _startPanel;
        [Space]
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private GameObject[] _onlyForStandaloneButtons;
        
        private float _barHeight;
        private float _poisonHeight;
        private float _heightOfHpUnit;
        private PlayerInput _playerInput;

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            foreach (var button in _onlyForStandaloneButtons)
            {
                button.SetActive(false);
            }
#endif
            Cursor.visible = false;
            if (PlayerData.IsFirstStart)
            {
                Time.timeScale = 0;
            }
            else
            {
                StartGame();
            }
            
        }

        private void Start()
        {
            _playerInput = PlayerData.Instance.GetComponent<PlayerInput>();
            PlayerData.Instance.HealthChanged += UpdateHp;
            PlayerData.Instance.PoisoningChanged += UpdatePoisoning;
            PlayerData.Instance.ScoreChanged += UpdateScore;
            PlayerData.Instance.GameOver += OnGameOver;
            _playerInput.actions["Navigate"].performed += ReturnFocus;
            
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
        }

        public void StartGame()
        {
            Time.timeScale = 1;
            PlayerData.IsFirstStart = false;
            _startPanel.SetActive(false);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Exit()
        {
            if (Application.isEditor)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
            else
            {
                Application.Quit();
            }
        }

        public void ShowControls()
        {
            PlayerData.IsFirstStart = true;
            RestartGame();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var button = GetComponentInChildren<Button>(false);
            if (button == null)
                return;
            EventSystem.current.SetSelectedGameObject(button.gameObject);
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
            OnPointerClick(null);
            
            _gameOverText.text = $"JESTER IS DEAD\n...\n";
            if (PlayerData.Instance.Score > PlayerData.Instance.BestScore)
            {
                _gameOverText.text += $"NEW GREATEST OVATIONS: {PlayerData.Instance.ScoreString}";
            }
            else
            {
                _gameOverText.text +=
                    $@"YOUR OVATION: {PlayerData.Instance.ScoreString}\nTHE GREATEST OVATION: {PlayerData.Instance.BestScoreString}";
            }
        }

        private void ReturnFocus(InputAction.CallbackContext callbackContext)
        {
            if (!EventSystem.current.currentSelectedGameObject)
            {
                OnPointerClick(null);
            }
        }
    }
}