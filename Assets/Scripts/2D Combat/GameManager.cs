using UnityEngine;
using TMPro;
using NUnit.Framework;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI TimerGUI;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private PlayerHealthManager _playerHealthManager;
    [SerializeField] private float _hpWeight = 50.0f;

    private float _elapsedSeconds;
    private bool _runActive;
    private float _finalScore;
    private float _timer;

    public float Timer
    {
        get => _timer;

        set
        {
            _timer = value;

            int minutes = Mathf.FloorToInt(Timer / 60.0f);
            int seconds = Mathf.FloorToInt(Timer % 60.0f);

            TimerGUI.text = $"{minutes:00}:{seconds:00}";
        }
    }

        public void FinishRun()
    {
        _runActive = false;
        _finalScore = _playerHealthManager.CurrentHP * _hpWeight - Timer;
        _scoreText.text = "Score: " + _finalScore.ToString("0");
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Timer = 0f;
        _runActive = true;
    }

    void Update()
    {
        if (_runActive)
        {
            Timer += Time.deltaTime;
        }
    }
}