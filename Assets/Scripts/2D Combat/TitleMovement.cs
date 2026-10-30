using UnityEngine;
using TMPro;
using System.Diagnostics;

public class TitleMovement : MonoBehaviour
{
    [SerializeField] private TMP_Text _mainTitle;
    [SerializeField] private float _titleSpeed = 5f;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.linearVelocity = Vector2.left * _titleSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        _rb.linearVelocity = Vector2.zero;
        _audioSource.PlayOneShot(_clip);
    }
}
