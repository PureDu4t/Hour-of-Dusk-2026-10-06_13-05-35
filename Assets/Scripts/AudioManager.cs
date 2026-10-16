using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioSource _playerSource;
    [SerializeField]private AudioSource _enemySource;
    [SerializeField]private AudioSource _UISource;
    [SerializeField]private AudioClip[] _pitchRecordings;
    [SerializeField]private AudioClip[] _weakPoints;
    [SerializeField] private AudioClip _attackSlash;
    [SerializeField] private AudioClip _criticalSlash;
    [SerializeField] private AudioClip _attackHit;
    [SerializeField] private AudioClip _criticalHit;
    [SerializeField] private AudioClip _attackMiss;
    [SerializeField] private AudioClip _uiStinger;
    [SerializeField] private AudioClip _enemyAttack;
    [SerializeField] private AudioClip[] _enemyHurt;
    [SerializeField] private AudioClip _enemyDeath;
    [SerializeField] private AudioClip[] _playerHurt;
    [SerializeField] private AudioClip _playerDeath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPitch(int _pitchNumber)
    {
        if (_pitchNumber <= 0 || _pitchNumber >= _pitchRecordings.Length)
        {
            return;
        }

        if (_pitchRecordings[_pitchNumber] == null)
        {
            return;
        }

        _enemySource.clip = _pitchRecordings[_pitchNumber];
        _enemySource.Play();
    }

        public void PlayWeakPitch(int _pitchNumber)
    {
        if (_pitchNumber <= 0 || _pitchNumber >= _pitchRecordings.Length)
        {
            return;
        }

        if (_pitchRecordings[_pitchNumber] == null)
        {
            return;
        }

        _enemySource.clip = _weakPoints[_pitchNumber];
        _enemySource.Play();
    }

    public void PlayAttackSlash()
    {
        _playerSource.clip = _attackSlash;
        _playerSource.Play();
    }

    public void PlayCriticalSlash()
    {
        _playerSource.clip = _criticalSlash;
        _playerSource.Play();
    }

    public void PlayAttackHit()
    {
        _playerSource.clip = _attackHit;
        _playerSource.Play();
    }

    public void PlayCriticalHit()
    {
        _playerSource.clip = _criticalHit;
        _playerSource.Play();
    }

    public void PlayAttackMiss()
    {
        _playerSource.clip = _attackMiss;
        _playerSource.Play();
    }

    public void PlayUIStinger()
    {
        _UISource.clip = _uiStinger;
        _UISource.Play();
    }

    public void PlayEnemyAttack()
    {
        _enemySource.clip = _enemyAttack;
        _enemySource.Play();
    }

    public void PlayEnemyHurt()
    {
        if (_enemyHurt == null || _enemyHurt.Length == 0)
        {
            return;
        }

        int i = UnityEngine.Random.Range(0, _enemyHurt.Length);

        if (_enemyHurt[i] == null)
        {
            return;
        }

        _enemySource.clip = _enemyHurt[i];
        _enemySource.Play();
    }

    public void PlayEnemyDeath()
    {
        _enemySource.clip = _enemyDeath;
        _enemySource.Play();
    }

    public void PlayPlayerHurt()
    {
        if (_playerHurt == null || _playerHurt.Length == 0)
        {
            return;
        }

        int i = UnityEngine.Random.Range(0, _playerHurt.Length);

        if (_playerHurt[i] == null)
        {
            return;
        }
        _playerSource.clip = _playerHurt[i];
        _playerSource.Play();
    }

    public void PlayPlayerDeath()
    {
        _playerSource.clip = _playerDeath;
        _playerSource.Play();
    }
}