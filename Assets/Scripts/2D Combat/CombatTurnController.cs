using UnityEngine;
using System.Collections;
using TMPro;

public class CombatTurnController : MonoBehaviour
{
    public enum CombatState
    {
        PlayerChoosing, PlayerAttacking, PlayerDefending, PlayerPoweringUp, EnemyActing, CombatEnded
    }

    [SerializeField] AudioManager _audioManager;
    [SerializeField]private float _windupDuration;
    [SerializeField]private float _recoveryDuration;
    [SerializeField]private int _enemyDamage;
    [SerializeField]private int _enemyHitCount;
    [SerializeField]private PlayerHealthManager _playerHealthManager;
    [SerializeField]private TMP_Text _intentText;
    [SerializeField]private TMP_Text _currentTurn;
    [SerializeField]private DefenseController _defenseController;
    [SerializeField]private AttackController _attackController;
    [SerializeField]private int _enemyBuffAmount = 1;
    [SerializeField]private int _enemyAttackBonus = 0;
    private CombatState _currentState;
    private bool _defendChosen;
    private bool _enemyWillBuff = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public int EnemyAttackBonus => _enemyAttackBonus;

    private void UpdateIntentDisplay()
    {
        if (_enemyWillBuff)
        {
            _intentText.text = "Enemy Intent = Buff " + _enemyBuffAmount;
            _audioManager.PlayEnemyIntentBuff();
        }
        else if (_enemyHitCount == 1)
        {
            _intentText.text = "Enemy Intent = " + _enemyDamage.ToString();
            _audioManager.PlayEnemyIntentAttack();
        }
        else if (_enemyHitCount > 1)
        {
           _intentText.text = "Enemy Intent = " + _enemyDamage.ToString() + "x" + _enemyHitCount.ToString();
           _audioManager.PlayEnemyIntentAttack();
        }
    }
    void Awake()
    {
        _currentState = CombatState.PlayerChoosing;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(_windupDuration);
        if (_enemyWillBuff)
        {
            {
                _audioManager.PlayEnemyBuff();
                _enemyAttackBonus += _enemyBuffAmount;
                _attackController.UpdateHealthDisplay();
            }
        }
        else if (_defendChosen)
        {
            _defenseController.BeginDefense();
            yield return new WaitUntil(() => !_defenseController.IsAwaitingBlock());
            if (_defenseController.WasBlockSuccessful())
            {
                _audioManager.PlayBlockSuccess();
            }

            if(!_defenseController.WasBlockSuccessful())
            {
                _playerHealthManager.TakeDamage(_enemyDamage + _enemyAttackBonus);
                yield return PlayPlayerHurt();
            }
        }
        else
        {
            _playerHealthManager.TakeDamage(_enemyDamage + _enemyAttackBonus);
            yield return PlayPlayerHurt();
        }

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

        _enemyWillBuff = !_enemyWillBuff;
        UpdateIntentDisplay();
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
        
        if (newState == CombatState.CombatEnded)
        {
            GameManager.Instance.FinishRun();
        }
    }

    public void BeginEnemyTurn()
    {
        if (_currentState == CombatState.PlayerAttacking ||
        _currentState == CombatState.PlayerDefending ||
        _currentState == CombatState.PlayerPoweringUp)
        {
            if (_currentState == CombatState.PlayerDefending)
            {
                _defendChosen = true;
            }
            _defendChosen = _currentState == CombatState.PlayerDefending;
            SetCurrentState(CombatState.EnemyActing);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            return;
        }
    }
}
