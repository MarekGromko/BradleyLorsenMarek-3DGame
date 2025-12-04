using UnityEngine;
using UnityEngine.Events;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance { get; private set; }

    [Header("Time config")]
    [SerializeField] private float _survivalTime = 300f;
    
    [Header("State")]
    [SerializeField] private float _elapsedTime = 0f;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _isPaused = false;

    [Header("Events")]
    public UnityEvent OnTimeStarted;
    public UnityEvent OnTimePaused;
    public UnityEvent OnTimeResumed;
    public UnityEvent<float> OnTimeUpdated; // Envoie le temps restant
    public UnityEvent OnSurvivalComplete;
    public UnityEvent OnTimeReset;

    // Properties
    public float ElapsedTime => _elapsedTime;
    public float RemainingTime => Mathf.Max(0, _survivalTime - _elapsedTime);
    public float SurvivalTime => _survivalTime;
    public float CompletionPercentage => Mathf.Clamp01(_elapsedTime / _survivalTime);
    public bool IsRunning => _isRunning && !_isPaused;
    public bool IsPaused => _isPaused;
    public bool IsComplete => _elapsedTime >= _survivalTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_isRunning && !_isPaused && !IsComplete)
        {
            _elapsedTime += Time.deltaTime;
            OnTimeUpdated?.Invoke(RemainingTime);
            if (IsComplete)
            {
                CompleteSurvival();
            }
        }
    }

    public void StartTime()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _isPaused = false;
            OnTimeStarted?.Invoke();
            Debug.Log($"TimeSystem: Started. Survival time: {_survivalTime} seconds");
        }
    }
    public void PauseTime()
    {
        if (_isRunning && !_isPaused)
        {
            _isPaused = true;
            OnTimePaused?.Invoke();
            Debug.Log("TimeSystem: Paused");
        }
    }

    public void ResumeTime()
    {
        if (_isRunning && _isPaused)
        {
            _isPaused = false;
            OnTimeResumed?.Invoke();
            Debug.Log("TimeSystem: Resumed");
        }
    }
    public void ResetTime()
    {
        _elapsedTime = 0f;
        _isRunning = false;
        _isPaused = false;
        OnTimeReset?.Invoke();
        Debug.Log("TimeSystem: Reset");
    }
    private void CompleteSurvival()
    {
        _isRunning = false;
        OnSurvivalComplete?.Invoke();
        Debug.Log("TimeSystem: Survival complete!");
    }
    public string GetFormattedTime()
    {
        
        int minutes = Mathf.FloorToInt(RemainingTime / 60f);
        int seconds = Mathf.FloorToInt(RemainingTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}