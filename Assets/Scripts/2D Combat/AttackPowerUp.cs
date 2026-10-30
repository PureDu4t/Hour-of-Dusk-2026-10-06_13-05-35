using UnityEngine;
using System.Collections;

public class AttackPowerUp : MonoBehaviour
{
    [SerializeField] private int _attackBonus = 3;
    [SerializeField] private float _duration = 15f;
    [SerializeField] private AttackController _attackController;
    [SerializeField] private CombatTurnController _turnController;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private PlayerHealthManager _playerHealthManager;
    private bool _isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)&&_turnController.GetCurrentState()==CombatTurnController.CombatState.PlayerChoosing)
        {
            _audioManager.PlayUIStinger();
            ActivatePowerUp();
            _playerHealthManager.UpdateHealthDisplay();
        }
    }

    public void ActivatePowerUp()
    {
            StartCoroutine(PowerUpRoutine());
    }

    private IEnumerator PowerUpRoutine()
    {
        _audioManager.PlayPlayerBuff();
        _attackController.PowerUpBonus += _attackBonus;
        _turnController.SetCurrentState(CombatTurnController.CombatState.PlayerPoweringUp);
        _turnController.BeginEnemyTurn();
        yield return new WaitForSeconds(_duration);
        _attackController.PowerUpBonus -= _attackBonus;
    }
}
