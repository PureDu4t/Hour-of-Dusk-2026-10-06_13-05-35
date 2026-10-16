using UnityEngine;
using System.Collections;
using TMPro;

public class CombatTurnController : MonoBehaviour
{
    public enum CombatState
    {
        PlayerChoosing, PlayerAttacking, EnemyActing, CombatEnded
    }

    [SerializeField] AudioManager _audioManager;
    [SerializeField]private float _windupDuration;
    [SerializeField]private float _recoveryDuration;
    [SerializeField]private int _enemyDamage;
    [SerializeField]private int _enemyHitCount;
    [SerializeField]private PlayerHealthManager _playerHealthManager;
    [SerializeField]private TMP_Text _intentText;
    [SerializeField]private TMP_Text _currentTurn;
    private CombatState _currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void UpdateIntentDisplay()
    {
        if (_enemyHitCount == 1)
        {
            _intentText.text = "Enemy Intent = " + _enemyDamage.ToString();
        }
        else
        {
           _intentText.text = "Enemy Intent = " + _enemyDamage.ToString() + "x" + _enemyHitCount.ToString();
        }
    }
    void Awake()
    {
        _currentState = CombatState.PlayerChoosing;
    }
    void Start()
    {
        UpdateIntentDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(_windupDuration);
        _playerHealthManager.TakeDamage(_enemyDamage);
        yield return PlayPlayerHurt();
        yield return new WaitForSeconds(_recoveryDuration);

        if (_playerHealthManager.IsDefeated())
        {
            SetCurrentState(CombatState.CombatEnded);
            _audioManager.PlayPlayerDeath();
        }
        else
        {
            SetCurrentState(CombatState.PlayerChoosing);
        }
    }

        private IEnumerator PlayPlayerHurt()
    {
        yield return new WaitForSeconds(0.2f);
        _audioManager.PlayPlayerHurt();
    }

    public CombatState GetCurrentState()
    {
        return _currentState;
    }

    public void SetCurrentState(CombatState newState)
    {
        _currentState = newState;
        _currentTurn.text = "Current state: " + _currentState;
    }

    public void BeginEnemyTurn()
    {
        if (_currentState == CombatState.PlayerAttacking)
        {
            SetCurrentState(CombatState.EnemyActing);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            return;
        }
    }
}
