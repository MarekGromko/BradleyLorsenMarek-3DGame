using UnityEngine;
using UnityEngine.Events;

public class HealthState : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    public float MaxHealth { get {return _maxHealth; } }
    public float CurrentHealth { 
        get { return _currentHealth; }
        set
        {
            if(!Mathf.Approximately(_currentHealth, value) && value >= 0) {
                if(value <= 0) {
                    OnDeath?.Invoke();
                }
                float previousHealth = _currentHealth;
                _currentHealth = value;
                OnHealthChange.Invoke(_currentHealth, previousHealth);
            }
        }
    }
    
    [Header("Events")]
    public UnityEvent<float, float> OnHealthChange;
    public UnityEvent OnDeath;
}
