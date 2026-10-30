using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _playerSource;
    [SerializeField] private AudioSource _enemySource;
    [SerializeField] private AudioSource _UISource;
    [SerializeField] private AudioSource _ttsSource;
    [SerializeField] private AudioSource _defendSource;
    [SerializeField] private AudioClip[] _pitchRecordings;
    [SerializeField] private AudioClip[] _weakPoints;
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
    [SerializeField] private AudioClip _defendDrumLoop;
    [SerializeField] private AudioClip _blockPercussion;
    [SerializeField] private AudioClip _blockSuccess;
    [SerializeField] private MidiSample[] _midiSamples;
    [SerializeField] private AudioClip _playerBuff;
    [SerializeField] private AudioClip _enemyBuff;
    [SerializeField] private AudioClip _enemyIntentAttack;
    [SerializeField] private AudioClip _enemyIntentBuff;
    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable] private class MidiSample
    {
        public int MidiNote;
        public AudioClip Clip;
    }

    public AudioClip GetMidiClip(int MidiNote)
    {
        foreach (MidiSample sample in _midiSamples)
        {
            if (sample.MidiNote == MidiNote)
            {
                return sample.Clip;
            }
        }
        return null;
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

    //Defend

    public void PlayDefendDrums(double scheduledTime)
    {
        _defendSource.clip = _defendDrumLoop;
        _defendSource.PlayScheduled(scheduledTime);
    }

    public void PlayBlockPercussion()
    {
        _defendSource.clip = _blockPercussion;
        _defendSource.Play();
    }

    public void PlayBlockSuccess()
    {
        _UISource.clip = _blockSuccess;
        _UISource.Play();
    }

    public void PlayPlayerBuff()
    {
        _playerSource.clip = _playerBuff;
        _playerSource.Play();
    }

    public void PlayEnemyBuff()
    {
        _enemySource.clip = _enemyBuff;
        _enemySource.Play();
    }

    public void PlayEnemyIntentAttack()
    {
        _ttsSource.clip = _enemyIntentAttack;
        _ttsSource.Play();
    }
    public void PlayEnemyIntentBuff()
    {
        _ttsSource.clip = _enemyIntentBuff;
        _ttsSource.Play();
    }
}