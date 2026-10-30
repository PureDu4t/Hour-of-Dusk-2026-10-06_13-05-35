using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    [SerializeField] private int[] _midiPitches;//Pitch numbers
    [SerializeField] private float[] _beatPositions;//Each note’s position in beats
    [SerializeField] private float _bpm;//Tempo
    [SerializeField] private int _blockNoteIndex;//Which note requires Space
    [SerializeField] private float _hitWindow;//Timing tolerance in seconds
    [SerializeField] private AudioSource[] _noteSources;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private EnemyAttackTile _enemyAttackTile;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private float _travelDuration;
    private double _sequenceStartTime;
    private double _secondsPerBeat;
    private double _targetHitTime;
    private bool _awaitingBlock;
    private bool _blockSucceeded;
    private EnemyAttackTile _activeTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_awaitingBlock && Input.GetKeyDown(KeyCode.Space))
        {
            double timingError = AudioSettings.dspTime - _targetHitTime;
            _audioManager.PlayBlockPercussion();
            if (System.Math.Abs(timingError) <= _hitWindow)
            {
                _awaitingBlock = false;
                Debug.Log("Block");
                ResolveBlock();
            }
            else
            {
                _awaitingBlock = false;
                Debug.Log("Miss");
                ResolveMiss();
            }
        }
        if (_awaitingBlock && AudioSettings.dspTime > _targetHitTime + _hitWindow)
        {
            _awaitingBlock = false;
            Debug.Log("Miss");
            ResolveMiss();
        }
    }

    public void BeginDefense()
    {
        _blockSucceeded = false;
        _awaitingBlock = true;
        _secondsPerBeat = 60.0 / _bpm;
        _sequenceStartTime = AudioSettings.dspTime + _travelDuration;
        _audioManager.PlayDefendDrums(_sequenceStartTime);
        _activeTile = Instantiate(_enemyAttackTile, _spawnPoint.position, Quaternion.identity);
        
        for (int n = 0; n < _beatPositions.Length; n++)
        {
            double scheduledTime = _sequenceStartTime + _beatPositions[n] * _secondsPerBeat;
            _noteSources[n].clip = _audioManager.GetMidiClip(_midiPitches[n]);
            _noteSources[n].PlayScheduled(scheduledTime);

            if(n == 0)
            {
                _targetHitTime = scheduledTime + _travelDuration;
                _activeTile.Initialize(_targetHitTime, _travelDuration, _spawnPoint.position, _hitPoint.position);
            }
        }
    }

    public void ResolveBlock()
    {
        _awaitingBlock = false;
        _blockSucceeded = true;
        _audioManager.PlayBlockSuccess();
        Destroy(_activeTile.gameObject);
    }

    public void ResolveMiss()
    {
        _awaitingBlock = false;
        _blockSucceeded = false;
        Destroy(_activeTile.gameObject);
    }

    public bool IsAwaitingBlock()
    {
        return _awaitingBlock;
    }

    public bool WasBlockSuccessful()
    {
        return _blockSucceeded;
    }
}

