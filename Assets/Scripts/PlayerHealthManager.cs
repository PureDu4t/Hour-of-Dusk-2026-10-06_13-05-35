using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField]private int _maxHP;
    [SerializeField]private AttackGridSpawner _attackGridSpawner;
    [SerializeField]private AudioManager _audioManager;
    [SerializeField]private TMP_Text _playerHealthBar;
    private SpriteRenderer _playerSprite;
    private int _currentHP;
    private Color _originalColor;

    private void UpdateHealthDisplay()
    {
        _playerHealthBar.text = "HP: " + _currentHP.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerSprite = GetComponent<SpriteRenderer>();
        _originalColor = _playerSprite.color;
        _currentHP = _maxHP;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayHitEffect()
    {
        _playerSprite.color = Color.red;
        _audioManager.PlayEnemyAttack();
        yield return new WaitForSeconds(0.1f);
        _playerSprite.color = _originalColor;
    }

    public void TakeDamage(int damage)
    {
        _currentHP = Mathf.Max(0, _currentHP - damage);
        UpdateHealthDisplay();
        Debug.Log("Player HP = " + _currentHP);
        StartCoroutine(PlayHitEffect());
    }

    public bool IsDefeated()
    {
        return _currentHP <= 0;
    }
}
