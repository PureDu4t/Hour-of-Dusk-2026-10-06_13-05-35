using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttackTile : MonoBehaviour
{
    private double _scheduledHitTime;
    private float _travelDuration;
    private Vector3 _spawnPosition;
    private Vector3 _hitPosition;
    private SpriteRenderer _spriteRenderer;
    public void Initialize(double hitTime, float travelDuration, Vector3 spawnPosition, Vector3 hitPosition)
    {
        _scheduledHitTime = hitTime;
        _travelDuration = travelDuration;
        _spawnPosition = spawnPosition;
        _hitPosition = hitPosition;
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        double _spawnTime = _scheduledHitTime - _travelDuration;
        _spriteRenderer.enabled = AudioSettings.dspTime >= _spawnTime;
        double _progress = (AudioSettings.dspTime - _spawnTime) / _travelDuration;
        transform.position = Vector3.Lerp(_spawnPosition, _hitPosition, (float)_progress);
    }
}
