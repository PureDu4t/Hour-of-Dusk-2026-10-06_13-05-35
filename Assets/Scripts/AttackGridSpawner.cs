using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System;

public class AttackGridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _square;
    [SerializeField] private int[] _pitches = { 1, 2, 3, 4, 5, 6, 7, 8 };
    [SerializeField] private Grid _grid;
    [SerializeField] private float _flashDuration = 0.2f;
    [SerializeField] private AudioManager _audioManager;
    private SpriteRenderer[] _noteTiles;
    private Color _originalColor;
    private float _elapsedTime = 0f;
    private int _currentNoteIndex = 0;
    private bool _attacking = false;
    private bool _hitEffectPlaying = false;
    private bool[] _blockEliminated;
    public int CurrentPitch()
    {
        return _pitches[_currentNoteIndex];
    }

    void Awake()
    {
        EnemyPatternGenerator generator = GetComponent<EnemyPatternGenerator>();
        if (generator != null)
        {
            _pitches = generator.RandomizePitches(); 
            GetComponent<AttackController>()._weakPointIndex[0] = UnityEngine.Random.Range(0, _pitches.Length);
        }
        else
        {
            return;
        }

        _grid = GetComponent<Grid>();
        SpawnBlocks();
        _blockEliminated = new bool[_pitches.Length];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

        // Update is called once per frame
    void Update()
    {
        if (_attacking)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _flashDuration)
            {
                RestoreTileColor();
                _currentNoteIndex++;
                if (_currentNoteIndex >= _pitches.Length)
                {
                    _currentNoteIndex = 0;
                }
                _elapsedTime -= _flashDuration;
                ActivateCurrentNote();
            }
        }
    }

    public bool RegisterHit(bool isWeakPoint)
    {
        // Check if the attack is currently active and if the hit effect is not already playing
        if (!_attacking || _hitEffectPlaying)
        {
            return false;
        }

        // Check if the current note is valid (not a rest and has a corresponding tile)
        if (_pitches[_currentNoteIndex] == 0 ||
            _noteTiles[_currentNoteIndex] == null)
        {
            _attacking = false;
            Debug.Log("Miss!");
            GetComponent<AttackController>().OnAttackFinished();
            _audioManager.PlayAttackMiss();
            return false;
        }

        _attacking = false;

        if (!isWeakPoint)
        {
            _blockEliminated[_currentNoteIndex] = true;
        }

        // Start the hit effect coroutine and set the flag to indicate that the hit effect is playing
        _hitEffectPlaying = true;
        StartCoroutine(PlayHitEffect(
            _noteTiles[_currentNoteIndex],
            _originalColor,
            isWeakPoint));

            return true;
    }

    private IEnumerator PlayHitEffect(
    SpriteRenderer tile, Color normalColor, bool isWeakPoint)
    {
        tile.color = Color.red;
        yield return new WaitForSeconds(0.1f);


        if (tile != null)
        {
            if (isWeakPoint)
            {
                tile.color = normalColor;
            }
            else
            {
                Destroy(tile.gameObject);
            }
        }

        _hitEffectPlaying = false;
        GetComponent<AttackController>().OnAttackFinished();
    }

    private void RestoreTileColor()
    {
        if (_pitches[_currentNoteIndex] == 0 || _noteTiles[_currentNoteIndex] == null)
        {
            return;
        }

        _noteTiles[_currentNoteIndex].color = _originalColor;
    }

    private void ActivateCurrentNote()
    {
        if (_pitches[_currentNoteIndex] == 0 || _noteTiles[_currentNoteIndex] == null)
        {
            return;
        }
        else
        {
            _originalColor = _noteTiles[_currentNoteIndex].color;
            _noteTiles[_currentNoteIndex].color = Color.yellow;

            if (_currentNoteIndex == GetComponent<AttackController>()._weakPointIndex[0])
            {
                _audioManager.PlayWeakPitch(_pitches[_currentNoteIndex]);
            }
            else
            {
                _audioManager.PlayPitch(_pitches[_currentNoteIndex]);
            }
        }
    }

    private void SpawnBlocks()
    {
        _noteTiles = new SpriteRenderer[_pitches.Length];
        int[] weakPointIndices = GetComponent<AttackController>()._weakPointIndex;
        for (int i = 0; i < _pitches.Length; i++)
        {   
            if (_pitches[i] == 0)
                {
                    continue;
                }

            Vector3Int cell = new Vector3Int(i, _pitches[i] - 1, 0);
            //pitches[i] - 1 gets the vertical hight, and since i adds 1 to its own value after each loop, the x position will also increment by 1.
            Vector3 position = _grid.GetCellCenterWorld(cell);
            GameObject block = Instantiate(_square, position, Quaternion.identity, transform);
            _noteTiles[i] = block.GetComponent<SpriteRenderer>();
            foreach (int j in weakPointIndices)
            {
                if (j == i)
                {
                    _noteTiles[j].color = Color.cyan;
                    break;
                }
            }
        }
    }

    public int ExposeCurrentNoteIndex()
    {
        Debug.Log("Current Note Index: " + _currentNoteIndex);
        return _currentNoteIndex;
    }
    public void StartAttack()
    {
        
        if (_attacking || _hitEffectPlaying ||
            _pitches.Length == 0 || _flashDuration <= 0f)
        {
            return;
        }

        _audioManager.PlayUIStinger();
        _attacking = true;
        _elapsedTime = 0f;
        _currentNoteIndex = 0;
        ActivateCurrentNote();
        
    }
    public void DestroyBlocks()
    {
        Destroy(_noteTiles[_currentNoteIndex].gameObject, 0.1f);
    }

    public void DestroyAllBlocks()
    {
        _attacking = false;
        foreach (SpriteRenderer tile in _noteTiles)
        {
            if (tile == null)
            {
            continue;
            }
            else
            {
               Destroy(tile.gameObject);
            }
        }
    }
}
