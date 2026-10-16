using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class AttackController : MonoBehaviour
{
    public int[] _weakPointIndex = { 1 };
    [SerializeField] private int _ATK = 6;
    [SerializeField] private int _attackModifier = 0;
    [SerializeField] private float _criticalModifier = 2f;
    [SerializeField] private int _enemyMaxHP = 18;
    [SerializeField] AudioManager _audioManager;
    [SerializeField] CombatTurnController _turnController;
    [SerializeField]private TMP_Text _enemyHealthBar;
    private int _enemyHP;
    private bool _deathHandled = false;

    private void UpdateHealthDisplay()
    {
        _enemyHealthBar.text = "HP: " + _enemyHP.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<AttackGridSpawner>().CurrentPitch();
        _enemyHP = _enemyMaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyHP <= 0 && !_deathHandled)
                {
                    Debug.Log("Enemy defeated!");
                    _deathHandled = true;
                    _audioManager.PlayEnemyDeath();
                    GetComponent<AttackGridSpawner>().DestroyAllBlocks();
                }

        if (Input.GetKeyDown(KeyCode.Alpha1)&&_turnController.GetCurrentState()==CombatTurnController.CombatState.PlayerChoosing)
        {
            if (_deathHandled)
            {
            return;
            }

            Debug.Log("Attacking");
            _turnController.SetCurrentState(CombatTurnController.CombatState.PlayerAttacking);
            GetComponent<AttackGridSpawner>().StartAttack();
        }

        if (Input.GetKeyDown(KeyCode.Space)&&_turnController.GetCurrentState()==CombatTurnController.CombatState.PlayerAttacking)
        {
            if (_deathHandled)
            {
            return;
            }

            AttackGridSpawner spawner = GetComponent<AttackGridSpawner>();
            int currentIndex = spawner.ExposeCurrentNoteIndex();
            bool isWeakPoint = false;

            foreach (int i in _weakPointIndex)
            {
                if (currentIndex == i)
                {
                    isWeakPoint = true;
                    break;
                }
            }

            bool hitRegistered = spawner.RegisterHit(isWeakPoint);

            if (hitRegistered)
            {
                int damage = _ATK + _attackModifier;

                if (isWeakPoint)
                {
                    damage = Mathf.RoundToInt(damage * _criticalModifier);
                    
                    Debug.Log("Critical Hit! Damage: " + damage);
                    _audioManager.PlayCriticalSlash();
                    StartCoroutine(PlayCriticalHit());
                }
                else
                {
                    Debug.Log("Normal Hit. Damage: " + damage);
                    _audioManager.PlayAttackSlash();
                    StartCoroutine(PlayAttackHit());
                }
                _enemyHP = Mathf.Max(0, _enemyHP - damage);
                UpdateHealthDisplay();
                Debug.Log("Enemy HP: " + _enemyHP + "/" + _enemyMaxHP);
                StartCoroutine(PlayEnemyHurt());
            }
        }
    }

    public void OnAttackFinished()
    {
        Debug.Log("Attack finished");
        if (_turnController.GetCurrentState() != CombatTurnController.CombatState.PlayerAttacking)
        {
            return;
        }
        if (_enemyHP <= 0)
        {
            _turnController.SetCurrentState(CombatTurnController.CombatState.CombatEnded);
        }
        else if (_enemyHP > 0)
        {
            _turnController.BeginEnemyTurn();
        }

    }

    private IEnumerator PlayEnemyHurt()
    {
        if (_enemyHP > 0)
        {
            yield return new WaitForSeconds(0.1f);
            _audioManager.PlayEnemyHurt();
        }
    }

    private IEnumerator PlayAttackHit()
    {
        if (_enemyHP > 0)
        {
            yield return new WaitForSeconds(0.02f);
            _audioManager.PlayAttackHit();
        }
    }

    private IEnumerator PlayCriticalHit()
    {
        if (_enemyHP > 0)
        {
            yield return new WaitForSeconds(0.02f);
            _audioManager.PlayCriticalHit();
        }
    }
}

